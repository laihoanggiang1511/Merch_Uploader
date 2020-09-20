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
            string path = @"Upload\MerchUploadLog.txt";
            Common.Log.SetUpLog("MerchUploadLog.txt", path);
            string productData = "RDI1NTc1OTIzMzhCRUVERUFCQkIyMzNFRTIwQTFFODA=.TDwstW7iNSV7Glp0cfbefpY1A1vuVnalijoG2HVIlSiJLDv6D/5K7dBts+Sm+GRFF+PU3okSn6u+/uyDL1hveBrF45PXU2fzQp675VWqRaD9vzama8hnsHUEyZNru3RAxpx5SkAqIzlLmQSEPG33RHJZ80flEuN4Y+hmzIfK4prFHyL7EnLrBCLEajyHX1UvyhH/XHr/toRCiNr/0RrUS/XZILiFMoFF4pIHBMkJCD/Mc78kUsIV1ODkjBzZL8+Xw6wp7Gk6d730e+S7PPcwNXYggsAjwArNWa7qkGX5vVZ2pZdbUnp9H6JLHaPGuA38enDoIvBfdbKDAfU5qHA/O2NejWZoD5MOW1MK7TnjX3CY9jLRCnu74bxxWCTfRCeKskrdvKulpVPyAn68xVHqa4ycxj/jmvohZpC4lCPq9DabB9ZBR41T8+ckT+gucZLyP7/I2ZDLkEXIu0xFk2lIVXwqEW2dRrdsoTY7yfRiIYKN/ip0gj34WiK9N/VhbKomqSG382R3/VyyQ2VDspQujLZXDnYsuKDkCKCm38GU10mdoyV4mfZCPVpZRyQUWtgFVIotuNtY0R8QxbByFHvqhBHErAXxGVnK8axDRtqhD8/lyufypgFnPiclIqkqU4b12YxclixPOcMCuIsjCx9zQMkjTwEudl5RzwhMtBWeTnofPolV6hWCHNL6wThMdPmFq4IGfgXDgVz9HewsANa+E6OYGYpdflrho6tnVL6rJAY=";
            string productId = "d26ecd19-9ba5-4372-b509-7041becc28d1";
            CryptlexLicenseManager.SetLicenseData(productData, productId,1);
            Actions.MainActions actions = new Actions.MainActions();
            actions.ShowMainWindow();
        }
    }
}
