using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upload.ViewModel
{
    public class SweetShirt:ShirtType
    {
        public SweetShirt() : base()
        {
            this.TypeName = "SweetShirt";
            this.IsActive = false;
            this.Colors = new ObservableCollection<Color>
            {
                new Color("Black", true),
                new Color("Dark Heather", true),
                new Color("Heather Grey", true),
                new Color("Navy", true),
                new Color("Blue", true),
            };
            this.FitTypes = new ObservableCollection<bool>(); //ko co
            this.MarketPlaces = new ObservableCollection<bool> { true,false,false };
            this.Prices = new ObservableCollection<double> { 31.99,31.99,33.99 };
        }
        
    }
}
