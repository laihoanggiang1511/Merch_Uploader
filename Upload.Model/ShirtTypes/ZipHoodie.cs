using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upload.Definitions;

namespace Upload.Model
{
    public class ZipHoodie : ShirtBase
    {
        public ZipHoodie() : base()
        {
            this.TypeName = "ZipHoodie";
            this.IsActive = false;
            this.Colors = new Color[]
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
            this.FitTypes = null;//ko co
            this.MarketPlaces = new bool[] { true ,false,false};
            this.Prices = new double[] { 33.99,28.99,32.99};
        }
        public override object Clone()
        {
            ZipHoodie target = new ZipHoodie();
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
