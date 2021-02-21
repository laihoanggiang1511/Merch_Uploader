﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.MVVMCore;

namespace EzUpload.ViewModel
{
    public class MainViewModel: ViewModelBase
    {
        public RelayCommand CreateWindowCmd { get; set; }
        public RelayCommand UploadWindowCmd { get; set; }
        public RelayCommand LicenseWindowCmd { get; set; }

        public RelayCommand HelpCmd { get; set; }
        private bool enableCreate = false;
        public bool EnableCreate
        {
            get { return enableCreate; }
            set
            {
                if (enableCreate != value)
                {
                    enableCreate = value;
                    RaisePropertyChanged("EnableCreate");
                }
            }
        }
        private string licenseStatus = string.Empty;
        public string LicenseStatus
        {
            get { return licenseStatus; }
            set
            {
                if (licenseStatus != value)
                {
                    licenseStatus = value;
                    RaisePropertyChanged("LicenseStatus");
                }
            }
        }
        private bool enableUpload = false;
        public bool EnableUpload
        {
            get { return enableUpload; }
            set
            {
                if (enableUpload != value)
                {
                    enableUpload = value;
                    RaisePropertyChanged("EnableUpload");
                }
            }
        }
    }
}
