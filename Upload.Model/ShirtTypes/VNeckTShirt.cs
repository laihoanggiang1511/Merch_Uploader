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
            this.MarketPlaces = new bool[] { true ,false,false};
            this.Prices = new double[] { 19.99,15.99,16.99 };
        }
        public override object Clone()
        {
            VNeckTShirt target = new VNeckTShirt();
            target.IsActive = IsActive;
            target.MarketPlaces = MarketPlaces?.Clone() as bool[];
            target.Prices = Prices.Clone() as double[];
            if (this.Colors != null)
            {
                List<Color> colors = new List<Color>();
                this.Colors.ToList().ForEach(x => colors.Add(x));
                target.Colors = colors.ToArray();
            }
            else
            {
                target.Colors = null;
            }
            target.FitTypes = FitTypes?.Clone() as bool[];
            target.TypeName = TypeName;

            return target;
        }
    }
}
