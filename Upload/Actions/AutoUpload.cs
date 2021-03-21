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

        private BlockingCollection<KeyValuePair<string, Shirt>> _shirtQueue = new BlockingCollection<KeyValuePair<string, Shirt>>();
        private string _userFolderPath;
        private string _email;
        private string _password;
        private readonly int _dailyUploadLimit;
        private DateTime _startTime;
        private int _countUploadToday;
        public Action<UploadWindowViewModel, string> WriteLog;
        private UploadWindowViewModel _uploadVM;

        public AutoUpload(UploadWindowViewModel uploadVM)
        {
            this._userFolderPath = uploadVM.UserFolderPath;
            this._email = uploadVM.Email;
            this._password = uploadVM.password;
            this._dailyUploadLimit = uploadVM.DailyUploadLimit;
            this._uploadVM = uploadVM;
            WriteLog = new Action<UploadWindowViewModel, string>((x, y) => Console.WriteLine(y));
        }
        public void StartWatching(string folderToWatch)
        {
            WriteLog.Invoke(_uploadVM, $"Waiting for files in folder: {folderToWatch}");

            //Scan for firstTime
            string[] filesInDir = Directory.GetFiles(folderToWatch, "*.*", SearchOption.TopDirectoryOnly);
            foreach (string file in filesInDir)
            {
                AddToQueue(file);
            }

            _startTime = Utils.ConvertToLATime(System.DateTime.Now).Date;
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
                WriteLog.Invoke(_uploadVM, $"File added: {filePath} ");
                _shirtQueue.Add(new KeyValuePair<string, Shirt>(filePath, shirt));
            }
        }

        private void CheckDailyLimitUpload()
        {
            while (true)
            {
                Thread.Sleep(10000);
                DateTime today = Utils.ConvertToLATime(System.DateTime.Now).Date;
                if (!today.Equals(_startTime))
                {
                    _countUploadToday = 0;
                    _startTime = today;
                }
            }
        }

        private void OnStartUpload()
        {
            foreach (var pair in _shirtQueue.GetConsumingEnumerable(CancellationToken.None))
            {
                try
                {
                    while (_countUploadToday >= _dailyUploadLimit)
                    {
                        //block upload
                        Thread.Sleep(10000);
                    }
                    Shirt shirt = pair.Value;
                    string jsonFullFileName = pair.Key;
                    string dir = Path.GetDirectoryName(jsonFullFileName);
                    string jsonFileName = Path.GetFileName(jsonFullFileName);
                    WriteLog.Invoke(_uploadVM, $"Uploading {jsonFileName}");
                    
                    IUpload upload = null;
                    if(_uploadVM.UploadPlatform == UploadPlatform.Merch)
                    {
                        upload = new UploadMerch(_password, _email);
                    }
                    else
                    {
                        //TODO teepublic
                    }

                    upload.OpenChrome(_userFolderPath);
                    upload.GoToUploadPage();
                    bool uploadSuccess = upload.LogIn();
                    if (uploadSuccess)
                    {
                        uploadSuccess = upload.Upload(shirt);
                    }
                    upload.QuitDriver();

                    //Copy to success or fail folder
                    string dateFolder = Utils.ConvertToLATime(DateTime.Now).ToString("yyyy-MM-dd");
                    if (uploadSuccess == true)
                    {
                        string successDir = Path.Combine(dir, "Success", dateFolder);
                        CutFile(Path.GetFileName(jsonFullFileName), dir, successDir);
                        CutFile(Path.GetFileName(shirt.ImagePath), dir, successDir);
                        _countUploadToday++;
                        WriteLog.Invoke(_uploadVM, $"Upload Success {jsonFileName}");
                        WriteLog.Invoke(_uploadVM, $"Uploaded today: {_countUploadToday}");
                    }
                    else
                    {
                        WriteLog.Invoke(_uploadVM, $"Upload Fail: {jsonFileName}");
                        string failDir = Path.Combine(dir, "Fail", dateFolder);
                        CutFile(Path.GetFileName(jsonFullFileName), dir, failDir);
                        CutFile(Path.GetFileName(shirt.ImagePath), dir, failDir);
                    }

                }
                catch (Exception ex)
                {
                    WriteLog.Invoke(_uploadVM, ex.Message);
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
                WriteLog.Invoke(_uploadVM, ex.Message);
            }
        }
    }

}
