﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upload.Definitions;

namespace Upload.Model
{
    public class PullOverHoodie : ShirtBase
    {
        public PullOverHoodie() : base()
        {
            this.TypeName = "PullOverHoodie";
            this.IsActive = false;
            this.Colors = new Color[]
            {
                new Color("Black", true),
                new Color("Dark Heather", true),
                new Color("Heather Grey", true),
                new Color("Navy", true),
                new Color("Royal Blue", true),
            };
            this.FitTypes = null;
            this.MarketPlaces = new bool[] { true, false, false };
            this.Prices = new double[] { 31.99, 33.99, 36.99 };
        }
        public override object Clone()
        {
            PullOverHoodie target = new PullOverHoodie();
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
