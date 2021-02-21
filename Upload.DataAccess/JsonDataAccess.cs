using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EzUpload.DataAccess.Model;

namespace EzUpload.DataAccess
{
    public class JsonDataAccess : IDataAccess
    {
        //private string data = string.Empty;
        //private string jsonFilePath = string.Empty;
        //public JsonDataAccess(string jsonFilePath)
        //{
        //    data = System.IO.File.ReadAllText(jsonFilePath);
        //    this.jsonFilePath = jsonFilePath;
        //}

        //public JsonDataAccess()
        //{
        //}

        public bool SaveShirt(ShirtData s)
        {
            try
            {
                string path = GetJsonPathFromImage(s.ImagePath);
                s.ImagePath = Path.GetFileName(s.ImagePath);
                string json = JsonConvert.SerializeObject(s,Formatting.Indented);
                if (!string.IsNullOrEmpty(path) && !string.IsNullOrEmpty(json))
                {
                    File.WriteAllText(path, json);
                    return true;
                }
            }
            catch 
            { 
            }
            return false;
        }

        public bool DeleteShirt()
        {
            throw new NotImplementedException();
        }

        public ShirtData ReadShirt(string filePath)
        {
            try
            {
                string json = File.ReadAllText(filePath);
                ShirtData result = JsonConvert.DeserializeObject<ShirtData>(json);
                result.ImagePath = Path.Combine(Path.GetDirectoryName(filePath), result.ImagePath);
                return result;
               
            }
            catch
            {
            }
            return null;
        }

        public bool UpdateShirt(ShirtData s)
        {
            throw new NotImplementedException();
        }

        private string Between(string source, string left, string right)
        {
            string temp = Regex.Match(source, string.Format(@"{0}([\s\S]*?){1}", left, right)).Value;
            if (temp.Length == 0)
                return temp;
            else
                return temp.Substring(left.Length, temp.Length - left.Length - 1);
        }
        private string GetJsonPathFromImage(string PNGPath)
        {
            string xmlFilePath = Path.GetDirectoryName(PNGPath) + "\\"
                                + Path.GetFileNameWithoutExtension(PNGPath) +
                                ".json";
            return xmlFilePath;
        }


    }
}
