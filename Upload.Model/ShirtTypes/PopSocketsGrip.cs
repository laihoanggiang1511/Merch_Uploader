using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upload.Definitions;

namespace Upload.Model
{
    public class PopSocketsGrip:ShirtBase
    {
        public PopSocketsGrip():base()
        {
            this.TypeName = "PopSocketsGrip";
            this.IsActive = false;
            this.MarketPlaces = new bool[] { true, false, false };
            this.FitTypes = null;
            this.Colors = null;
            this.Prices = new double[] { 14.99, 11.99, 12.99 };
        }
        public override object Clone()
        {
            PopSocketsGrip target = new PopSocketsGrip();
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
