using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upload.Definitions;

namespace Upload.Model
{
    public class ShirtBase
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
    }
}
