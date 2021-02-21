using Microsoft.Office.Tools.Ribbon;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UploadTemplate
{
    public class DictionaryActions
    {
        public static void ReloadDictionary()
        {
            GlobalVariables.replaceDict = ReadDictionary();
            Globals.Ribbons.Ribbon2.cbb_Dictionary.Items.Clear();
            foreach(var pair in GlobalVariables.replaceDict)
            {
                RibbonDropDownItem item = Globals.Factory.GetRibbonFactory().CreateRibbonDropDownItem();
                item.Label = pair.Key;
                Globals.Ribbons.Ribbon2.cbb_Dictionary.Items.Add(item);
            }
        }
        public static readonly string dictFile = $"{GlobalVariables.PRODUCT_NAME}\\Dictionary.json";
        public static Dictionary<string, string> ReadDictionary()
        {
            Dictionary<string, string> result = null;
            string appFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string dictPath = Path.Combine(appFolder, dictFile);
            if (File.Exists(dictPath))
            {
                string jsonDict = File.ReadAllText(dictPath);
                result = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonDict);
            }
            if(result == null)
            {
                result = new Dictionary<string, string>();
            }
            return result;
        }
        public static void SaveDictionary(Dictionary<string, string> inputDict)
        {
            string appFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string dictPath = Path.Combine(appFolder, dictFile);
            string jsonDict = JsonConvert.SerializeObject(inputDict,Formatting.Indented);
            File.WriteAllText(dictPath, jsonDict);
        }
    }
}
