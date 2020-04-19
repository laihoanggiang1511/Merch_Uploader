using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Cryptlex;
using Miscellaneous.LicenseValidator;
using System.Windows.Media;

namespace Miscellaneous
{
    public class ActivationActions
    {
        public Action ShowMainWindow;
        public void ShowActivationForm()
        {
            string errorMessage = string.Empty;
            ActivationFormViewModel activationFormVM = new ActivationFormViewModel
            {
                ActivateCmd = new RelayCommand(ActivateCmdInvoke),
                DeactivateCmd = new RelayCommand(DeactivateCmdInvoke),
                BackToActivationCmd = new RelayCommand(BackToActivationCmdInvoke),
                BuyCmd = new RelayCommand(PurchaseCmdInvoke),
                PurchaseCmd = new RelayCommand(PurchaseCmdInvoke),
                CreateTrialKeyCmd = new RelayCommand(CreateTrialKeyCmdInvoke),
            };
            activationFormVM.SerialNumber = LexActivator.GetLicenseKey();

            ActivationForm activationFrm = new ActivationForm();
            activationFrm.DataContext = activationFormVM;
            activationFrm.Closed += CloseActivationForm;
            UpdateLicenseInfo(activationFormVM);
            activationFrm.Show();
        }

        //public void ShowActivationForm()
        //{
        //    string errorMessage = string.Empty;
        //    ActivationFormViewModel activationFormVM = new ActivationFormViewModel
        //    {
        //        ActivateCmd = new RelayCommand(ActivateCmdInvoke),
        //        DeactivateCmd = new RelayCommand(DeactivateCmdInvoke),
        //        BackToActivationCmd = new RelayCommand(BackToActivationCmdInvoke),
        //        BuyCmd = new RelayCommand(BuyCmdInvoke),
        //        PurchaseCmd = new RelayCommand(PurchaseCmdInvoke),
        //        CreateTrialKeyCmd = new RelayCommand(CreateTrialKeyCmdInvoke),
        //        SerialNumber = LicenseManager.LoadLicense(),
        //    };

        //    ActivationForm activationFrm = new ActivationForm();
        //    activationFrm.DataContext = activationFormVM;
        //    activationFrm.Closed += CloseActivationForm;
        //    activationFrm.Show();
        //}

        private void CreateTrialKeyCmdInvoke(object obj)
        {
            if (obj is ActivationFormViewModel activateVM)
            {
                if (CryptlexLicenseManager.CreateTrialKey())
                {
                    UpdateLicenseInfo(activateVM);
                    //activateVM.ActivateCmd.Execute(activateVM);
                }
            }
        }
        private void UpdateLicenseInfo(ActivationFormViewModel activeVM)
        {
            activeVM.SerialNumber = LexActivator.GetLicenseKey();

            if (LexActivator.IsLicenseGenuine() == LexStatusCodes.LA_OK)
            {
                activeVM.Status = "Activated!";
                activeVM.StatusColor = new SolidColorBrush(Colors.Green);
                activeVM.DayLeft = CryptlexLicenseManager.GetDayLeft();
                DateTime expiryDate = CryptlexLicenseManager.GetExpiryDate();
                activeVM.ExpiryDate = string.Format("{0: MMMM/dd/yyyy}", expiryDate);
            }
            else if (LexActivator.IsTrialGenuine() == LexStatusCodes.LA_OK)
            {
                activeVM.Status = "Trial Activated!";
                activeVM.SerialNumber = string.Empty;
                activeVM.StatusColor = new SolidColorBrush(Colors.Green);
                activeVM.DayLeft = CryptlexLicenseManager.GetDayLeft();
                DateTime expiryDate = CryptlexLicenseManager.GetExpiryDate();
                activeVM.ExpiryDate = string.Format("{0: MMMM/dd/yyyy}", expiryDate);
            }
            else
            {
                activeVM.Status = "Not Activated!";
                activeVM.StatusColor = new SolidColorBrush(Colors.Crimson);
                activeVM.DayLeft = 0;
                activeVM.ExpiryDate = string.Empty;
            }

        }

        private void PurchaseCmdInvoke(object obj)
        {
            if (obj is string purchaseURL)
            {
                System.Diagnostics.Process.Start(purchaseURL);
            }
        }

        private void BackToActivationCmdInvoke(object obj)
        {
            if (obj is ActivationFormViewModel activateVM)
            {
                activateVM.ActivationFormEnable = Visibility.Visible;
                activateVM.PurchaseFormEnable = Visibility.Collapsed;
            }
        }

        private void BuyCmdInvoke(object obj)
        {
            if (obj is ActivationFormViewModel activateVM)
            {
                activateVM.ActivationFormEnable = Visibility.Collapsed;
                activateVM.PurchaseFormEnable = Visibility.Visible;
            }
        }

        public void CloseActivationForm(object sender, EventArgs e)
        {
            ShowMainWindow.Invoke();
        }
        private void DeactivateCmdInvoke(object obj)
        {
            //ActivationFormViewModel activeVM = obj as ActivationFormViewModel;
            //if (activeVM != null)
            //{
            //    string key = activeVM.SerialNumber;
            //    if (LicenseManager.DeactivateKey(PRODUCT_ID, key))
            //    {
            //        Utils.Utils.ShowInfoMessageBox($"Successfully deactive license key {key}", "Deactivation");
            //        //activeVM.SerialNumber = string.Empty;
            //        //KeyInfo = null;
            //        activeVM.LicenseKeyInfo = null;
            //    }
            //}
        }

        public void ActivateCmdInvoke(object obj)
        {
            ActivationFormViewModel activeVM = obj as ActivationFormViewModel;
            if (activeVM != null)
            {
                LexActivator.SetLicenseKey(activeVM.SerialNumber);
                if (LexActivator.IsLicenseGenuine() == LexStatusCodes.LA_OK)
                {
                    RegistryIO.SaveKey(activeVM.SerialNumber);
                    UpdateLicenseInfo(activeVM);
                }
                else if (CryptlexLicenseManager.ActivateKey(activeVM.SerialNumber))
                {
                    RegistryIO.SaveKey(activeVM.SerialNumber);
                    UpdateLicenseInfo(activeVM);
                }
            }
        }
    }
}
