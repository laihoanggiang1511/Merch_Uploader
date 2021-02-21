using Cryptlex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using Common;

namespace Common.LicenseManager
{
    public class LicenseManager
    {
        private static string _registrySubKey;
        private static string _registryKeyName;
        public static string _productData;
        public static string _productID;

        public static void SetLicenseData(string registrySubKey, string registryKeyName, string productData, string productID, int permissionFlag = 1)
        {
            _registrySubKey = registrySubKey;
            _registryKeyName = registryKeyName;
            _productData = productData;
            _productID = productID;
            LexActivator.SetProductData(productData);
            if (permissionFlag == 1)
            {
                LexActivator.SetProductId(productID, LexActivator.PermissionFlags.LA_USER);
            }
            else if (permissionFlag == 0)
            {
                LexActivator.SetProductId(productID, LexActivator.PermissionFlags.LA_SYSTEM);
            }
            string licenseKey = LexActivator.GetLicenseKey();
            if (!string.IsNullOrEmpty(licenseKey))
            {
                LexActivator.SetLicenseKey(licenseKey);
            }
        }



        public static string ReadLicenseKeyFromRegistry()
        {
            return Crypt.Decrypt(RegistryIO.GetValueAtKey(_registrySubKey, _registryKeyName) as string, true);
        }
        public static void WriteLicenseKeyToRegistry(string licenseKey)
        {
            RegistryIO.SaveValueToKey(_registrySubKey, _registryKeyName, Crypt.Encrypt(licenseKey, true));
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
                    //Utils.ShowInfoMessageBox("Activation Successful!");
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
