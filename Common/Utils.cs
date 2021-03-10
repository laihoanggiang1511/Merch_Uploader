using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;
using MessageBox = System.Windows.MessageBox;

namespace Common
{
    public class Utils
    {
        public static string GetMD5File(string filePath)
        {
            Byte[] allBytes = File.ReadAllBytes(filePath);
            System.Security.Cryptography.HashAlgorithm md5Algo = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] hash = md5Algo.ComputeHash(allBytes);
            string output = BytesToHex(hash);
            return output;
        }
        private static string BytesToHex(byte[] bytes)
        {
            // write each byte as two char hex output.
            return String.Concat(Array.ConvertAll(bytes, x => x.ToString("X2")));
        }
        public static void ShowErrorMessageBox(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        public static void ShowInfoMessageBox(string message, string caption = null)
        {
            MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Information);
        }
        public static bool ShowWarningMessageBox(string message, string caption = null)
        {
            MessageBoxResult result = MessageBox.Show(message, caption, MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.OK)
            {
                return true;
            }
            else
                return false;
        }

    }
}

