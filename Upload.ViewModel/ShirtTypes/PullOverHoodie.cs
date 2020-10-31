using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upload.ViewModel
{
    public class PullOverHoodie : ShirtType
    {
        public PullOverHoodie() : base()
        {
            this.TypeName = "PullOverHoodie";
            this.IsActive = false;
            this.Colors = new ObservableCollection<Color>
            {
                new Color("Black", true),
                new Color("Dark Heather", true),
                new Color("Heather Grey", true),
                new Color("Navy", true),
                new Color("Royal Blue", true),
            };
            this.FitTypes = new ObservableCollection<bool>();
            this.MarketPlaces = new ObservableCollection<bool> { true, false, false };
            this.Prices = new ObservableCollection<double> { 31.99, 33.99, 36.99 };
        }
        
    }
}
