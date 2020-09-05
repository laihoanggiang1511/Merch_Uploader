using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upload.Definitions;

namespace Upload.Model
{
    public class ShirtBase : ICloneable
    {
        public bool IsActive { get; set; }
        public bool[] MarketPlaces { get; set; }
        public double[] Prices { get; set; }
        public Color[] Colors { get; set; }
        public bool[] FitTypes { get; set; }
        public string TypeName { get; set; }
        public ShirtBase()
        {
            this.IsActive = false;
        }

        public virtual object Clone()
        {
            dynamic target = new System.Dynamic.ExpandoObject();
            target.IsActive = IsActive;
            target.MarketPlaces = MarketPlaces.Clone() as bool[];
            target.Prices = Prices.Clone() as double[];
            if (this.Colors != null)
            {
                List<Color> colors = new List<Color>();
                this.Colors.ToList().ForEach(x=>colors.Add(x));
                target.Colors = colors.ToArray();
            }
            else
            {
                target.Colors = null;
            }
            target.FitTypes = FitTypes;
            target.TypeName = TypeName;

            return target;
        }
    }
}
