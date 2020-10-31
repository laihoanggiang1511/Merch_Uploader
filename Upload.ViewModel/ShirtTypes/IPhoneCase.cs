using Common.MVVMCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upload.ViewModel
{
    public class IPhoneCase:ShirtType
    {
        public IPhoneCase()
        {
            this.TypeName = "IPhoneCase";
            this.IsActive = false;
            this.FitTypes = new ObservableCollection<bool>();
            this.Colors = new ObservableCollection<Color>();
            this.MarketPlaces = new ObservableCollection<bool> { true };
            this.Prices = new ObservableCollection<double> { 17.99 };
        }
        
    }
}
