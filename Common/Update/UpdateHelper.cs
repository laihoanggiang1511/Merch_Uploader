using RestSharp;
using RestSharp.Serialization.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Runtime;
using System.Net;
using System.Windows.Controls;
using System.Windows;
using System.Diagnostics;

namespace Common.Update
{
    public class UpdateHelper
    {
        public UpdateModel UpdateModel { get; set; }

        public Version LocalVersion { get; set; }

         string updateServerURL = "http://192.168.0.1/api/update";

        int ProductId { get; set; }

        public UpdateHelper(string uploadServerURL, int productId, Version localVersion)
        {
            updateServerURL = uploadServerURL;
            ProductId = productId;
            //Version version = null;
            //Version.TryParse(localVersion, out version);
            LocalVersion = localVersion;
        }
        public UpdateHelper(int productId, string localVersion)
        {
            ProductId = productId;
            Version version = null;
            Version.TryParse(localVersion, out version);
            LocalVersion = version;
        }

        public bool ConnectUpdateServer()
        {
            try
            {
                UpdateModel = null;
                string url = updateServerURL;
                if (ProductId > 0)
                {
                    url += $"/{ProductId}";
                }
                var client = new RestClient(url);
                client.Timeout = 3000;
                var request = new RestRequest(Method.GET);

                request.AddHeader("content-type", "application/json");
                //request.AddHeader("accept", "application/json");

                IRestResponse response = client.Execute(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    JsonDeserializer deserializer = new JsonDeserializer();
                    UpdateModel = deserializer.Deserialize<UpdateModel>(response);
                    return true;
                }
            }
            catch
            {
            }
            return false;
        }
        public bool IsThereNewUpdate()
        {
            if (UpdateModel != null && UpdateModel.ServerVersion != null)
            {
                Version serverVersion;
                if (Version.TryParse(UpdateModel.ServerVersion, out serverVersion))
                {
                    return serverVersion.CompareTo(LocalVersion) > 0;
                }
            }
            return false;
        }

        DownloadProgress downloadWindow = null;
        bool downloadResult = false;
        string filePath = string.Empty;



        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath">temp file to store download</param>
        /// <returns></returns>
        public bool ExecuteUpdate(string filePath)
        {
            try
            {
                if (UpdateModel != null && !string.IsNullOrEmpty(UpdateModel.UpdateURL))
                {
                    using (var client = new WebClient())
                    {
                        downloadWindow = new DownloadProgress();
                        this.filePath = filePath;
                        client.DownloadProgressChanged += Client_DownloadProgressChanged;
                        client.DownloadFileCompleted += Client_DownloadFileCompleted; ;
                        client.DownloadFileAsync(new System.Uri(UpdateModel.UpdateURL), filePath);
                        downloadWindow.Show();

                        while (!downloadResult)
                        {
                            System.Windows.Forms.Application.DoEvents();
                        }

                        return true;

                    }
                }
            }
            catch { }
            return false;
        }

        private void Client_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            downloadResult = true;
            Process.Start(filePath);
            if (downloadWindow != null)
                downloadWindow.Close();
            Environment.Exit(0);
        }

        private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            var proggressBar = downloadWindow.FindName("DownloadProgressBar") as ProgressBar;
            if (proggressBar != null)
            {
                proggressBar.Value = e.ProgressPercentage;
            }

        }
    }
}
