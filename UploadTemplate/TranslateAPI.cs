using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UploadTemplate
{
    public class TranslateAPI
    {
        public static string Translate(string english, string language)
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
            client.Timeout = 5000;
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


    }
}
