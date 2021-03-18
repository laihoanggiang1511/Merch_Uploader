using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serialization.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace UploadTemplate
{
    public class TagGenerator
    {
        public static string GenerateTag(string seedingKeyword)
        {
            string result = string.Empty;
            string url = "https://bubblesear.ch/generator";
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            //request.AddHeader("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
            request.AddHeader("accept", "*/*");
            request.AddHeader("User-Agent", @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/88.0.4324.150 Safari/537.36");
            request.AddParameter("query", seedingKeyword);
            client.Timeout = 5000;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            IRestResponse response = client.Execute(request);

            string resultList = string.Empty;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                 result = Between(response.Content, "<textarea type=\"text\" id=\"output_tags\" readonly>", $@"</textarea>");
            }
            return result;
        }
        private static string Between(string source, string left, string right)
        {
            string temp = Regex.Match(source, string.Format(@"{0}([\s\S]*?){1}", left, right)).Value;
            if (temp.Length == 0)
                return temp;
            else
                return temp.Substring(left.Length, temp.Length - left.Length - right.Length);
        }
    }
}
