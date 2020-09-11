using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upload.ViewModel
{
    public class LongSleeveTShirt:ShirtType
    {
        public LongSleeveTShirt():base()
        {
            this.TypeName = "LongSleeveTShirt";
            this.IsActive = false;
            this.MarketPlaces = new ObservableCollection<bool> { true, false, false };
            this.FitTypes = null; //Ko co
            this.Colors = new ObservableCollection<Color>
            {
                new Color("Black", true),
                new Color("Dark Heather", true),
                new Color("Heather Grey", true),
                new Color("Navy", true),
                new Color("Blue", true),
            };
            this.Prices = new ObservableCollection<double> { 22.99, 21.99, 22.99 };
        }
       

    }
}
