using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upload.Definitions;

namespace Upload.Model
{
    public class TankTop : ShirtBase
    {
        public TankTop() : base()
        {
            this.TypeName = "TankTop";
            this.IsActive = false;
            this.MarketPlaces = new bool[] { false };
            this.Colors = new Color[]
            {
                new Color("Black",true),
                new Color("Dark Heather",true),
                new Color("Heather Grey",true),
                new Color("Navy",true),
                new Color("Pink",false),
                new Color("Purple",true),
                new Color("Red",false),
                new Color("Royal Blue",true),
                new Color("Sapphire",false),
                new Color("White",false),
            };
            this.FitTypes = new bool[] { true, true };
            this.MarketPlaces = new bool[] { true ,false,false};
            this.Prices = new double[] { 19.99,16.69,17.99 };
        }
        public override object Clone()
        {
            TankTop target = new TankTop();
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
