using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
using EzUpload.DataAccess.Model;

namespace EzUpload.DataAccess.Helper
{
    public class DataHelper
    {
        public static string GetXElementValue(XDocument xdoc, string XPath)
        {
            XElement xElement = xdoc.XPathSelectElement(XPath);
            if (xElement != null)
            {
                return xElement.Value;
            }
            else return string.Empty;
        }
        public static string ArrayToString(Array input)
        {
            string result = string.Empty;
            if (input != null)
            {
                foreach (var v in input)
                {
                    result += (v.ToString() + ",");
                }
                if (result.Length > 0)
                {
                    result = result.Remove(result.Length - 1, 1);
                }
            }
            return result;
        }

        public static List<bool> StringToBoolArray(string input)
        {
            try
            {
                string[] temp = input.Split(',');
                bool[] result = new bool[temp.Length];
                for (int i = 0; i < temp.Length; i++)
                {
                    bool.TryParse(temp[i], out result[i]);
                }
                return result.ToList();
            }
            catch
            {
                return null;
            }
        }
        public static bool StringToColorArray(string input, List<ColorData> colors)
        {
            //List<Color> lstColor = new List<Color>();
            try
            {
                if (colors != null)
                {
                    string[] temp = input.Split(',');
                    //bool[] parseResult = new bool[temp.Length];
                    for (int i = 0; i < colors.Count; i++)
                    {
                        bool.TryParse(temp[i], out bool result);
                        colors[i].IsActive = result;
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static int[] StringToIntArray(string input)
        {
            try
            {
                string[] temp = input.Split(',');
                int[] result = new int[temp.Length];
                for (int i = 0; i < temp.Length; i++)
                {
                    int.TryParse(temp[i], out result[i]);
                }
                return result;
            }
            catch
            {
                return null;
            }
        }
        public static List<double> StringToDoubleArray(string input)
        {
            try
            {
                string[] temp = input.Split(',');
                double[] result = new double[temp.Length];
                for (int i = 0; i < temp.Length; i++)
                {
                    double.TryParse(temp[i], out result[i]);
                }
                return result.ToList();
            }
            catch
            {
                return null;
            }
        }
        public static int ConvertToInt(Object input)
        {
            int nVal = 0;
            if (input != null)
                Int32.TryParse(input.ToString(), out nVal);
            return nVal;
        }
        public static string ConvertTypeToString(object input)
        {
            string str= input.GetType().ToString();
            string[] splittedString = str.Split('.');
            return splittedString[splittedString.Length - 1];
        }
        public static double ConvertToDouble(Object input)
        {

            double dVal = 0;
            if (input != null)
                double.TryParse(input.ToString(), out dVal);
            return dVal;
        }

        public static bool ConvertToBool(string input)
        {
            bool dVal = false;
            if (string.IsNullOrEmpty(input))
                return false;
            if (input == "1")
                return true;

            bool.TryParse(input, out dVal);
            return dVal;
        }
    }

}
