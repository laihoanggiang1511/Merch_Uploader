using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using EzUpload.DataAccess;
using EzUpload.GUI;
using EzUpload.ViewModel;
using Common.MVVMCore;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Diagnostics;
using OpenQA.Selenium.Internal;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Controls.Primitives;
using Microsoft.Office.Interop.Excel;
using EzUpload.Actions.Chrome;
using EzUpload.DataAccess.Model;
using EzUpload.DataAccess.DTO;
using Common;

namespace EzUpload.Actions
{
    public class UploadActions
    {
        UploadWindow uploadWindow = null;

        public void ShowWindow(UploadPlatform uploadPlatform = UploadPlatform.Merch)
        {
            uploadWindow = new UploadWindow();
            uploadWindow.Closed += this.OnExit;
            string dataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            dataFolder += $"\\{Constants.PRODUCT_NAME}\\UserFolders";
            UploadWindowViewModel mainVM = new UploadWindowViewModel(dataFolder)
            {
                BrowseCmd = new RelayCommand(BrowseCmdInvoke),
                OpenChromeCmd = new RelayCommand(OpenChromeCmdInvoke),
                DeleteCmd = new RelayCommand(DeleteCmdInvoke),
                UploadCmd = new RelayCommand(UploadCmdInvoke),
                AbortCmd = new RelayCommand(AbortCmdInvoke),
                ChooseFolderCmd = new RelayCommand(ChooseFolderCmdInvoke),
                SaveXmlCmd = new RelayCommand(SaveXmlCmdInvoke),
                EditShirtCmd = new RelayCommand(EditShirtCmdInvoke),
                ShowConfigurationCmd = new RelayCommand(ShowConfigurationCmdInvoke),
                RemoveFolderCmd = new RelayCommand(RemoveFolderCmdInvoke),
                FolderChangeCmd = new RelayCommand(LoadProperties),
                BrowseUploadFolderCmd = new RelayCommand(BrowseUploadFolderCmdInvoke),
                StartAutoUploadCmd = new RelayCommand(AutoUploadCmdInvoke),
            };
            mainVM.UploadPlatform = uploadPlatform;
            mainVM.UserFolders = GetUserFolders();
            mainVM.SelectedPath = mainVM.UserFolders.FirstOrDefault(x => EzUpload.Properties.Settings.Default.UserFolderPath.EndsWith(x));
            uploadWindow.DataContext = mainVM;
            uploadWindow.Show();
        }

        private void BrowseUploadFolderCmdInvoke(object obj)
        {
            if (obj is UploadWindowViewModel mainVM)
            {
                using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
                {

                    folderDialog.ShowNewFolderButton = true;
                    folderDialog.Description = "Browse for Image Folder";
                    if (folderDialog.ShowDialog() == DialogResult.OK)
                    {
                        mainVM.UploadFolder = folderDialog.SelectedPath;
                    }
                }
            }
        }
        private void SaveProperties(object obj)
        {
            if (obj is UploadWindowViewModel uploadVM)
            {
                try
                {
                    if (string.IsNullOrEmpty(uploadVM.UserFolderPath))
                        return;
                    string dataFileName = Path.Combine(uploadVM.UserFolderPath, "MerchUploadData.json");
                    if (File.Exists(dataFileName))
                    {
                        File.Delete(dataFileName);
                    }
                    string email = Common.Crypt.Encrypt(uploadVM.Email, true);
                    string password = Common.Crypt.Encrypt(GetPassword(uploadWindow), true);
                    string uploadFolder = uploadVM.UploadFolder;
                    string dailyUploadLimit = uploadVM.DailyUploadLimit.ToString();

                    string[] content = new string[] { email, password, uploadFolder, dailyUploadLimit };
                    File.WriteAllLines(dataFileName, content);
                }
                catch (Exception ex)
                {
                    Common.Log.log.Fatal(ex);
                }
            }

        }

        private void LoadProperties(object obj)
        {
            if (obj is UploadWindowViewModel uploadVM)
            {
                if (string.IsNullOrEmpty(uploadVM.UserFolderPath))
                    return;
                uploadVM.Email = string.Empty;
                SetPassWord(uploadWindow, string.Empty);
                string dataFileName = Path.Combine(uploadVM.UserFolderPath, "MerchUploadData.json");
                if (File.Exists(dataFileName))
                {
                    string[] data = File.ReadAllLines(dataFileName);
                    if (data != null && data.Length == 4)
                    {
                        uploadVM.Email = Common.Crypt.Decrypt(data[0], true);
                        SetPassWord(uploadWindow, Common.Crypt.Decrypt(data[1], true));
                        uploadVM.UploadFolder = data[2];
                        int dailyUploadLimit;
                        if (int.TryParse(data[3], out dailyUploadLimit))
                        {
                            uploadVM.DailyUploadLimit = dailyUploadLimit;
                        }
                    }
                }
            }
        }

        private void RemoveFolderCmdInvoke(object obj)
        {
            if (obj is UploadWindowViewModel uploadVM)
            {
                if (Directory.Exists(uploadVM.UserFolderPath))
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(uploadVM.UserFolderPath);
                    directoryInfo.Delete(true);
                }
                uploadVM.UserFolders = GetUserFolders();
                uploadVM.SelectedPath = uploadVM.UserFolders.FirstOrDefault();
            }
        }

        private void ShowConfigurationCmdInvoke(object obj)
        {
            if (obj is UploadWindowViewModel uploadVM)
            {
                uploadVM.ShowConfiguration = !uploadVM.ShowConfiguration;
            }
        }

        public string GetDataDirectory()
        {
            string dataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            dataFolder += $"\\{Constants.PRODUCT_NAME}\\UserFolders";
            return dataFolder;
        }

        public ObservableCollection<string> GetUserFolders()
        {
            ObservableCollection<string> result = new ObservableCollection<string>();
            string dataFolder = GetDataDirectory();
            if (!Directory.Exists(dataFolder))
            {
                Directory.CreateDirectory(dataFolder);
            }
            string[] userFolders = Directory.GetDirectories(dataFolder);
            if (userFolders != null && userFolders.Length > 0)
            {
                foreach (string userFolder in userFolders)
                {
                    if (!string.IsNullOrEmpty(userFolder) && userFolder.Contains("\\"))
                    {
                        result.Add(userFolder.Split('\\').Last());
                    }

                }
            }
            return result;
        }

        private void EditShirtCmdInvoke(object obj)
        {
            if (obj is UploadWindowViewModel uploadVM)
            {
                ShirtCreatorView shirtCreatorView = new ShirtCreatorView();
                ShirtCreatorViewModel editShirtVM = new ShirtCreatorViewModel();
                ShirtCreatorActions shirtCreatorActions = new ShirtCreatorActions();
                if (uploadVM.SelectedShirt != null)
                    shirtCreatorActions.ShowWindow(uploadVM.SelectedShirt);
            }
        }

        private void AbortCmdInvoke(object obj)
        {
            //if (thread != null)
            //{

            //    if (mainVM != null)
            //    {
            //        mainVM.IsUploading = false;
            //        thread.Abort();
            //    }

            //}
        }

        public void OnExit(object sender, EventArgs e)
        {
            try
            {
                UploadWindow wind = sender as UploadWindow;
                if (wind != null)
                {
                    UploadWindowViewModel uploadVM = wind.DataContext as UploadWindowViewModel;
                    SaveProperties(uploadVM);
                    //Save Setting
                    EzUpload.Properties.Settings.Default.UserFolderPath = uploadVM.UserFolderPath;
                    EzUpload.Properties.Settings.Default.Save();
                    ChromeHelper.QuitDriver();
                }
            }
            catch
            {
            }
            finally
            {
                Environment.Exit(0);
            }
        }

        private void SaveXmlCmdInvoke(object obj)
        {
            UploadWindowViewModel mainVM = obj as UploadWindowViewModel;
            JsonDataAccess xMLDataAccess = new JsonDataAccess();
            foreach (Shirt s in mainVM.Shirts)
            {
                ShirtData sData = ShirtDTO.MapData(s, typeof(ShirtData)) as ShirtData;
                xMLDataAccess.SaveShirt(sData);
            }
            System.Windows.MessageBox.Show("Saved!");
        }

        private void ChooseFolderCmdInvoke(object obj)
        {
            if (obj is UploadWindowViewModel uploadVM)
            {
                if (!string.IsNullOrEmpty(uploadVM.AddFolderName))
                {
                    char[] invalidChars = Path.GetInvalidPathChars();
                    if (!invalidChars.ToList().Any(x => uploadVM.AddFolderName.Contains(x) == true))
                    {
                        string dataFolder = GetDataDirectory();
                        string folderPath = Path.Combine(dataFolder, uploadVM.AddFolderName);
                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                            uploadVM.UserFolders = GetUserFolders();
                            uploadVM.RaisePropertyChanged("UserFolders");
                            //uploadVM.UserFolderPath = folderPath;
                            uploadVM.SelectedPath = uploadVM.UserFolders.Select(x => x = uploadVM.AddFolderName).FirstOrDefault();
                        }
                        else
                        {
                            ChromeHelper.ShowErrorMessageBox("Name already exist, please choose a new name");
                        }
                    }
                    else
                    {
                        ChromeHelper.ShowErrorMessageBox("Name contains invalid character, please choose a new name");
                    }
                }
                else
                {
                    ChromeHelper.ShowErrorMessageBox("Folder name is empty");
                }
                //uploadVM.UserFolderPath
                //if (!string.IsNullOrEmpty(uploadVM.UserFolderPath) && uploadVM.UserFolderPath .
            }
        }
        private void OpenChromeCmdInvoke(object obj)
        {

            if (obj is UploadWindow uploadWind)
            {
                UploadWindowViewModel uploadVM = uploadWind.DataContext as UploadWindowViewModel;
                uploadVM.password = GetPassword(uploadWind);
                if (uploadVM.IsUploading == true)
                {
                    ChromeHelper.ShowInfoMessageBox("Another proccess is running");
                    return;
                }
                if (!Directory.Exists(uploadVM.UserFolderPath))
                {
                    ChromeHelper.ShowErrorMessageBox("User folder does not exist");
                    return;
                }

                ParameterizedThreadStart paramsThreadStart = new ParameterizedThreadStart(ManualLogInCallBack);
                Thread thrd = new Thread(paramsThreadStart);
                thrd.Start(uploadVM);
            }
        }
        public void ManualLogInCallBack(object obj)
        {
            if (obj is UploadWindowViewModel uploadVM)
            {
                try
                {
                    IUpload upload = null;
                    if (uploadVM.UploadPlatform == UploadPlatform.Merch)
                    {
                        upload = new UploadMerch(uploadVM.password, uploadVM.Email);
                    }
                    else if (uploadVM.UploadPlatform == UploadPlatform.TeePublic)
                    {
                        upload = new UploadTeePublic();
                    }
                    upload.OpenChrome(uploadVM.UserFolderPath);
                    upload.GoToUploadPage();
                    upload.LogIn();
                }
                catch (Exception ex)
                {
                    Log.log.Fatal(ex);
                }
            }

        }
        private void UploadCmdInvoke(object obj)
        {
            UploadWindow mainWindow = obj as UploadWindow;
            if (mainWindow != null)
            {
                UploadWindowViewModel mainVM = mainWindow.DataContext as UploadWindowViewModel;
                mainVM.password = GetPassword(mainWindow);

                if (mainVM != null)
                {
                    if (mainVM.IsUploading == true)
                    {
                        ChromeHelper.ShowInfoMessageBox("Another proccess is running!");
                        return;
                    }
                    if (ValidateShirt(mainVM.Shirts.ToList()))
                    {
                        Thread thread = new Thread(UploadShirt);
                        thread.SetApartmentState(ApartmentState.STA);
                        thread.Start(mainVM);
                        mainVM.IsUploading = true;
                    }
                }
            }
        }

        private bool ValidateShirt(List<Shirt> shirts)
        {
            Dictionary<Shirt, ShirtStatus> invalidShirts = new Dictionary<Shirt, ShirtStatus>();
            bool bContinue = true;
            foreach (Shirt s in shirts)
            {
                ShirtStatus errorCode = 0;
                if (!ShirtCreatorActions.ValidateShirt(s, ref errorCode))
                {
                    invalidShirts.Add(s, errorCode);
                    bContinue = false;
                }
            }
            if (bContinue == false)
            {
                string message = "Following shirt(s) are invalid, please check them again before upload:\n";
                foreach (var pair in invalidShirts)
                {
                    message += pair.Key.ImagePath + ":\n" + ShirtCreatorActions.GetErrorMessage(pair.Value) + "\n";
                }
                Utils.ShowErrorMessageBox(message);
            }
            return bContinue;
        }

        private void UploadShirt(object obj)
        {
            List<Shirt> failShirts = new List<Shirt>();
            UploadWindowViewModel mainVM = obj as UploadWindowViewModel;
            if (mainVM != null)
            {
                if (mainVM.Shirts != null && mainVM.Shirts.Count > 0)
                {
                    try
                    {
                        if (Directory.Exists(mainVM.UserFolderPath))
                        {
                            IUpload upload = null;
                            if (mainVM.UploadPlatform == UploadPlatform.Merch)
                            {
                                upload = new UploadMerch(mainVM.password, mainVM.Email);
                            }
                            else if(mainVM.UploadPlatform == UploadPlatform.TeePublic)
                            {
                                //TODO Tee   constructor
                            }
                            upload.OpenChrome(mainVM.UserFolderPath);
                            upload.GoToUploadPage();
                            upload.LogIn();
                            for (int i = 0; i < mainVM.Shirts.Count; i++)
                            {
                                mainVM.SelectedShirt = mainVM.Shirts[i];
                                if (!upload.Upload(mainVM.Shirts[i]))
                                    failShirts.Add(mainVM.Shirts[i]);
                            }
                            upload.QuitDriver();
                            if (failShirts.Count == 0)
                            {
                                System.Windows.MessageBox.Show("Job Done!");
                            }
                            else
                            {
                                string message = "Fail to upload following shirt(s):\n";
                                foreach (Shirt shirt in failShirts)
                                    message += shirt.ImagePath + "\n";
                                System.Windows.MessageBox.Show(message);
                            }

                        }
                        else
                        {
                            ChromeHelper.ShowErrorMessageBox("User folder does not exist");
                        }
                    }
                    catch (Exception ex)
                    {
                        Utils.ShowErrorMessageBox(ex.Message);
                    }

                }
                mainVM.IsUploading = false;
            }
        }

        private void DeleteCmdInvoke(object obj)
        {
            while ((obj as UploadWindowViewModel).SelectedShirt != null)
            {
                (obj as UploadWindowViewModel).Shirts.Remove((obj as UploadWindowViewModel).SelectedShirt);
            }
        }
        private void BrowseCmdInvoke(object obj)
        {
            if (obj != null)
            {
                UploadWindowViewModel mainVM = obj as UploadWindowViewModel;
                List<Shirt> lstShirts = Utils.BrowseForShirts();
                lstShirts.ForEach(x => mainVM.Shirts.Add(x));
            }
        }

        private void AutoUploadCmdInvoke(object obj)
        {
            UploadWindow mainWindow = obj as UploadWindow;
            if (mainWindow != null)
            {
                UploadWindowViewModel mainVM = mainWindow.DataContext as UploadWindowViewModel;
                mainVM.password = GetPassword(mainWindow);
                if (mainVM != null)
                {
                    if (mainVM.IsUploading == false)
                    {
                        mainVM.IsUploading = true;
                        if (!Directory.Exists(mainVM.UploadFolder))
                        {
                            Utils.ShowErrorMessageBox("Upload Folder does not exist!");
                            return;
                        }
                        Thread thread = new Thread(x =>
                        {
                            AutoUpload autoUpload = new AutoUpload(mainVM);
                            autoUpload.WriteLog = WriteAutoUploadLog;
                            autoUpload.StartWatching(mainVM.UploadFolder);
                        });
                        thread.SetApartmentState(ApartmentState.STA);
                        thread.Start();
                    }
                    else
                    {
                        Utils.ShowErrorMessageBox("Already Running!");
                    }
                }
            }
        }

        private void WriteAutoUploadLog(UploadWindowViewModel mainVM, string log)
        {
            if (mainVM.AutoUploadLog.Length > 1000000)
            {
                mainVM.AutoUploadLog = string.Empty;
            }
            mainVM.AutoUploadLog += System.DateTime.Now.ToString("yyyy-MMM-dd-HH:mm:ss") + ": " + log + "\n";
        }

        private string GetPassword(UploadWindow uploadWind)
        {
            try
            {
                uploadWind.FindName("password");
                PasswordBox passwordBox = uploadWind.FindName("password") as PasswordBox;
                return passwordBox.Password;
            }
            catch
            {
                return string.Empty;
            }

        }
        private bool SetPassWord(UploadWindow uploadWind, string password)
        {
            try
            {
                uploadWind.FindName("password");
                PasswordBox passwordBox = uploadWind.FindName("password") as PasswordBox;
                passwordBox.Password = password;
                return true;
            }
            catch
            {
                return false;
            }

        }

    }
}

