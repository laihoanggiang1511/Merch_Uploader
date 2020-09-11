using Common.MVVMCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upload.ViewModel
{
    public class SamsungCase : ShirtType
    {
        public SamsungCase()
        {
            this.TypeName = "SamsungCase";
            this.IsActive = false;
            this.FitTypes = null;
            this.Colors = null;
            this.MarketPlaces = new ObservableCollection<bool> { true };
            this.Prices = new ObservableCollection<double> { 17.99 };
        }

    }
}
