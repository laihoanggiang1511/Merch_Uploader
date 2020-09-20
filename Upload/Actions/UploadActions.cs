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
using Upload.DataAccess;
using Upload.GUI;
using Upload.ViewModel;
using Common.MVVMCore;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Diagnostics;
using OpenQA.Selenium.Internal;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Controls.Primitives;
using Microsoft.Office.Interop.Excel;
using Upload.Actions.Chrome;
using Upload.DataAccess.Model;
using Upload.DataAccess.DTO;

namespace Upload.Actions
{
    public class UploadActions
    {

        public void ShowWindow()
        {
            UploadWindow uploadWindow = new UploadWindow();
            uploadWindow.Closed += this.OnExit;
            UploadWindowViewModel mainVM = new UploadWindowViewModel
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
                Email = Common.Crypt.Decrypt(Properties.Settings.Default.Email, true),
            };
            mainVM.UserFolders = GetUserFolders();
            mainVM.SelectedPath = mainVM.UserFolders.FirstOrDefault(x => Properties.Settings.Default.UserFolderPath.EndsWith(x));
            mainVM.Email = Common.Crypt.Decrypt(Properties.Settings.Default.Email, true);

            SetPassWord(uploadWindow, Common.Crypt.Decrypt(Properties.Settings.Default.Password, true));
            uploadWindow.DataContext = mainVM;
            uploadWindow.Show();
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
            dataFolder += "\\Upload\\UserFolders";
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
                    shirtCreatorActions.ShowShirtCreatorWindow(uploadVM.SelectedShirt);
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
                    //Save Setting
                    Properties.Settings.Default.UserFolderPath = uploadVM.UserFolderPath;
                    Properties.Settings.Default.Email = Common.Crypt.Encrypt(uploadVM.Email, true);
                    PasswordBox passwordBox = wind.FindName("password") as PasswordBox;
                    if (passwordBox != null)
                    {
                        Properties.Settings.Default.Password = Common.Crypt.Encrypt(passwordBox.Password, true);
                    }
                    Properties.Settings.Default.Save();
                    UploadMerch.QuitDriver();
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
                if (!string.IsNullOrEmpty(uploadVM.UserFolderPath))
                {
                    char[] invalidChars = Path.GetInvalidPathChars();
                    if (!invalidChars.ToList().Any(x => uploadVM.UserFolderPath.Contains(x) == true))
                    {
                        string dataFolder = GetDataDirectory();
                        string folderPath = Path.Combine(dataFolder, uploadVM.UserFolderPath);
                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                            uploadVM.UserFolders = GetUserFolders();
                            uploadVM.RaisePropertyChanged("UserFolders");
                            uploadVM.UserFolderPath = folderPath;
                        }
                        else
                        {
                            Helper.ShowErrorMessageBox("Name already exist, please choose a new name");
                        }
                    }
                    else
                    {
                        Helper.ShowErrorMessageBox("Name contains invalid character, please choose a new name");
                    }
                }
                else
                {
                    Helper.ShowErrorMessageBox("Folder name is empty");
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
                    Helper.ShowInfoMessageBox("Another proccess is running");
                    return;
                }
                if (!Directory.Exists(uploadVM.UserFolderPath))
                {
                    Helper.ShowErrorMessageBox("User folder does not exist");
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
                    UploadMerch upload = new UploadMerch(uploadVM.password, uploadVM.Email);
                    upload.OpenChrome(uploadVM.UserFolderPath);
                    UploadMerch.driver.Navigate().GoToUrl("https://merch.amazon.com/designs/new");
                    upload.Log_In();
                    if(UploadMerch.driver.Url.Contains("merch.amazon.com/designs/new"))
                    {
                        Helper.ShowInfoMessageBox("Log in Sucess!");
                        uploadVM.IsUploading = false;
                    }
                }
                catch
                { 
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
                        Helper.ShowInfoMessageBox("Another proccess is running!");
                        return;
                    }
                    Thread thread = new Thread(UploadShirt);
                    thread.Start(mainVM);
                    mainVM.IsUploading = true;

                }
            }
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

                        Dictionary<Shirt, ShirtStatus> invalidShirts = new Dictionary<Shirt, ShirtStatus>();
                        bool bContinue = true;
                        foreach (Shirt s in mainVM.Shirts)
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
                            return;
                        }

                        UploadMerch upload = new UploadMerch(mainVM.password, mainVM.Email);

                        if (Directory.Exists(mainVM.UserFolderPath))
                        {
                            upload.OpenChrome(mainVM.UserFolderPath);
                            if (UploadMerch.driver != null)
                            {
                                UploadMerch.driver.Navigate().GoToUrl("https://merch.amazon.com/designs/new");
                                upload.Log_In();

                                for (int i = 0; i < mainVM.Shirts.Count; i++)
                                {
                                    mainVM.SelectedShirt = mainVM.Shirts[i];
                                    if (!upload.Upload(mainVM.Shirts[i]))
                                        failShirts.Add(mainVM.Shirts[i]);
                                }
                                UploadMerch.QuitDriver();
                                if (failShirts == null || failShirts.Count == 0)
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
                        }
                        else
                        {
                            Helper.ShowErrorMessageBox("User folder does not exist");
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

