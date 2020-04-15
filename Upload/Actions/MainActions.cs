using Miscellaneous;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upload.GUI;
using Upload.ViewModel;
using Upload.ViewModel.MVVMCore;
using Cryptlex;
using RelayCommand = Upload.ViewModel.MVVMCore.RelayCommand;
using System.Windows;
using System.Threading;
using System.Drawing;
using System.Windows.Media;
using Miscellaneous.LicenseValidator;

namespace Upload.Actions
{
    public class MainActions
    {
        //const int PRODUCT_ID = 6059;
        //KeyInfoResult keyInfo;
        public void ShowMainWindow()
        {
            string licenseKey = RegistryIO.GetKey();
            if (!string.IsNullOrEmpty(licenseKey))
            {
                LexActivator.SetLicenseKey(licenseKey);
            }
            if (LexActivator.IsLicenseGenuine() == LexStatusCodes.LA_OK||
                LexActivator.IsTrialGenuine() == LexStatusCodes.LA_OK)
            {
                MainWindow mainWindow = new MainWindow();
                MainViewModel mainVM = new MainViewModel
                {
                    CreateWindowCmd = new RelayCommand(CreateWindowCmdInvoke),
                    UploadWindowCmd = new RelayCommand(UploadCmdInvoke),
                    LicenseWindowCmd = new RelayCommand(LicenseWindowCmdInvoke),
                };
                mainWindow.DataContext = mainVM;
                //if (keyInfo.LicenseKey.F3 == true ||
                //    keyInfo.LicenseKey.F2 == true)
                   mainVM.EnableCreate = true;
                //else
                //    mainVM.EnableCreate = false;
                //if (keyInfo.LicenseKey.F2 == true ||
                //    keyInfo.LicenseKey.F3 == true)
                    mainVM.EnableUpload = true;
                //else
                //    mainVM.EnableUpload = false;

                mainVM.LicenseStatus = string.Format("{0} day(s) left in your subscription", CryptlexLicenseManager.GetDayLeft());
                mainWindow.Show();
            }
            else
            {
                MessageBoxResult msgBoxResult = MessageBox.Show("Do you want to open license form now?", "License validation fail", MessageBoxButton.YesNo, MessageBoxImage.Error);
                if (msgBoxResult == MessageBoxResult.Yes || msgBoxResult == MessageBoxResult.OK)
                {
                    LicenseWindowCmdInvoke(null);
                }
                else
                {
                    Environment.Exit(0);
                }
            }

            //keyInfo = obj as KeyInfoResult;
            //string serialNumber = LicenseManager.LoadLicense();
            //string errorMessage = string.Empty;
            //if (keyInfo == null)
            //{
            //    keyInfo = LicenseManager.GetKey(PRODUCT_ID, serialNumber, ref errorMessage);
            //}
            //if (keyInfo != null)
            //    {
            //    MainWindow mainWindow = new MainWindow();
            //    MainViewModel mainVM = new MainViewModel
            //    {
            //        CreateWindowCmd = new RelayCommand(CreateWindowCmdInvoke),
            //        UploadWindowCmd = new RelayCommand(UploadCmdInvoke),
            //        LicenseWindowCmd = new RelayCommand(LicenseWindowCmdInvoke),
            //    };
            //    mainWindow.DataContext = mainVM;
            //    if (keyInfo.LicenseKey.F3 == true||
            //        keyInfo.LicenseKey.F2 == true)
            //        mainVM.EnableCreate = true;
            //    else
            //        mainVM.EnableCreate = false;
            //    if (keyInfo.LicenseKey.F2 == true||
            //        keyInfo.LicenseKey.F3 == true)
            //        mainVM.EnableUpload = true;
            //    else
            //        mainVM.EnableUpload = false;

            //    mainVM.LicenseStatus = string.Format("{0} day(s) left in your subscription", keyInfo.LicenseKey.DaysLeft());
            //    mainWindow.Show();
            //}
            //else
            //{
            //    //MainWindow mainWindow = new MainWindow();
            //    //MainViewModel mainVM = new MainViewModel
            //    //{
            //    //    CreateWindowCmd = new RelayCommand(CreateWindowCmdInvoke),
            //    //    UploadWindowCmd = new RelayCommand(UploadCmdInvoke),
            //    //    LicenseWindowCmd = new RelayCommand(LicenseWindowCmdInvoke),
            //    //};
            //    //mainVM.EnableUpload = true;
            //    //mainVM.EnableCreate = true;

            //    //mainWindow.DataContext = mainVM;
            //    //mainWindow.Show();

            //    MessageBoxResult msgBoxResult = MessageBox.Show(errorMessage + "\nDo you want to open license form now?", "License validation fail", MessageBoxButton.YesNo, MessageBoxImage.Error);
            //    if (msgBoxResult == MessageBoxResult.Yes || msgBoxResult == MessageBoxResult.OK)
            //    {
            //        LicenseWindowCmdInvoke(null);
            //    }
            //    else
            //    {
            //        Environment.Exit(0);
            //    }
            //}
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
            activateActions.ShowMainWindow = new Action(ShowMainWindow);
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
            (new ShirtCreatorActions()).ShowShirtCreatorWindow();
            if (mainWin != null)
                mainWin.Close();
        }
    }
}
