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
        public static readonly string dictFile = "Dictionary.json";
        public static Dictionary<string, string> ReadDictionary()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            string appFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string dictPath = Path.Combine(appFolder, dictFile);
            if (File.Exists(dictPath))
            {
                string jsonDict = File.ReadAllText(dictPath);
                result = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonDict);
            }
            return result;
        }
        public static void SaveDictionary(Dictionary<string, string> inputDict)
        {
            string appFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string dictPath = Path.Combine(appFolder, dictFile);
            string jsonDict = JsonConvert.SerializeObject(inputDict);
            File.WriteAllText(dictPath, jsonDict);
        }
    }
}
