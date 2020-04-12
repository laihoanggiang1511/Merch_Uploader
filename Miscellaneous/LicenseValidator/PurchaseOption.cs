using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Brush = System.Windows.Media.Brush;

namespace Miscellaneous
{
    public class PurchaseOption
    {
        public Image OptionImage { get; set; }
        public double Price { get; set; }
        public double NumberOfDevices { get; set; }
        public string PurchaseURL { get; set; }
        public string Header1 { get; set; }
        public string Header2 { get; set; }
        public string Feature1 { get; set; }
        public string Feature2 { get; set; }
        public string Feature3 { get; set; }
        public string Feature4 { get; set; }
        public string Feature5 { get; set; }

        public Brush TextColor { get; set; }
        public PurchaseOption(string PurchaseURL)
        {
            this.PurchaseURL = PurchaseURL;
        }
    }
}
