using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serialization.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace UploadTemplate
{
    public class CheckTrademarkAPI
    {
        public static List<List<string>> CheckTrademark(string wordToCheck)
        {
            string url = string.Format("http://tmhunt.com/ngrams.php");
            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
            request.AddHeader("accept", "*/*");
            request.AddHeader("User-Agent", @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/88.0.4324.150 Safari/537.36");
            string httpEncodedString = HttpUtility.UrlEncode(wordToCheck);
            request.AddParameter("application/x-www-form-urlencoded", $"query=%22{httpEncodedString}%22", ParameterType.RequestBody);
            client.Timeout = 5000;
            IRestResponse response = client.Execute(request);
            List<List<string>> resultList = new List<List<string>>();
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
             {
                resultList = JsonConvert.DeserializeObject<List<List<string>>>(response.Content);
                if (resultList == null)
                {
                    resultList = new List<List<string>>(); 
                }
            }
            return resultList;
        }
    }
}
