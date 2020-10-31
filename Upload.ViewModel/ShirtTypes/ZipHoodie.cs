using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upload.ViewModel
{
    public class ZipHoodie : ShirtType
    {
        public ZipHoodie() : base()
        {
            this.TypeName = "ZipHoodie";
            this.IsActive = false;
            this.Colors = new ObservableCollection<Color>
            {
                new Color("Black",true),
                new Color("Dark Heather",true),
                new Color("Forest Green",true),
                new Color("Heather Grey",false),
                new Color("Navy",true),
                new Color("Purple",true),
                new Color("Red",false),
                new Color("Royal Blue",false),
            };
            this.FitTypes = new ObservableCollection<bool>();//ko co
            this.MarketPlaces = new ObservableCollection<bool> { true ,false,false};
            this.Prices = new ObservableCollection<double> { 33.99,28.99,32.99};
        }
       
    }
}
