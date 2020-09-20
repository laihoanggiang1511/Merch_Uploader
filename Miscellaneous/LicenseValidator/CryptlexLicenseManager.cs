using Cryptlex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Miscellaneous;
using System.IO;
using System.Reflection;

namespace Miscellaneous.LicenseValidator
{
    public class CryptlexLicenseManager
    {
        public static void SetLicenseData()
        {
            LexActivator.SetProductData("RERERkM0RjRDOEQwOTUyODVGMzc4OTE5QjM5MUI3M0Y=.t0KklKKyhwa5tUij0vbSNtNQ6xayIYRRB79wGOlZkeffscybfJi4+d9SnUQ3YVhZxohwgGz90pNeRQYX/6xu4P0WcfBgMYUpr/v8izz+zmZsmCdKhCjvN35Wo4FKWm1J4eXOlbc0PJhmID2SIf8llfHDDRImb8Rr2R/RBXYSaOoIGt4eddK/CZKKzirYG1ZrnThADks2syE/BVSM7J5WBWIkHxqqGay3gyb5q9/n/Ugwe1FGCVHBB+i1eaigCn62i3U3GbJekOzmdq/f6MfQqr83ka7FivPEcHxcCt0511153635oKjeIV49x+zWtss34a+jkRCyVCiwKgvCRrGgoNTE1P2P71sgltHTVIqfa8vjEd9xQOu5kqX7n8S5UJeXSh8neRasnby9q6w4fzAl4cYspnzvlm+8OxGlnSp+kBPdDhO1NylZwiHqYsyPnkoOZn/isZkJGEltDM6JNwta0xGGQ2CYcEczfU16NGWrBUFTILwdWbgq35mUhoEhqrnj4NAJLlfF7h0jkV1B+DM/a0zX1vgcRa5MSA2ZbqOtYoTfTS4Wiwe1YtvRF235G/9wpFtg1ZBsp691DSLkGmKTQYkahPAHmAneeYNUxrecR9MGYF/JMeBwGnuXUnQnweFE2b/vQvre/2UHCL36KZgZatcMtLy6NkTTXmvqO0iLNDp16ispKzZA+1aY2cwzjuJk3eIu2TkCOmLQyVp1Xp/jhxIMfg7ZCzw/HtvT+wgem7M=");
            LexActivator.SetProductId("b1ca06a5-d046-46bb-a6ab-9347495cac1d", LexActivator.PermissionFlags.LA_USER);
            string licenseKey = LexActivator.GetLicenseKey();
            if (!string.IsNullOrEmpty(licenseKey))
            {
                LexActivator.SetLicenseKey(licenseKey);
            }
        }
        public static bool IsLicenseOK()
        {
            if (LexActivator.IsLicenseGenuine() == LexStatusCodes.LA_OK ||
                LexActivator.IsTrialGenuine() == LexStatusCodes.LA_OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static int GetDayLeft()
        {
            uint expiryDate = LexActivator.GetLicenseExpiryDate();
            int daysLeft = (int)(expiryDate - unixTimestamp()) / 86400;
            if (daysLeft <= 0)
            {
                expiryDate = LexActivator.GetTrialExpiryDate();
                daysLeft = (int)(expiryDate - unixTimestamp()) / 86400;
            }
            if (daysLeft <= 0)
            {
                daysLeft = 0;
            }
            return daysLeft;
        }
        public static bool ActivateKey(string licenseKey)
        {
            try
            {
                LexActivator.SetLicenseKey(licenseKey);
                //LexActivator.SetActivationMetadata("key1", "value1");
                int status = LexActivator.ActivateLicense();
                if (status == LexStatusCodes.LA_OK)
                {
                    Utils.ShowInfoMessageBox("Activation Successful!");
                    return true;
                }
                else
                {
                    Utils.ShowInfoMessageBox("Error activating the license:\n" + GetErrorMessage(status));
                    return false;
                }
            }
            catch (LexActivatorException ex)
            {
                Utils.ShowErrorMessageBox("Error code: " + ex.Code.ToString() + "\nError message: " + ex.Message);
                return false;
            }
        }

        public static string GetErrorMessage(int errorCode)
        {
            try
            {
                var props = typeof(LexStatusCodes).GetFields(BindingFlags.Public | BindingFlags.Static);
                var wantedProp = props.FirstOrDefault(prop => (int)prop.GetValue(null) == errorCode);
                string message = wantedProp.Name.ToString().TrimStart(new char[] { 'L', 'A', '_' });

                return message;
            }
            catch
            {
                return string.Empty;
            }

        }

        public static bool CreateTrialKey()
        {
            try
            {
                //LexActivator.SetTrialActivationMetadata("key2", "value2");
                int status = LexActivator.ActivateTrial();
                if (status != LexStatusCodes.LA_OK)
                {
                    string message = "Error activating the trial: " + GetErrorMessage(status);
                    Utils.ShowInfoMessageBox(message);
                    return false;
                }
                else
                {
                    Utils.ShowInfoMessageBox("Trial started successful");
                    return true;
                }
            }
            catch (LexActivatorException ex)
            {
                Utils.ShowErrorMessageBox(ex.Message);
                return false;
            }
        }
        public static string GetLicenseMetadata(string key)
        {
            try
            {
                string value = LexActivator.GetLicenseMetadata(key);
                return value;
            }
            catch
            {
                return string.Empty;
            }
        }
        public static bool DeactiveKey(string licenseKey)
        {
            throw new NotImplementedException();
        }

        private static uint unixTimestamp()
        {
            return (uint)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }
        public static DateTime GetExpiryDate()
        {
            uint period = LexActivator.GetLicenseExpiryDate();
            if (period <= 0)
            {
                period = LexActivator.GetTrialExpiryDate();
            }
            TimeSpan span = TimeSpan.FromSeconds(period);
            DateTime startDate = new DateTime(1970, 1, 1);
            DateTime expiryDate = startDate + span;
            return expiryDate;
        }
    }
}
