using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Cryptlex;
using System.Windows.Media;
using Common.MVVMCore;
using Common.LicenseManager;

namespace Common.LicenseManager
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
            activationFormVM.SerialNumber = LicenseManager.ReadLicenseKeyFromRegistry();

            ActivationForm activationFrm = new ActivationForm();
            activationFrm.DataContext = activationFormVM;
            //activationFrm.Closed += CloseActivationForm;
            UpdateLicenseInfo(activationFormVM);
            activationFrm.Show();
        }

        private void CreateTrialKeyCmdInvoke(object obj)
        {
            if (obj is ActivationFormViewModel activateVM)
            {
                if (LicenseManager.CreateTrialKey())
                {
                    UpdateLicenseInfo(activateVM);
                    //activateVM.ActivateCmd.Execute(activateVM);
                }
            }
        }
        private void UpdateLicenseInfo(ActivationFormViewModel activeVM)
        {
            if (LexActivator.IsLicenseGenuine() == LexStatusCodes.LA_OK)
            {
                activeVM.Status = "Activated!";
                activeVM.StatusColor = new SolidColorBrush(Colors.Green);
                activeVM.DayLeft = LicenseManager.GetDayLeft();
                DateTime expiryDate = LicenseManager.GetExpiryDate();
                activeVM.ExpiryDate = string.Format("{0: MMMM/dd/yyyy}", expiryDate);
            }
            else if (LexActivator.IsTrialGenuine() == LexStatusCodes.LA_OK)
            {
                activeVM.Status = "Trial Activated!";
                activeVM.SerialNumber = string.Empty;
                activeVM.StatusColor = new SolidColorBrush(Colors.Green);
                activeVM.DayLeft = LicenseManager.GetDayLeft();
                DateTime expiryDate = LicenseManager.GetExpiryDate();
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
            //System.Diagnostics.Process.Start("https://www.facebook.com/profile.php?id=100048979920714");
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
        private void DeactivateCmdInvoke(object obj)
        {
        }

        public void ActivateCmdInvoke(object obj)
        {
            try
            {
                ActivationFormViewModel activeVM = obj as ActivationFormViewModel;
                if (activeVM != null)
                {
                    LexActivator.SetLicenseKey(activeVM.SerialNumber);
                    if (LexActivator.IsLicenseGenuine() != LexStatusCodes.LA_OK)
                    {
                        if (!LicenseManager.ActivateKey(activeVM.SerialNumber))
                        {
                            return;
                        }
                    }
                    Utils.ShowInfoMessageBox("Activated!");

                    LicenseManager.WriteLicenseKeyToRegistry(activeVM.SerialNumber);
                    UpdateLicenseInfo(activeVM);
                }
            }
            catch (LexActivatorException ex)
            {
                Utils.ShowErrorMessageBox(ex.Message);
            }
        }
    }
}
