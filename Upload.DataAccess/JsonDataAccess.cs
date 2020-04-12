using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Upload.Model;

namespace Upload.DataAccess
{
    public class JsonDataAccess : IDataAccess
    {
        private string data = string.Empty;
        private string jsonFilePath = string.Empty;
        public JsonDataAccess(string jsonFilePath)
        {
            data = System.IO.File.ReadAllText(jsonFilePath);
            this.jsonFilePath = jsonFilePath;
        }

        public JsonDataAccess()
        {
        }

        public bool SaveShirt(Shirt s)
        {
            throw new NotImplementedException();
        }

        public bool DeleteShirt()
        {
            throw new NotImplementedException();
        }

        public Shirt GetShirt()
        {
            try
            {
                Shirt s = new Shirt();
                string temp = Between(data, "STANDARD_TSHIRT-prices-US: ", ",");
                if (!double.TryParse(Regex.Replace(temp, " ", ""), out double price))
                {
                    price = 19.99;
                }
                s.StandardTShirt.Prices[0] = price;

                //PNG File Name
                temp = Between(data, "\"FRONT\": \"", "\"");
                s.FrontStdPath = Path.GetDirectoryName(jsonFilePath) + "\\" + temp;

                //Fit Types
                temp = Between(data, "\"STANDARD_TSHIRT\": {", "]");
                s.StandardTShirt.FitTypes[0] = temp.Contains("\"men\"");
                s.StandardTShirt.FitTypes[1] = temp.Contains("\"women\"");
                s.StandardTShirt.FitTypes[2] = temp.Contains("\"youth\"");

                //Price
                string strPrice_US = Between(data, "\"prices-US\":", ",");
                if (double.TryParse(strPrice_US, out double price_US))
                    s.StandardTShirt.Prices[0] = price_US;

                //BrandName
                s.BrandName = Between(data, "\"brandName\": \"", "\"");
                s.DesignTitle = Between(data, "\"designTitle\": \"", "\"");
                s.FeatureBullet1 = Between(data, "\"featureBullet1\": \"", "\"");
                s.FeatureBullet2 = Between(data, "\"featureBullet2\": \"", "\"");
                s.Description = Between(data, "\"description\": \"", "\"");
                return s;
            }
            catch
            {
                return null;
            }
        }

        public bool UpdateShirt(Shirt s)
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

   
    }
}
