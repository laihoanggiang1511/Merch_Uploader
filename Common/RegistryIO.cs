using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class RegistryIO
    {
        public static object GetValueAtKey(string subkey, string name)
        {
            try
            {
                string strSubkey = string.Format(@"Software\{0}", subkey);
                RegistryKey rKey = Registry.CurrentUser.OpenSubKey(strSubkey);
                if (rKey == null)
                {
                    rKey = Registry.LocalMachine.OpenSubKey(strSubkey);
                }
                if (rKey != null)
                {
                    return rKey.GetValue(name);
                }
                return null;
            }
            catch (Exception ex)
            {
                Log.log.Fatal(ex);
                return null;
            }

        }

        public static void SaveValueToKey(string subkey, string name, object Value, bool localMachine = false)
        {
            try
            {
                RegistryKey keyLocation = localMachine ? Registry.LocalMachine : Registry.CurrentUser;
                string strSubkey = string.Format(@"Software\{0}", subkey);      
                RegistryKey rKey = keyLocation.OpenSubKey(strSubkey,true);
                if (rKey != null)
                {
                    rKey.SetValue(name, Value);
                    rKey.Close();
                }
                else
                {
                    rKey = keyLocation.CreateSubKey(strSubkey);
                    rKey.SetValue(name, Value);
                }
            }
            catch (Exception ex)
            {
                Log.log.Fatal(ex);
            }
        }
    }
}
