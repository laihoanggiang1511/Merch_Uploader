using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using Common.LicenseManager;
using Microsoft.Office.Tools.Excel;
using Microsoft.VisualStudio.Tools.Applications.Runtime;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;

namespace UploadTemplate
{
    public partial class ThisWorkbook
    {
        private void ThisWorkbook_Startup(object sender, System.EventArgs e)
        {
#if !DEBUG
            //Setup License
            string productData = "RDI1NTc1OTIzMzhCRUVERUFCQkIyMzNFRTIwQTFFODA=.TDwstW7iNSV7Glp0cfbefpY1A1vuVnalijoG2HVIlSiJLDv6D/5K7dBts+Sm+GRFF+PU3okSn6u+/uyDL1hveBrF45PXU2fzQp675VWqRaD9vzama8hnsHUEyZNru3RAxpx5SkAqIzlLmQSEPG33RHJZ80flEuN4Y+hmzIfK4prFHyL7EnLrBCLEajyHX1UvyhH/XHr/toRCiNr/0RrUS/XZILiFMoFF4pIHBMkJCD/Mc78kUsIV1ODkjBzZL8+Xw6wp7Gk6d730e+S7PPcwNXYggsAjwArNWa7qkGX5vVZ2pZdbUnp9H6JLHaPGuA38enDoIvBfdbKDAfU5qHA/O2NejWZoD5MOW1MK7TnjX3CY9jLRCnu74bxxWCTfRCeKskrdvKulpVPyAn68xVHqa4ycxj/jmvohZpC4lCPq9DabB9ZBR41T8+ckT+gucZLyP7/I2ZDLkEXIu0xFk2lIVXwqEW2dRrdsoTY7yfRiIYKN/ip0gj34WiK9N/VhbKomqSG382R3/VyyQ2VDspQujLZXDnYsuKDkCKCm38GU10mdoyV4mfZCPVpZRyQUWtgFVIotuNtY0R8QxbByFHvqhBHErAXxGVnK8axDRtqhD8/lyufypgFnPiclIqkqU4b12YxclixPOcMCuIsjCx9zQMkjTwEudl5RzwhMtBWeTnofPolV6hWCHNL6wThMdPmFq4IGfgXDgVz9HewsANa+E6OYGYpdflrho6tnVL6rJAY=";
            string productId = "d26ecd19-9ba5-4372-b509-7041becc28d1";
            CryptlexLicenseManager.SetLicenseData(productData, productId, 1);
            bool allowOpen = false;
            if (CryptlexLicenseManager.IsLicenseOK() == true)
            {
                //Allow Create
                string metaData = CryptlexLicenseManager.GetLicenseMetadata("create");
                if (bool.TryParse(metaData, out bool enableCreate))
                {
                    if (enableCreate)
                        allowOpen = true;
                        
                }
                else
                    allowOpen = true;
            }
            if(!allowOpen)
            {
                Common.Utils.ShowInfoMessageBox("License is not valid or expired!\nPlease go to \"License\" to activate the product", "Activation Failed!");
                Environment.Exit(0);
            }
#endif
        }

        private void ThisWorkbook_Shutdown(object sender, System.EventArgs e)
        {
        }
        // 1: Copy the following code block into the ThisAddin, ThisWorkbook, or ThisDocument class.

        #region VSTO Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisWorkbook_Startup);
            this.Shutdown += new System.EventHandler(ThisWorkbook_Shutdown);
        }

        #endregion

    }
}
