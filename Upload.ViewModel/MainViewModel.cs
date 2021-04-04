using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.MVVMCore;

namespace EzUpload.ViewModel
{
   public class MainViewModel : ViewModelBase
   {
      public RelayCommand CreateWindowCmd { get; set; }
      public RelayCommand UploadWindowCmd { get; set; }
      public RelayCommand LicenseWindowCmd { get; set; }
      public RelayCommand RunTeePublicUploadCmd { get; set; }

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
      private bool _merchUploadEnable = false;
      public bool MerchUploadEnable
      {
         get { return _merchUploadEnable; }
         set
         {
            if (_merchUploadEnable != value)
            {
               _merchUploadEnable = value;
               RaisePropertyChanged("MerchUploadEnable");
            }
         }
      }
      private bool _teePublicUploadEnable = false;
      public bool TeePublicUploadEnable
      {
         get { return _teePublicUploadEnable; }
         set
         {
            if (_teePublicUploadEnable != value)
            {
               _teePublicUploadEnable = value;
               RaisePropertyChanged("TeePublicUploadEnable");
            }
         }
      }
   }
}
