using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upload.Excel
{
    public class TranslateAPI
    {
        public static string Translete(string english, string language)
        {
            string url = string.Format("https://microsoft-translator-text.p.rapidapi.com/translate?profanityAction=NoAction&textType=plain&to={0}&api-version=3.0", language);
            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            request.AddHeader("x-rapidapi-host", "microsoft-translator-text.p.rapidapi.com");
            request.AddHeader("x-rapidapi-key", "1db425c959msh518eb885697b2c9p13d169jsncb68aac4db37");
            request.AddHeader("content-type", "application/json");
            request.AddHeader("accept", "application/json");

            string textRequest = "[ {  \"Text\": \"" + english + "\" }]";
            request.AddParameter("application/json", textRequest, ParameterType.RequestBody);

            //request.AddParameter("application/json", "[ {  \"Text\": \"I would really like to drive your car around the block a few times.\" }]", ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string content = response.Content;

                string[] values = content.Split(new string[] { "\"text\":" }, StringSplitOptions.RemoveEmptyEntries);

                if (values.Length == 2)
                {
                    values = values[1].Split(new string[] { "\"" }, StringSplitOptions.RemoveEmptyEntries);

                    if (values.Length > 0)
                    {
                        return values[0];
                    }
                }
            }

            return english;
        }

        public static string LanguageIndexToCode(int idx)
        {
            switch (idx)
            {
                case 0:
                    return "en";//English
                case 1:
                    return "zh";//Chinese_Simplified
                case 2:
                    return "cs";//Czech
                case 3:
                    return "fr";//French
                case 4:
                    return "de";//German
                case 5:
                    return "it";//Italian
                case 6:
                    return "ja";//Japanese
                case 7:
                    return "ko";//Korean
                case 8:
                    return "pt";//Portuguese
                case 9:
                    return "ru";//Russian
                case 10:
                    return "es";//Spanish
                case 11:
                    return "th";//Thai
                default:
                    return "en";//English
            }
        }
        public static int LanguageCodeToIndex(string codePage)
        {
            int languageIndex = 0;
            if (codePage == "zh")
            {
                languageIndex = 1;
            }
            else if (codePage == "cs")
            {
                languageIndex = 2;
            }
            else if (codePage == "fr")
            {
                languageIndex = 3;
            }
            else if (codePage == "de")
            {
                languageIndex = 4;
            }
            else if (codePage == "it")
            {
                languageIndex = 5;
            }
            else if (codePage == "ja")
            {
                languageIndex = 6;
            }
            else if (codePage == "ko")
            {
                languageIndex = 7;
            }
            else if (codePage == "pt")
            {
                languageIndex = 8;
            }
            else if (codePage == "ru")
            {
                languageIndex = 9;
            }
            else if (codePage == "es")
            {
                languageIndex = 10;
            }
            else if (codePage == "th")
            {
                languageIndex = 11;
            }
            return languageIndex;
        }

        public static List<string> Transaletes(List<string> englishs, string language)
        {
            string sepString = "<#>";
            List<string> results = new List<string>();
            string english = string.Empty;
            foreach (var value in englishs)
            {
                if (string.IsNullOrEmpty(english) == false)
                {
                    english += sepString;
                }
                english += value.Replace("\\n", "\n");
            }
            string url = string.Format("https://microsoft-translator-text.p.rapidapi.com/translate?profanityAction=NoAction&textType=plain&to={0}&api-version=3.0", language);
            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            request.AddHeader("x-rapidapi-host", "microsoft-translator-text.p.rapidapi.com");
            request.AddHeader("x-rapidapi-key", "1db425c959msh518eb885697b2c9p13d169jsncb68aac4db37");
            request.AddHeader("content-type", "application/json");
            request.AddHeader("accept", "application/json");

            string textRequest = "[ {  \"Text\": \"" + english + "\" }]";
            request.AddParameter("application/json", textRequest, ParameterType.RequestBody);

            //request.AddParameter("application/json", "[ {  \"Text\": \"I would really like to drive your car around the block a few times.\" }]", ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string content = response.Content;
                string split1 = "[{\"text\":\"";
                string split2 = "\",\"to\":";
                string[] values = content.Split(new string[] { split1 }, StringSplitOptions.RemoveEmptyEntries);

                if (values.Length == 2)
                {
                    content = values[1];
                    values = content.Split(new string[] { split2 }, StringSplitOptions.RemoveEmptyEntries);
                    if (values.Length == 2)
                    {
                        content = values[0];
                        if (content.Length > 0)
                        {
                            values = content.Split(new string[] { sepString }, StringSplitOptions.RemoveEmptyEntries);
                            for (int i = 0; i < values.Length; i++)
                            {
                                results.Add(values[i]);
                            }
                        }
                    }
                }
            }
            return results;
        }
    }
}
