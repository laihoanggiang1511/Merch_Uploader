using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upload.ViewModel
{
    public class VNeckTShirt : ShirtType
    {
        public VNeckTShirt() : base()
        {
            this.TypeName = "VNeckTShirt";
            this.IsActive = false;
            this.MarketPlaces = new ObservableCollection<bool> { false };
            this.Colors = new ObservableCollection<Color>
            {
                new Color("Baby Blue",true),
                new Color("Black",true),
                new Color("Dark Heather",true),
                new Color("Green",true),
                new Color("Heather Grey",false),
                new Color("Navy",true),
                new Color("Pink",false),
                new Color("Purple",true),
                new Color("Red",false),
                new Color("Sapphire",false),
            };
            this.FitTypes = null;//ko co
            this.MarketPlaces = new ObservableCollection<bool> { true ,false,false};
            this.Prices = new ObservableCollection<double> { 19.99,15.99,16.99 };
        }
        
    }
}
