using Common.LicenseManager;
using Common.MVVMCore;
using Microsoft.Office.Interop.Excel;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using Upload.GUI;
using Upload.ViewModel;
using UploadTemplate;

namespace Upload.Actions
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
                if (CryptlexLicenseManager.IsLicenseOK() == true)
                {
                    mainVM.EnableCreate = true;
                    mainVM.EnableUpload = true;
                    //Allow Create
                    string metaData = CryptlexLicenseManager.GetLicenseMetadata("create");
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
                    mainVM.LicenseStatus = string.Format("{0} day(s) left in your subscription", CryptlexLicenseManager.GetDayLeft());
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

            #region Purchase Options
            //List<PurchaseOption> purchaseOptions = new List<PurchaseOption>();
            //PurchaseOption option1 = new PurchaseOption("https://app.cryptolens.io/Form/P/ZDk6rOc3/477")
            //{                                            
            //    Header2 = "1 Month",
            //    Feature1 = " \n ",
            //    Feature2 = "~$15/device/mo",
            //    Price = 15,
            //    NumberOfDevices = 1,
            //    TextColor = new SolidColorBrush(Colors.Black),
            //};
            //purchaseOptions.Add(option1);
            //PurchaseOption option2 = new PurchaseOption("https://app.cryptolens.io/Form/P/ZpmfgKoy/487")
            //{
            //    Header2 = "6 Months",
            //    Feature1 = "+1 month\n(total 7 months)",
            //    Feature2 = "~$6.5/device/mo",
            //    Price = 90,
            //    NumberOfDevices = 2,
            //    TextColor = new SolidColorBrush(Colors.Blue),
            //};
            //purchaseOptions.Add(option2);
            //PurchaseOption option3 = new PurchaseOption("https://app.cryptolens.io/Form/P/NWcQ0hfT/488")
            //{
            //    Header2 = "1 Year",
            //    Feature1 = "+2 months\n(total 14 months)",
            //    Feature2 = "~$5.5/device/mo",
            //    Price = 150,
            //    NumberOfDevices = 2,
            //    TextColor = new SolidColorBrush(Colors.Red),
            //};
            //purchaseOptions.Add(option3);
            #endregion

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

    }
}
