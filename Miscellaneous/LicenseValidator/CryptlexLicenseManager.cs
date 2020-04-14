using Cryptlex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Miscellaneous;
using System.IO;

namespace Miscellaneous.LicenseValidator
{
    public class CryptlexLicenseManager
    {
        public static int GetDayLeft()
        {
            uint expiryDate = LexActivator.GetLicenseExpiryDate();
            int daysLeft = (int)(expiryDate - unixTimestamp()) / 86400;
            return daysLeft;
        }
        public static bool ActivateKey(string licenseKey)
        {
            try
            {
                LexActivator.SetLicenseKey(licenseKey);
                //LexActivator.SetActivationMetadata("key1", "value1");
                int status = LexActivator.ActivateLicense();
                if (status == LexStatusCodes.LA_OK || status == LexStatusCodes.LA_EXPIRED || status == LexStatusCodes.LA_SUSPENDED)
                {
                    Utils.ShowInfoMessageBox("Activation Successful :" + status.ToString());
                    return true;
                }
                else
                {
                    Utils.ShowInfoMessageBox("Error activating the license: " + status.ToString());
                    return false;
                }
            }
            catch (LexActivatorException ex)
            {
                Utils.ShowErrorMessageBox("Error code: " + ex.Code.ToString() + " Error message: " + ex.Message);
                return false;
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
                    string message = "Error activating the trial: " + status.ToString();
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

        public static bool DeactiveKey(string licenseKey)
        {
            throw new NotImplementedException();
        }

        public static LicenseModel GetKey(string licenseKey)
        {
            throw new NotImplementedException();
        }
        private static uint unixTimestamp()
        {
            return (uint)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }
    }
}
