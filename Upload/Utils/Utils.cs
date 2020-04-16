using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;
using Upload.DataAccess;
using Upload.Model;
using MessageBox = System.Windows.MessageBox;

namespace Upload
{
    public class Utils
    {
        public static void ShowErrorMessageBox(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        public static void ShowInfoMessageBox(string message,string caption = null)
        {
            MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Error);
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
        public static List<Shirt> BrowseForShirts()
        {
            List<Shirt> result = new List<Shirt>();
            OpenFileDialog openFile = new OpenFileDialog
            {
                Filter = "Data File |*.JSON;*.xml| All Files |*.*",
                Multiselect = true
            };
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                for (int i = 0; i < openFile.FileNames.Length; i++)
                {
                    string filePath = openFile.FileNames[i];
                    Shirt s = new Shirt();
                    if (Path.GetExtension(filePath).ToLower() == ".json")
                    {
                        JsonDataAccess jsonData = new JsonDataAccess(openFile.FileNames[i]);
                        s = jsonData.GetShirt();
                    }
                    else if (Path.GetExtension(filePath).ToLower() == ".xml")
                    {
                        XMLDataAccess xmlData = new XMLDataAccess(filePath);
                        s = xmlData.GetShirt();
                    }
                    if (s != null)
                    {
                        result.Add(s);
                    }
                }
            }
            return result;
        }

        public static string[] BrowseForFilePath(string filter = "PNG file |*.PNG| All Files |*.*", bool multiselect = false)
        {
            OpenFileDialog openFile = new OpenFileDialog
            {
                Filter = filter,
                Multiselect = multiselect,
            };
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                return openFile.FileNames;
            }
            else return new string[] { string.Empty };
        }
    }
}

