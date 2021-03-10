﻿using RestSharp;
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
using Cryptlex;
using Common.LicenseManager;
using Newtonsoft.Json;
using System.IO;
using System.Threading;
using System.Windows.Threading;

namespace Common.Update
{
    public class UpdateHelper
    {
        DownloadProgress _downloadWindow = null;
        bool _downloadResult = false;
        string _fileDir = string.Empty;
        string _filePath = string.Empty;
        public bool _completeCheckUpload = false;

        public UpdateHelper(string downloadDirectory)
        {
            _fileDir = downloadDirectory;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentVersion"></param>
        /// <returns>check for update done</returns>
        public void CheckForUpdate(string currentVersion)
        {
            LexActivator.CallbackType callbackType = new LexActivator.CallbackType(CheckForRelease);
            LexActivator.CheckForReleaseUpdate("windows", currentVersion, "stable", callbackType);
        }
        private void CheckForRelease(uint status)
        {
            if (status == LexStatusCodes.LA_RELEASE_UPDATE_AVAILABLE)
            {
                try
                {
                    UpdateModel updateModel = GetUpdateInfo();
                    if (updateModel != null)
                    {
                        string message = "An update is available, Do you want to download?\n\n";
                        message += "Change logs: \n";
                        message += updateModel.Notes;
                        if (MessageBox.Show(message, "Update available",
                            MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            UploadFile fileModel = updateModel.Files.FirstOrDefault(x => x.Extension.ToLower() == ".png" || x.Extension.ToLower() == ".exe");
                            if (fileModel != null)
                            {
                                _filePath = Path.Combine(_fileDir, fileModel.Name);
                                if (File.Exists(_filePath))
                                {
                                    string localMD5 = Utils.GetMD5File(_filePath);
                                    if (localMD5.ToUpper() == fileModel.Checksum.ToUpper())
                                    {
                                        DoItWhenDownloadComplete();
                                    }
                                    else
                                    {
                                        File.Delete(_filePath);
                                    }
                                }
                                Thread.CurrentThread.SetApartmentState(ApartmentState.STA);
                                DownloadUpdate(fileModel.URL);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
            _completeCheckUpload = true; 
        }

        public UpdateModel GetUpdateInfo()
        {
            UpdateModel updateModel = null;
            try
            {
                string url = string.Format("https://api.cryptlex.com/v3/releases/latest");
                var client = new RestClient(url);
                var request = new RestRequest(Method.GET);
                request.AddParameter("productId", LicenseManager.LicenseManager._productID);
                request.AddParameter("platform", "windows");
                string licenseKey = LexActivator.GetLicenseKey();
                request.AddParameter("key", licenseKey);
                client.Timeout = 5000;
                IRestResponse response = client.Execute(request);
                List<List<string>> resultList = new List<List<string>>();
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    updateModel = JsonConvert.DeserializeObject<UpdateModel>(response.Content);
                }
            }
            catch
            {
            }
            return updateModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath">temp file to store download</param>
        /// <returns></returns>
        public void DownloadUpdate(string url)
        {
            try
            {
                if (!string.IsNullOrEmpty(url) && !string.IsNullOrEmpty(_filePath))
                {
                    using (var client = new WebClient())
                    {
                        _downloadWindow = new DownloadProgress();
                        client.DownloadProgressChanged += Client_DownloadProgressChanged;
                        client.DownloadFileCompleted += Client_DownloadFileCompleted; ;
                        client.DownloadFileAsync(new System.Uri(url), _filePath);
                        _downloadWindow.ShowDialog();

                        while (!_downloadResult)
                        {
                            System.Windows.Forms.Application.DoEvents();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utils.ShowErrorMessageBox("Download fail: " + ex.Message);
                _completeCheckUpload = true;
            }
        }

        private void Client_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            DoItWhenDownloadComplete();
        }

        private void DoItWhenDownloadComplete()
        {
            _downloadResult = true;
            Process.Start(_filePath);
            Environment.Exit(0);
        }

        private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            _downloadWindow.Dispatcher.Invoke(delegate ()
            {
                var proggressBar = _downloadWindow.FindName("DownloadProgressBar") as ProgressBar;
                if (proggressBar != null)
                {
                    proggressBar.Value = e.ProgressPercentage;
                }
            });
        }
    }
}
