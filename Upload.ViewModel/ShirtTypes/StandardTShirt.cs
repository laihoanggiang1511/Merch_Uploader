using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upload.ViewModel
{
    public class StandardTShirt:ShirtType
    {
        public StandardTShirt()
        {
            this.TypeName = "StandardTShirt";
            this.IsActive = true;
            this.Colors = new ObservableCollection<Color>
            {
                new Color("Asphalt",true),
                new Color("Baby Blue",false),
                new Color("Black",true),
                new Color("Brown",true),
                new Color("Cranberry",true),
                new Color("Dark Heather",true),
                new Color("Grass",false),
                new Color("Heather Blue",true),
                new Color("Heather Grey",true),
                new Color("Kelly Green",false),
                new Color("Lemon",false),
                new Color("Navy",true),
                new Color("Olive",false),
                new Color("Orange",false),
                new Color("Pink",false),
                new Color("Purple",true),
                new Color("Red",false),
                new Color("Royal",true),
                new Color("Silver",false),
                new Color("Slate",false),
                new Color("White",false),
            };
            this.FitTypes = new ObservableCollection<bool> { true, true, false };
            this.MarketPlaces = new ObservableCollection<bool> { true, false, false };
            this.Prices = new ObservableCollection<double> { 19.99, 17.49,18.49 };

        }
       
    }
}
