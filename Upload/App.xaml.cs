using Common.LicenseManager;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Upload.GUI;

namespace Upload
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            string path = @"Upload/MerchUploadLog.txt";
            Common.Log.SetUpLog("MerchUploadLog.txt", path);
            CryptlexLicenseManager.SetLicenseData();
            Actions.MainActions actions = new Actions.MainActions();
            actions.ShowMainWindow();
        }
    }
}
