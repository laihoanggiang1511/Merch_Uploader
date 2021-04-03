using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace EzUpload
{
   public class CaptchaResolver
   {
      string _googleKey;
      string _pageUrl;
      public CaptchaResolver(string siteKey, string pageURL)
      {
         _googleKey = siteKey;
         _pageUrl = pageURL;
      }
      public string SolveV2Captcha()
      {
         string id = SendV2CaptchaInput();
         string result = GetCaptchaResult(id);
         return result;
      }

      private string SendV2CaptchaInput()
      {
         string result = string.Empty;
         string url = string.Format("http://api.captcha.guru/in.php");
         var client = new RestClient(url);
         var request = new RestRequest(Method.GET);
         request.AddHeader("accept", "*/*");
         request.AddHeader("User-Agent", @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/88.0.4324.150 Safari/537.36");
         request.AddParameter("key", "89861b16082638032805e224be6f6410");
         request.AddParameter("method", "userrecaptcha");
         request.AddParameter("googlekey", _googleKey);
         request.AddParameter("pageurl", _pageUrl);
         client.Timeout = 5000;
         IRestResponse response = client.Execute(request);
         if (response.StatusCode == System.Net.HttpStatusCode.OK)
         {
            if (response.Content.Contains("OK"))
            {
               result = response.Content.TrimStart("OK|".ToCharArray());
            }
         }
         return result;
      }

      private string GetCaptchaResult(string id)
      {
         Thread.Sleep(10000);
         string result = string.Empty;
         string url = string.Format("http://api.captcha.guru/res.php");
         var client = new RestClient(url);
         var request = new RestRequest(Method.GET);
         request.AddHeader("accept", "*/*");
         request.AddHeader("User-Agent", @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/88.0.4324.150 Safari/537.36");
         request.AddParameter("key", "89861b16082638032805e224be6f6410");
         request.AddParameter("action", "get");
         request.AddParameter("id", id);
         client.Timeout = 5000;
         IRestResponse response = client.Execute(request);
         if (response.StatusCode == System.Net.HttpStatusCode.OK)
         {
            if (response.Content.Contains("CAPCHA_NOT_READY"))
            {
               result = GetCaptchaResult(id);
            }
            if (response.Content.Contains("OK"))
            {
               result = response.Content.TrimStart("OK|".ToCharArray());
            }
         }
         return result;
      }
   }
}
