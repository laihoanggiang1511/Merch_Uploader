﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upload.Definitions;


namespace Upload.Model
{
    public class Raglan:ShirtBase
    {
        public Raglan() : base()
        {
            this.TypeName = "Raglan";
            this.IsActive = false;
            this.Colors = new Color[] //Neeed to Complete
            {
                new Color("Black-Althetic Heather", true),
                new Color("Black-White", true),
                new Color("Dark Heather-White", true),
                new Color("Navy-Athletic Heather", true),
                new Color("Navy-White", true),
                new Color("Red-White", true),
                new Color("Royal Blue-White", true),
            };
            this.FitTypes = new bool[] { true, true };
            this.MarketPlaces = new bool[] { true,false,false};
            this.Prices = new double[] { 23.99,17.99,19.99 };
        }
    }
}
