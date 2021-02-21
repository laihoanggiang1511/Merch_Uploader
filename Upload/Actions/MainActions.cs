using Common;
using Common.LicenseManager;
using Common.MVVMCore;
using Common.Update;
using Microsoft.Office.Interop.Excel;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using EzUpload.GUI;
using EzUpload.ViewModel;
using UploadTemplate;

namespace EzUpload.Actions
{
    public class MainActions
    {
        public void ShowMainWindow(string jsonShirt, string excelFile)
        {

        }
        public void ShowMainWindow()
        {
            try
            {
                MainWindow mainWindow = new MainWindow();
                MainViewModel mainVM = new MainViewModel
                {
                    CreateWindowCmd = new RelayCommand(CreateWindowCmdInvoke),
                    UploadWindowCmd = new RelayCommand(UploadCmdInvoke),
                    LicenseWindowCmd = new RelayCommand(LicenseWindowCmdInvoke),
                    HelpCmd = new RelayCommand(HelpCmdInvoke),

                };
                mainWindow.DataContext = mainVM;
                if (LicenseManager.IsLicenseOK() == true)
                {
                    mainVM.EnableCreate = true;
                    mainVM.EnableUpload = true;
                    //Allow Create
                    string metaData = LicenseManager.GetLicenseMetadata("create");
                    if (bool.TryParse(metaData, out bool enableCreate))
                    {
                        if (enableCreate)
                            mainVM.EnableCreate = true;
                        else
                            mainVM.EnableCreate = false;
                        //mainVM.EnableCreate = true;
                    }
                    else
                    {
                        mainVM.EnableCreate = true;
                    }
                    mainVM.LicenseStatus = string.Format("{0} day(s) left in your subscription", LicenseManager.GetDayLeft());
                }
                else
                {
                    mainVM.LicenseStatus = "License is not valid or expired!\n Please go to \"License\" to activate the product";
                }
#if DEBUG
                mainVM.EnableCreate = true;
                mainVM.EnableUpload = true;
#endif
                mainWindow.Show();
            }
            catch (Exception ex)
            {
                Environment.Exit(0);
            }
        }

        private void HelpCmdInvoke(object obj)
        {
            System.Diagnostics.Process.Start("https://www.youtube.com/playlist?list=PLiGK-5tw14EYTIb2o5-WUgaIHnhsBSuM1");
        }

        private void LicenseWindowCmdInvoke(object obj)
        {
            MainWindow mainWnd = obj as MainWindow;
            ActivationActions activateActions = new ActivationActions();
            activateActions.ShowMainWindow = new System.Action(ShowMainWindow);
            activateActions.ShowActivationForm();
            if (mainWnd != null)
            {
                mainWnd.Close();
            }
        }

        private void UploadCmdInvoke(object obj)
        {
            MainWindow mainWin = obj as MainWindow;
            (new UploadActions()).ShowWindow();
            if (mainWin != null)
                mainWin.Close();
        }

        private void CreateWindowCmdInvoke(object obj)
        {
            MainWindow mainWin = obj as MainWindow;
            //(new ExcelActions()).StartExcel();
            string folder = Path.GetDirectoryName(this.GetType().Assembly.Location);
            string excelFile = "UploadTemplate.xltx";
            excelFile = Path.Combine(folder, excelFile);
            Process.Start(excelFile);
            Environment.Exit(0);
        }
        //public static void CheckForUpdate()
        //{
        //    //Check for update from 2 server
        //    var localVersion = Assembly.GetExecutingAssembly().GetName().Version;

        //    string serverURLs = RegistryIO.GetValueAtKey(@"EzTools\Merch Uploader", "UpdateServerURL") as string;
        //    UpdateHelper updateHelper = null;
        //    if (string.IsNullOrEmpty(serverURLs))
        //    {
        //        serverURLs = "http://52.152.168.133/api/update";
        //    }
        //    string[] URLs = serverURLs.Split(',');
        //    foreach (string URL in URLs)
        //    {
        //        if (!string.IsNullOrEmpty(URL))
        //        {
        //            updateHelper = new UpdateHelper(URL, 1000, localVersion);
        //            if (updateHelper.ConnectUpdateServer() && updateHelper.UpdateModel != null)
        //            {
        //                RegistryIO.SaveValueToKey(@"EzTools\Merch Uploader", "UpdateServerURL", updateHelper.UpdateModel.NewUpdateServerURL);
        //                break;
        //            }
        //        }
        //    }
        //    if (updateHelper != null && updateHelper.IsThereNewUpdate())
        //    {
        //        if (MessageBox.Show("There is a new update. Do you want to download it?", "Update",
        //            MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
        //        {
        //            string path = Path.GetTempPath();
        //            path = Path.Combine(path, "Upload_Setup.msi");
        //            if (File.Exists(path))
        //            {
        //                File.Delete(path);
        //            }
        //            updateHelper.ExecuteUpdate(path);
        //        }
        //    }
        //}
    }
}
