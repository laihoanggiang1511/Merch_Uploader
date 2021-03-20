using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using EzUpload.Actions.Chrome;
using EzUpload.DataAccess;
using EzUpload.DataAccess.DTO;
using EzUpload.DataAccess.Model;
using EzUpload.ViewModel;

namespace EzUpload.Actions
{
    public class AutoUpload
    {

        BlockingCollection<KeyValuePair<string, Shirt>> shirtQueue = new BlockingCollection<KeyValuePair<string, Shirt>>();
        string userFolderPath;
        string email;
        string password;
        readonly int dailyUploadLimit;
        DateTime startTime;
        int countUploadToday;
        public Action<UploadWindowViewModel, string> WriteLog;
        UploadWindowViewModel uploadVM;

        public AutoUpload(UploadWindowViewModel uploadVM)
        {
            this.userFolderPath = uploadVM.UserFolderPath;
            this.email = uploadVM.Email;
            this.password = uploadVM.password;
            this.dailyUploadLimit = uploadVM.DailyUploadLimit;
            this.uploadVM = uploadVM;
            WriteLog = new Action<UploadWindowViewModel, string>((x, y) => Console.WriteLine(y));
        }
        public void StartWatching(string folderToWatch)
        {
            WriteLog.Invoke(uploadVM, $"Waiting for files in folder: {folderToWatch}");

            //Scan for firstTime
            string[] filesInDir = Directory.GetFiles(folderToWatch, "*.*", SearchOption.TopDirectoryOnly);
            foreach (string file in filesInDir)
            {
                AddToQueue(file);
            }

            startTime = Utils.ConvertToLATime(System.DateTime.Now).Date;
            Thread thread = new Thread(new ThreadStart(OnStartUpload));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();

            Thread threadCheckLimit = new Thread(new ThreadStart(CheckDailyLimitUpload));
            threadCheckLimit.IsBackground = true;
            threadCheckLimit.Start();

            using (FileSystemWatcher watcher = new FileSystemWatcher())
            {
                watcher.Path = folderToWatch;
                // Watch for changes in LastAccess and LastWrite times, and
                // the renaming of files or directories.
                watcher.NotifyFilter = NotifyFilters.LastAccess
                                     | NotifyFilters.LastWrite
                                     | NotifyFilters.FileName;

                watcher.Filter = "*.*";
                // Add event handlers.
                watcher.Created += OnFileChanged;
                // Begin watching.
                watcher.EnableRaisingEvents = true;
                while (true)
                {
                    Thread.Sleep(100000);
                }
            }
        }

        private void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            string filePath = e.FullPath;
            AddToQueue(filePath);
        }

        private void AddToQueue(string filePath)
        {
            Shirt shirt = null;
            if (Path.GetExtension(filePath).ToLower() == ".json")
            {
                JsonDataAccess jsonData = new JsonDataAccess();
                ShirtData sData = jsonData.ReadShirt(filePath);
                shirt = ShirtDTO.MapData(sData, typeof(Shirt)) as Shirt;
            }
            else if (Path.GetExtension(filePath).ToLower() == ".xml")
            {
                XMLDataAccess xmlData = new XMLDataAccess();
                ShirtData sData = xmlData.ReadShirt(filePath);
                shirt = ShirtDTO.MapData(sData, typeof(Shirt)) as Shirt;
            }
            if (shirt != null)
            {
                WriteLog.Invoke(uploadVM, $"File added: {filePath} ");
                shirtQueue.Add(new KeyValuePair<string, Shirt>(filePath, shirt));
            }
        }

        private void CheckDailyLimitUpload()
        {
            while (true)
            {
                Thread.Sleep(10000);
                DateTime today = Utils.ConvertToLATime(System.DateTime.Now).Date;
                if (!today.Equals(startTime))
                {
                    countUploadToday = 0;
                    startTime = today;
                }
            }
        }

        private void OnStartUpload()
        {
            foreach (var pair in shirtQueue.GetConsumingEnumerable(CancellationToken.None))
            {
                try
                {
                    while (countUploadToday >= dailyUploadLimit)
                    {
                        //block upload
                        Thread.Sleep(10000);
                    }
                    Shirt shirt = pair.Value;
                    string jsonFullFileName = pair.Key;
                    string dir = Path.GetDirectoryName(jsonFullFileName);
                    string jsonFileName = Path.GetFileName(jsonFullFileName);
                    WriteLog.Invoke(uploadVM, $"Uploading {jsonFileName}");
                    UploadMerch upload = new UploadMerch(password, email);
                    upload.OpenChrome(userFolderPath);
                    if (UploadMerch.driver != null)
                    {
                        UploadMerch.driver.Navigate().GoToUrl("https://merch.amazon.com/designs/new");
                        bool uploadSuccess = upload.LogIn();
                        if (uploadSuccess)
                        {
                            uploadSuccess = upload.Upload(shirt);
                        }

                        UploadMerch.QuitDriver();
                        //Copy to success or fail folder
                        string dateFolder = Utils.ConvertToLATime(DateTime.Now).ToString("yyyy-MM-dd");
                        if (uploadSuccess == true)
                        {
                            string successDir = Path.Combine(dir, "Success", dateFolder);
                            CutFile(Path.GetFileName(jsonFullFileName), dir, successDir);
                            CutFile(Path.GetFileName(shirt.ImagePath), dir, successDir);
                            countUploadToday++;
                            WriteLog.Invoke(uploadVM, $"Upload Success {jsonFileName}");
                            WriteLog.Invoke(uploadVM, $"Uploaded today: {countUploadToday}");
                        }
                        else
                        {
                            WriteLog.Invoke(uploadVM, $"Upload Fail: {jsonFileName}");
                            string failDir = Path.Combine(dir, "Fail", dateFolder);
                            CutFile(Path.GetFileName(jsonFullFileName), dir, failDir);
                            CutFile(Path.GetFileName(shirt.ImagePath), dir, failDir);
                        }
                    }
                }
                catch (Exception ex)
                {
                    WriteLog.Invoke(uploadVM, ex.Message);
                }
            }
        }
        private void CutFile(string fileName, string sourceFolder, string destinationFolder)
        {
            try
            {
                if (!Directory.Exists(destinationFolder))
                {
                    Directory.CreateDirectory(destinationFolder);
                }
                string source = Path.Combine(sourceFolder, fileName);
                string destination = Path.Combine(destinationFolder, fileName);
                if (File.Exists(source))
                {
                    File.Copy(source, destination, true);
                    File.Delete(source);
                }
            }
            catch (Exception ex)
            {
                WriteLog.Invoke(uploadVM, ex.Message);
            }
        }
    }

}
