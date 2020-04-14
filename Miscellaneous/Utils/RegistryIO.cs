using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miscellaneous
{
    public class RegistryIO
    {
        const string ROOT_FOLDER = @"SOFTWARE\Merch Uploader";
        const string KEY_NAME = "Serial";
        public static bool SaveKey(string licenseKey)
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey(ROOT_FOLDER);
                key.SetValue(KEY_NAME, Crypt.Encrypt(licenseKey, true));
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static string GetKey()
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey(ROOT_FOLDER);
                string value = key.GetValue(KEY_NAME).ToString();
                return Crypt.Decrypt(value,true);
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
