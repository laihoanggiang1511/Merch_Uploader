using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upload.Definitions;

namespace Upload.Model
{
    public class StandardTShirt:ShirtBase
    {
        public StandardTShirt()
        {
            this.TypeName = "StandardTShirt";
            this.IsActive = true;
            this.Colors = new Color[]
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
            this.FitTypes = new bool[] { true, true, false };
            this.MarketPlaces = new bool[] { true, false, false };
            this.Prices = new double[] { 19.99, 17.49,18.49 };
        }
    }
}
