using Common;
using Common.LicenseManager;
using Common.Update;
using System;
using System.IO;
using System.IO.Pipes;
using System.Reflection;
using System.Threading;
using System.Windows;
using EzUpload.Actions;

namespace EzUpload
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ////Check for update
            //Thread thread = new Thread(x => MainActions.CheckForUpdate());
            //thread.Start();

            //Setup log
            string logPath = Constants.PRODUCT_NAME + "\\" + @"EzUploadLog.txt";
            Common.Log.SetUpLog("EzUploadLog.txt", logPath);

            //Setup License
            string subkey = $@"{Constants.PRODUCT_MANUFACTURER}\{Constants.PRODUCT_NAME}";
            LicenseManager.SetLicenseData(subkey, "LicenseKey", Constants.PRODUCT_DATA, Constants.PRODUCT_ID, 1);

            //Check for update
            string updateFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            updateFolder += $"\\{Constants.PRODUCT_NAME}\\Updates";
            if (!Directory.Exists(updateFolder))
            {
                Directory.CreateDirectory(updateFolder);
            }
            UpdateHelper updateHelper = new UpdateHelper(updateFolder);
            string localVersion = RegistryIO.GetValueAtKey(subkey, "Version") as string;
            if (string.IsNullOrEmpty(localVersion))
            {
                localVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
            updateHelper.CheckForUpdate(localVersion);
            while(!updateHelper._completeCheckUpload)
            {
            }
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;

            //Run
            if (e.Args != null && e.Args.Length > 0)
            {
                new ShirtCreatorActions().ShowWindow(e.Args[0]);
            }
            else
            {
                Actions.MainActions actions = new Actions.MainActions();
                actions.ShowMainWindow();
            }
            
        }
        protected override void OnExit(ExitEventArgs e)
        {
            try
            {
                //Tell Excel App that program is closed
                using (var clientPipe = new NamedPipeClientStream("MerchUploaderPipe"))
                {
                    if (clientPipe.IsConnected == false)
                    {
                        clientPipe.Connect(5000);
                        using (StreamWriter sWriter = new StreamWriter(clientPipe))
                        {
                            sWriter.Write(string.Empty);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.log.Fatal(ex);
            }
            base.OnExit(e);
        }
    }
}
