using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Brush = System.Windows.Media.Brush;
using Common.MVVMCore;

namespace Miscellaneous
{
    public class ActivationFormViewModel : ViewModelBase
    {

        //private KeyInfoResult licenseKeyInfo;
        //public KeyInfoResult LicenseKeyInfo
        //{
        //    get { return licenseKeyInfo; }
        //    set
        //    {
        //        if (licenseKeyInfo != value)
        //        {
        //            licenseKeyInfo = value;
        //        }
        //        UpdateKeyInfo(licenseKeyInfo);
        //    }
        //}
        public RelayCommand ActivateCmd { get; set; }

        private string serialNumber = string.Empty;
        public string SerialNumber
        {
            get
            {
                return serialNumber;
            }
            set
            {
                if (serialNumber != value)
                {
                    serialNumber = value;
                    RaisePropertyChanged("SerialNumber");
                }
            }
        }
        private string expiryDate = string.Empty;
        public string ExpiryDate
        {
            get
            {
                return expiryDate;
            }
            set
            {
                if (expiryDate != value)
                {
                    expiryDate = value;
                    RaisePropertyChanged("SerialNumber");
                }
            }
        }
        private string status = string.Empty;
        public string Status
        {
            get
            {
                return status;
            }
            set
            {
                if (status != value)
                {
                    status = value;
                    RaisePropertyChanged("Status");
                }
            }
        }

        private int dayLeft = 0;
        public int DayLeft
        {
            get
            {
                return dayLeft;
            }
            set
            {
                if (dayLeft != value)
                {
                    dayLeft = value;
                    RaisePropertyChanged("DayLeft");
                }
            }
        }
        private Brush statusColor;
        public Brush StatusColor
        {
            get
            {
                return statusColor;
            }
            set
            {
                if (statusColor != value)
                {
                    statusColor = value;
                    RaisePropertyChanged("StatusColor");
                }
            }
        }

        private string machineCode = string.Empty;
        public string MachineCode
        {
            get
            {
                return machineCode;
            }
            set
            {
                if (machineCode != value)
                {
                    machineCode = value;
                    RaisePropertyChanged("MachineCode");
                }
            }
        }

        private string licenseInfo = string.Format("Date Created: {0}\n\nPeriod: {1}\n\nDayleft: {2}\n", "", "", "");
        public string LicenseInfo
        {
            get
            {
                return licenseInfo;
            }
            set
            {
                if (licenseInfo != value)
                {
                    licenseInfo = value;
                    RaisePropertyChanged("LicenseInfo");
                }
            }
        }

        public RelayCommand DeactivateCmd { get; set; }
        public RelayCommand BuyCmd { get; set; }
        public RelayCommand CreateTrialKeyCmd { get; set; }

        //public void UpdateKeyInfo(KeyInfoResult licenseKeyInfo)
        //{
        //    if (licenseKeyInfo != null)
        //    {
        //        LicenseInfo = string.Format("Date Created: {0}\n\nPeriod: {1}\n\nDayleft: {2}\n",
        //                                licenseKeyInfo.LicenseKey.Created.ToString("MMM/dd/yyyy"),
        //                                licenseKeyInfo.LicenseKey.Period.ToString(),
        //                                licenseKeyInfo.LicenseKey.DaysLeft().ToString());
        //        SerialNumber = licenseKeyInfo.LicenseKey.Key;
        //        Status = "Activated!";
        //        StatusColor = new SolidColorBrush(Colors.Green);
        //    }
        //    else
        //    {
        //        LicenseInfo = string.Format("Date Created: {0}\n\nPeriod: {1}\n\nDayleft: {2}\n", "", "", "");
        //        Status = "Not Activated!";
        //        StatusColor = new SolidColorBrush(Colors.Red);
        //        //SerialNumber = string.Empty;
        //    }
        //}
        //private string GetFeature(KeyInfoResult licenseKeyInfo)
        //{
        //    try
        //    {
        //        string result = string.Empty;
        //        Type myType = licenseKeyInfo.LicenseKey.GetType();
        //        for (int i = 1; i < 9; i++)
        //        {
        //            PropertyInfo props = myType.GetProperty("F" + i.ToString());
        //            object value = props.GetValue(licenseKeyInfo.LicenseKey);
        //            if (value.GetType() == typeof(bool))
        //            {
        //                if ((bool)value == true)
        //                {
        //                    result += $"Feature {i}, ";
        //                }
        //            }
        //        }
        //        return result;
        //    }
        //    catch
        //    {
        //        return string.Empty;
        //    }
        //}


        private Visibility purchaseFormEnable = Visibility.Collapsed;

        private Visibility activationFormEnable = Visibility.Visible;
        public Visibility ActivationFormEnable
        {
            get
            {
                return activationFormEnable;
            }
            set
            {
                if (activationFormEnable != value)
                {
                    activationFormEnable = value;
                    RaisePropertyChanged("ActivationFormEnable");
                }
            }
        }

        #region PurchaseForm
        public RelayCommand PurchaseCmd { get; set; }
        public RelayCommand BackToActivationCmd { get; set; }
        public ObservableCollection<PurchaseOption> PurchaseOptions { get; set; }

        private PurchaseOption selectedOption;
        public PurchaseOption SelectedOption
        {
            get
            {
                return selectedOption;
            }
            set
            {
                if(selectedOption != value)
                {
                    selectedOption = value;
                    RaisePropertyChanged("SelectedOption");
                }
            }
        }
        public Visibility PurchaseFormEnable
        {
            get
            {
                return purchaseFormEnable;
            }
            set
            {
                if (purchaseFormEnable != value)
                {
                    purchaseFormEnable = value;
                    RaisePropertyChanged("PurchaseFormEnable");
                }
            }
        }
        public ActivationFormViewModel()
        {
           // MachineCode = FingerPrint.Value();
            
        }
        #endregion
    }
}
