using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upload.ViewModel.MVVMCore;

namespace Upload.ViewModel
{
    public class MainViewModel: MVVMCore.ViewModelBase
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
                    RaisePropertyChanged("LiscenseStatus");
                }
            }
        }
        private bool enableUpload = true;
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
