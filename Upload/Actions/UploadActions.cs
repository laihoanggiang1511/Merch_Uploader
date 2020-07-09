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
using Upload.Model;
using Upload.GUI;
using Upload.ViewModel;
using Common.MVVMCore;
using System.Windows.Forms;
using ChromeAPI;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Upload.Definitions;
using System.Diagnostics;
using OpenQA.Selenium.Internal;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Controls.Primitives;

namespace Upload.Actions
{
    public class UploadActions
    {
        UploadWindowViewModel mainVM = null;

        public void ShowWindow()
        {
            UploadWindow uploadWindow = new UploadWindow();
            uploadWindow.Closed += this.OnExit;
            mainVM = new UploadWindowViewModel
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
                UserFolderPath = Properties.Settings.Default.UserFolderPath,
                Email = Common.Crypt.Decrypt(Properties.Settings.Default.Email, true),
            };
            mainVM.UserFolders = GetUserFolders();
            mainVM.UserFolderPath = mainVM.UserFolders.FirstOrDefault();
            if (uploadWindow != null)
            {
                PasswordBox passwordBox = uploadWindow.FindName("password") as PasswordBox;
                passwordBox.Password = Common.Crypt.Decrypt(Properties.Settings.Default.Password, true);
            }
            uploadWindow.DataContext = mainVM;
            uploadWindow.Show();
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
                //Save Setting
                Properties.Settings.Default.UserFolderPath = mainVM.UserFolderPath;
                Properties.Settings.Default.Email = Common.Crypt.Encrypt(mainVM.Email, true);
                if (sender is UploadWindow uploadWindow)
                {
                    PasswordBox passwordBox = uploadWindow.FindName("password") as PasswordBox;
                    if (passwordBox != null)
                    {
                        Properties.Settings.Default.Password = Common.Crypt.Encrypt(passwordBox.Password, true);
                    }
                }
                Properties.Settings.Default.Save();
                UploadMerch.QuitDriver();
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
            XMLDataAccess xMLDataAccess = new XMLDataAccess();
            foreach (Shirt s in mainVM.Shirts)
            {
                xMLDataAccess.SaveShirt(s);
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
                    if(!invalidChars.ToList().Any(x=>uploadVM.UserFolderPath.Contains(x)==true))
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
                //uploadVM.UserFolderPath
                //if (!string.IsNullOrEmpty(uploadVM.UserFolderPath) && uploadVM.UserFolderPath .
            }
        }
        private void OpenChromeCmdInvoke(object obj)
        {
            if (Directory.Exists(mainVM.UserFolderPath))
            {
                UploadMerch.OpenChrome(mainVM.UserFolderPath);
            }
            else
            {
                Helper.ShowErrorMessageBox("User folder does not exist");
            }
        }
        private void UploadCmdInvoke(object obj)
        {
            UploadWindow mainWindow = obj as UploadWindow;
            if (mainWindow != null)
            {
                System.Windows.Controls.PasswordBox passwordBox = mainWindow.FindName("password") as System.Windows.Controls.PasswordBox;
                UploadWindowViewModel mainVM = mainWindow.DataContext as UploadWindowViewModel;
                if (mainVM != null)
                {
                    mainVM.Password = passwordBox.Password;
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
                            message += pair.Key.DesignTitle + ":\n" + ShirtCreatorActions.GetErrorMessage(pair.Value) + "\n";
                        }
                        Utils.ShowErrorMessageBox(message);
                        return;
                    }
                    UploadMerch upload = new UploadMerch();
                    if (Directory.Exists(mainVM.UserFolderPath))
                    {
                        UploadMerch.OpenChrome(mainVM.UserFolderPath);
                        if (UploadMerch.driver != null)
                        {
                            try
                            {
                                UploadMerch.driver.Navigate().GoToUrl("https://merch.amazon.com/designs/new");
                                upload.Log_In(mainVM.Password);
                                for (int i = 0; i < mainVM.Shirts.Count; i++)
                                {
                                    mainVM.SelectedShirt = mainVM.Shirts[i];
                                    if (!upload.Upload(mainVM.Shirts[i]))
                                        failShirts.Add(mainVM.Shirts[i]);
                                }
                                if (failShirts == null || failShirts.Count == 0)
                                {
                                    System.Windows.MessageBox.Show("Job Done!");
                                }
                                else
                                {
                                    string message = "Fail to upload following shirt(s):\n";
                                    foreach (Shirt shirt in failShirts)
                                        message += shirt.DesignTitle + "\n";
                                    System.Windows.MessageBox.Show(message);
                                }
                            }
                            catch (Exception ex)
                            {
                                Utils.ShowErrorMessageBox(ex.Message);
                            }
                        }
                    }
                    else
                    {
                        Helper.ShowErrorMessageBox("User folder does not exist");
                    }
                }
            }
            mainVM.IsUploading = false;
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
    }
}

