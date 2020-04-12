using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upload.Definitions;

namespace Upload.Model
{
    public class VNeckTShirt : ShirtBase
    {
        public VNeckTShirt() : base()
        {
            this.TypeName = "VNeckTShirt";
            this.IsActive = false;
            this.MarketPlaces = new bool[] { false };
            this.Colors = new Color[]
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
            this.MarketPlaces = new bool[] { true };
            this.Prices = new double[] { 19.99 };
        }
    }
}
