using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miscellaneous.LicenseValidator
{
    public interface ILicenseManager
    {
        LicenseModel ActivateKey(string licenseKey);
        bool DeactiveKey(string licenseKey);
        LicenseModel GetKey(string licenseKey);
        LicenseModel CreateTrialKey();
    }
}
