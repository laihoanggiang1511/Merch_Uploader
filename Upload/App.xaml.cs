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
            string path = @"Merch Uploader\MerchUploadLog.txt";
            Common.Log.SetUpLog("MerchUploadLog.txt", path);
            string productData = "RERERkM0RjRDOEQwOTUyODVGMzc4OTE5QjM5MUI3M0Y=.t0KklKKyhwa5tUij0vbSNtNQ6xayIYRRB79wGOlZkeffscybfJi4+d9SnUQ3YVhZxohwgGz90pNeRQYX/6xu4P0WcfBgMYUpr/v8izz+zmZsmCdKhCjvN35Wo4FKWm1J4eXOlbc0PJhmID2SIf8llfHDDRImb8Rr2R/RBXYSaOoIGt4eddK/CZKKzirYG1ZrnThADks2syE/BVSM7J5WBWIkHxqqGay3gyb5q9/n/Ugwe1FGCVHBB+i1eaigCn62i3U3GbJekOzmdq/f6MfQqr83ka7FivPEcHxcCt0511153635oKjeIV49x+zWtss34a+jkRCyVCiwKgvCRrGgoNTE1P2P71sgltHTVIqfa8vjEd9xQOu5kqX7n8S5UJeXSh8neRasnby9q6w4fzAl4cYspnzvlm+8OxGlnSp+kBPdDhO1NylZwiHqYsyPnkoOZn/isZkJGEltDM6JNwta0xGGQ2CYcEczfU16NGWrBUFTILwdWbgq35mUhoEhqrnj4NAJLlfF7h0jkV1B+DM/a0zX1vgcRa5MSA2ZbqOtYoTfTS4Wiwe1YtvRF235G/9wpFtg1ZBsp691DSLkGmKTQYkahPAHmAneeYNUxrecR9MGYF/JMeBwGnuXUnQnweFE2b/vQvre/2UHCL36KZgZatcMtLy6NkTTXmvqO0iLNDp16ispKzZA+1aY2cwzjuJk3eIu2TkCOmLQyVp1Xp/jhxIMfg7ZCzw/HtvT+wgem7M=";
            string productId = "b1ca06a5-d046-46bb-a6ab-9347495cac1d";
            CryptlexLicenseManager.SetLicenseData(productData, productId,1);
            Actions.MainActions actions = new Actions.MainActions();
            actions.ShowMainWindow();
        }
    }
}
