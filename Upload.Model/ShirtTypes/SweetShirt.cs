using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upload.Definitions;

namespace Upload.Model
{
    public class SweetShirt:ShirtBase
    {
        public SweetShirt() : base()
        {
            this.TypeName = "SweetShirt";
            this.IsActive = false;
            this.Colors = new Color[]
            {
                new Color("Black", true),
                new Color("Dark Heather", true),
                new Color("Heather Grey", true),
                new Color("Navy", true),
                new Color("Blue", true),
            };
            this.FitTypes = null; //ko co
            this.MarketPlaces = new bool[] { true,false,false };
            this.Prices = new double[] { 31.99,31.99,33.99 };
        }
    }
}
