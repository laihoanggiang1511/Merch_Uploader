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
using Upload.ViewModel.MVVMCore;
using System.Windows.Forms;
using ChromeAPI;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Upload.Definitions;
using System.Diagnostics;

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
                UserFolderPath = Properties.Settings.Default.UserFolderPath,
                Email = Miscellaneous.Crypt.Decrypt(Properties.Settings.Default.Email, true),
            };
            if (uploadWindow != null)
            {
                PasswordBox passwordBox = uploadWindow.FindName("password") as PasswordBox;
                passwordBox.Password = Miscellaneous.Crypt.Decrypt(Properties.Settings.Default.Password, true);
            }
            uploadWindow.DataContext = mainVM;
            uploadWindow.Show();
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
                Properties.Settings.Default.Email = Miscellaneous.Crypt.Encrypt(mainVM.Email, true);
                if (sender is UploadWindow uploadWindow)
                {
                    PasswordBox passwordBox = uploadWindow.FindName("password") as PasswordBox;
                    if (passwordBox != null)
                    {
                        Properties.Settings.Default.Password = Miscellaneous.Crypt.Encrypt(passwordBox.Password, true);
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
            FolderBrowserDialog openFile = new FolderBrowserDialog();
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                (obj as UploadWindowViewModel).UserFolderPath = openFile.SelectedPath;
            }
        }
        private void OpenChromeCmdInvoke(object obj)
        {
            UploadMerch.OpenChrome(mainVM.UserFolderPath);
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

