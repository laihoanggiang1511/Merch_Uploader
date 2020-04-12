using Miscellaneous.Utils;
using SKM.V3;
using SKM.V3.Methods;
using SKM.V3.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Miscellaneous
{
    public class LicenseManager
    {
        // public int PRODUCT_ID { get; set; }
        private const string TOKEN = "WyIxNzY5MCIsImJ1a2pQQnkySWFPUXVMbGczTUJQYW43NmRrb2RnWlZIOW1tWUFEd1giXQ==";
        private const string TOKEN_TRIALKEY = "WyIxNzc2NSIsIkcvS08waE85VDVoeTZ3SWhDNVRCb0tpbzROajBZbnBucnRiNnhKQ0ciXQ==";
        private const string RSAPUBKEY = "<RSAKeyValue><Modulus>4w9gNFEZZ4W4yMNEQ1V3NXJiC0Qz7swWOE/C6kniCEbLEKuN+ELOdROJgvycRjXUQFMz1kO/6h/f7aMfH8mUXTcoghwCFZgB7rLSBcHw9cRST3IiIzJYcdATdT9SEDNybrZNutSPfTBWziJtDxkn/TBTMbJ63zYEb5qMsRtQa+n9gAeG4I3g0FR0XxcWmMSUpEixD8leQtu8ude4AxOS4eAoNQXPrMv1wlDxkp6d6XNM4/YUCeYGgaofWba2H9ZJmxk9XNhLuj5if8hK93Bjx+ETxMLqhdujAIEkw53JtbTWqzaxIowZEhLxrn+u3W4ONSuLlyolzXParVVH1NqkLQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

        //public KeyInfoResult KeyInfo { get; set; }
        //public LicenseManager(int productId = 5961, KeyInfoResult keyInfo = null)
        //{
        //    //this.PRODUCT_ID = productId;
        //    this.KeyInfo = keyInfo;
        //}
        public static bool DeactivateKey(int PRODUCT_ID, string licenseKey)
        {
            if (!string.IsNullOrEmpty(licenseKey))
            {
                DeactivateModel deactiveModel = new DeactivateModel
                {
                    ProductId = PRODUCT_ID,
                    Key = licenseKey,
                    MachineCode = FingerPrint.Value(),
                };
                BasicResult result = Key.Deactivate(TOKEN, deactiveModel);

                if (result == null || result.Result == ResultType.Error)
                {
                    MessageBox.Show(result.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return false;
        }
        public static KeyInfoResult ActivateKey(int PRODUCT_ID, string licenseKey)
        {
            if (!string.IsNullOrEmpty(licenseKey))
            {
                string RSAPubKey = RSAPUBKEY;
                string auth = TOKEN;
                var result = Key.Activate(auth, new SKM.V3.Models.ActivateModel()
                {
                    Key = licenseKey.TrimStart(char.Parse(" ")).TrimEnd(char.Parse(" ")),
                    ProductId = PRODUCT_ID,
                    Sign = true,
                    MachineCode = FingerPrint.Value(),
                });
                if (result == null)
                {
                    MessageBox.Show("License key activation fail!", "License key activation fail!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;

                }
                else if (result.Result == ResultType.Error ||
                    !result.LicenseKey.HasValidSignature(RSAPubKey).IsValid())
                {
                    MessageBox.Show(result.Message, "License key activation fail!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
                else
                {
                    return result;
                }
            }
            else return null;
        }
        public static KeyInfoResult GetKey(int PRODUCT_ID, string serialNumber, ref string errorMessage)
        {
            errorMessage = string.Empty;
            if (string.IsNullOrEmpty(serialNumber))
            {
                errorMessage = "Product is not activated!";
            }
            else
            {
                string RSAPubKey = RSAPUBKEY;
                string auth = TOKEN;
                KeyInfoModel keyInfoModel = new KeyInfoModel()
                {
                    Key = serialNumber,
                    ProductId = PRODUCT_ID,
                    Sign = true,
                };

                KeyInfoResult result = Key.GetKey(auth, keyInfoModel);
                if (result == null || result.Result == ResultType.Error ||
                    !result.LicenseKey.HasValidSignature(RSAPubKey).IsValid() ||
                    !result.LicenseKey.IsValid())
                {
                    errorMessage = "License key is invalid!";
                }
                else if (!result.LicenseKey.IsOnRightMachine(FingerPrint.Value()).IsValid())
                {
                    errorMessage = "License is not activated on this machine!";
                }
                else if (!result.LicenseKey.HasNotExpired().IsValid())
                {
                    errorMessage = "This license is expired!";
                }
                else if (!result.LicenseKey.IsNotBlocked().IsValid())
                {
                    errorMessage = "This license is blocked!";
                }
                else
                {
                    return result;
                }
            }
            return null;
        }
        public static bool SaveLicense(string serialNumber)
        {
            //try
            //{
            //    string fullPath = System.Reflection.Assembly.GetAssembly(typeof(LicenseManager)).Location;
            //    string theDirectory = Path.GetDirectoryName(fullPath);

            //    string licenseFilePath = Path.Combine(theDirectory, "license.lic");

            //    if (!File.Exists(licenseFilePath))
            //    {
            //        File.Create(licenseFilePath).Close();
            //    }
            //    File.WriteAllText(licenseFilePath, Crypt.Encrypt(serialNumber, true));
            //}
            //catch
            //{
            //}
            return RegistryIO.SaveKey(serialNumber);
        }
        public static string LoadLicense()
        {
            return RegistryIO.GetKey();
            //try
            //{
            //    string fullPath = System.Reflection.Assembly.GetAssembly(typeof(LicenseValidator)).Location;
            //    string theDirectory = Path.GetDirectoryName(fullPath);
            //    string licenseFilePath = Path.Combine(theDirectory, "license.lic");

            //    if (!File.Exists(licenseFilePath))
            //    {
            //        return string.Empty;
            //    }
            //    string text = File.ReadAllText(licenseFilePath);
            //    return Crypt.Decrypt(text, true);
            //}
            //catch
            //{
            //    return string.Empty;
            //}
        }

        public static string CreateTrialKey(int ProductId)
        {
            try
            {
                CreateTrialKeyModel trialKeyModel = new CreateTrialKeyModel
                {
                    ProductId = ProductId,
                    MachineCode = FingerPrint.Value(),

                };
                CreateKeyResult newTrialKey = Key.CreateTrialKey(TOKEN_TRIALKEY, trialKeyModel);
                if (newTrialKey.Result == ResultType.Success)
                {
                    return newTrialKey.Key;
                }
                else
                {
                    Utils.Utils.ShowErrorMessageBox(newTrialKey.Message);
                    return string.Empty;
                }
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}

