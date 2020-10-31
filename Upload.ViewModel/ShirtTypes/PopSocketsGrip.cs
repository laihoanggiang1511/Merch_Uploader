using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upload.ViewModel
{
    public class PopSocketsGrip:ShirtType
    {
        public PopSocketsGrip():base()
        {
            this.TypeName = "PopSocketsGrip";
            this.IsActive = false;
            this.MarketPlaces = new ObservableCollection<bool>() { true, false, false };
            this.FitTypes = new ObservableCollection<bool>();
            this.Colors = new ObservableCollection<Color>();
            this.Prices = new ObservableCollection<double>() { 14.99, 11.99, 12.99 };
        }                          
       
    }
}
