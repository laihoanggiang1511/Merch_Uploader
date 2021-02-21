using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzUpload.ViewModel
{
    public class PremiumTShirt : ShirtType
    {
        public PremiumTShirt()
        {
            this.TypeName = "PremiumTShirt";
            this.IsActive = false;
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
                new Color("Royal Blue",true),
                new Color("Silver",false),
                new Color("Slate",false),
                new Color("White",false),
            };
            this.FitTypes = new ObservableCollection<bool> { true, true, false };
            this.MarketPlaces = new ObservableCollection<bool> { true };
            this.Prices = new ObservableCollection<double> { 19.99 };
        }
        
    }
}
