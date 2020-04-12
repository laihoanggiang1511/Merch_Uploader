﻿using System;
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

    }
}
