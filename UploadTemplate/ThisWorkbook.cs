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
            string subkey = $@"{GlobalVariables.PRODUCT_MANUFACTURER}\{GlobalVariables.PRODUCT_NAME}";
            LicenseManager.SetLicenseData(subkey, "LicenseKey", GlobalVariables.PRODUCT_DATA, GlobalVariables.PRODUCT_ID, 1);
            bool allowOpen = false;
            if (LicenseManager.IsLicenseOK() == true)
            {
                //Allow Create
                string metaData = LicenseManager.GetLicenseMetadata("create");
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
            //Load dictionary
            DictionaryActions.ReloadDictionary();
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
