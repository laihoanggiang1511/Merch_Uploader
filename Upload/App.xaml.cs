using Common;
using Common.LicenseManager;
using Common.Update;
using System;
using System.IO;
using System.IO.Pipes;
using System.Reflection;
using System.Windows;
using Upload.Actions;

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

            //Check for update
            var localVersion = Assembly.GetExecutingAssembly().GetName().Version;

            UpdateHelper update = new UpdateHelper(1000, localVersion);
            
            update.ConnectUpdateServer();
            if (update.IsThereNewUpdate())
            {
                if(MessageBox.Show("There is a new update. Do you want to download it?", "Update",
                    MessageBoxButton.YesNo,MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    string path = Path.GetTempPath();
                    path = Path.Combine(path, "Upload_Setup.msi");

                    update.ExecuteUpdate(path);
                }
            }


            //Setup log
            string logPath = @"Upload\MerchUploadLog.txt";
            Common.Log.SetUpLog("MerchUploadLog.txt", logPath);

            //Setup License
            string productData = "RDI1NTc1OTIzMzhCRUVERUFCQkIyMzNFRTIwQTFFODA=.TDwstW7iNSV7Glp0cfbefpY1A1vuVnalijoG2HVIlSiJLDv6D/5K7dBts+Sm+GRFF+PU3okSn6u+/uyDL1hveBrF45PXU2fzQp675VWqRaD9vzama8hnsHUEyZNru3RAxpx5SkAqIzlLmQSEPG33RHJZ80flEuN4Y+hmzIfK4prFHyL7EnLrBCLEajyHX1UvyhH/XHr/toRCiNr/0RrUS/XZILiFMoFF4pIHBMkJCD/Mc78kUsIV1ODkjBzZL8+Xw6wp7Gk6d730e+S7PPcwNXYggsAjwArNWa7qkGX5vVZ2pZdbUnp9H6JLHaPGuA38enDoIvBfdbKDAfU5qHA/O2NejWZoD5MOW1MK7TnjX3CY9jLRCnu74bxxWCTfRCeKskrdvKulpVPyAn68xVHqa4ycxj/jmvohZpC4lCPq9DabB9ZBR41T8+ckT+gucZLyP7/I2ZDLkEXIu0xFk2lIVXwqEW2dRrdsoTY7yfRiIYKN/ip0gj34WiK9N/VhbKomqSG382R3/VyyQ2VDspQujLZXDnYsuKDkCKCm38GU10mdoyV4mfZCPVpZRyQUWtgFVIotuNtY0R8QxbByFHvqhBHErAXxGVnK8axDRtqhD8/lyufypgFnPiclIqkqU4b12YxclixPOcMCuIsjCx9zQMkjTwEudl5RzwhMtBWeTnofPolV6hWCHNL6wThMdPmFq4IGfgXDgVz9HewsANa+E6OYGYpdflrho6tnVL6rJAY=";
            string productId = "d26ecd19-9ba5-4372-b509-7041becc28d1";
            CryptlexLicenseManager.SetLicenseData(productData, productId, 1);


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
                        clientPipe.Connect();
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
