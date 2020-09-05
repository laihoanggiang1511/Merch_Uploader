﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upload.Definitions;

namespace Upload.Model
{
    public class LongSleeveTShirt:ShirtBase
    {
        public LongSleeveTShirt():base()
        {
            this.TypeName = "LongSleeveTShirt";
            this.IsActive = false;
            this.MarketPlaces = new bool[] { true, false, false };
            this.FitTypes = null; //Ko co
            this.Colors = new Color[] 
            {
                new Color("Black", true),
                new Color("Dark Heather", true),
                new Color("Heather Grey", true),
                new Color("Navy", true),
                new Color("Blue", true),
            };
            this.Prices = new double[] { 22.99, 21.99, 22.99 };
        }
        public override object Clone()
        {
            LongSleeveTShirt target = new LongSleeveTShirt();
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
