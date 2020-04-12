using Miscellaneous.Utils;
using SKM.V3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Miscellaneous
{
    public class ActivationActions
    {
        readonly int PRODUCT_ID;
        public RelayCommand ShowMainWindow;

        public ActivationActions(int PRODUCT_ID)
        {
            this.PRODUCT_ID = PRODUCT_ID;
        }

        public void ShowActivationForm(KeyInfoResult KeyInfo, List<PurchaseOption> PurchaseOptions)
        {
            string errorMessage = string.Empty;
            ActivationFormViewModel activationFormVM = new ActivationFormViewModel
            {
                ActivateCmd = new RelayCommand(ActivateCmdInvoke),
                DeactivateCmd = new RelayCommand(DeactivateCmdInvoke),
                BackToActivationCmd = new RelayCommand(BackToActivationCmdInvoke),
                BuyCmd = new RelayCommand(BuyCmdInvoke),
                PurchaseCmd = new RelayCommand(PurchaseCmdInvoke),
                CreateTrialKeyCmd = new RelayCommand(CreateTrialKeyCmdInvoke),
                SerialNumber = LicenseManager.LoadLicense(),

            };

            activationFormVM.PurchaseOptions = new System.Collections.ObjectModel.ObservableCollection<PurchaseOption>();
            PurchaseOptions.ForEach(x => activationFormVM.PurchaseOptions.Add(x));
            activationFormVM.LicenseKeyInfo = KeyInfo;
            ActivationForm activationFrm = new ActivationForm();
            activationFrm.DataContext = activationFormVM;
            activationFrm.Closed += CloseActivationForm;
            activationFrm.Show();
        }
        private void CreateTrialKeyCmdInvoke(object obj)
        {
            if (obj is ActivationFormViewModel activateVM)
            {
                string trialkey = LicenseManager.CreateTrialKey(PRODUCT_ID);
                if (!string.IsNullOrEmpty(trialkey))
                {
                    activateVM.SerialNumber = trialkey;
                    activateVM.ActivateCmd.Execute(activateVM);
                }
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
            if (sender is ActivationForm activationForm)
            {
                if (activationForm.DataContext is ActivationFormViewModel activateVM)
                {
                    if (activateVM.LicenseKeyInfo != null)
                    {
                        ShowMainWindow.Execute(activateVM.LicenseInfo);
                    }
                    else
                    {
                        Environment.Exit(0);
                    }
                }
            }
        }
        private void DeactivateCmdInvoke(object obj)
        {
            ActivationFormViewModel activeVM = obj as ActivationFormViewModel;
            if (activeVM != null)
            {
                string key = activeVM.SerialNumber;
                if (LicenseManager.DeactivateKey(PRODUCT_ID, key))
                {
                    Utils.Utils.ShowInfoMessageBox($"Successfully deactive license key {key}", "Deactivation");
                    //activeVM.SerialNumber = string.Empty;
                    //KeyInfo = null;
                    activeVM.LicenseKeyInfo = null;
                }
            }
        }

        public void ActivateCmdInvoke(object obj)
        {
            ActivationFormViewModel activeVM = obj as ActivationFormViewModel;
            if (activeVM != null)
            {
                KeyInfoResult licenseKeyInfo = LicenseManager.ActivateKey(PRODUCT_ID, activeVM.SerialNumber);
                if (licenseKeyInfo != null)
                {
                    LicenseManager.SaveLicense(licenseKeyInfo.LicenseKey.Key);
                    activeVM.LicenseKeyInfo = licenseKeyInfo;
                    //KeyInfo = licenseKeyInfo;
                }
            }
        }
    }
}
