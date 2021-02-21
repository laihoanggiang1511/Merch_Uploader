using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EzUpload.ViewModel
{
    public class ShirtType : ICloneable
    {
        public bool IsActive { get; set; }
        public ObservableCollection<bool> MarketPlaces { get; set; }
        public ObservableCollection<double> Prices { get; set; }
        public ObservableCollection<Color> Colors { get; set; }
        public ObservableCollection<bool> FitTypes { get; set; }
        public string TypeName { get; set; }
        public ShirtType()
        {
         
            this.IsActive = false;
            MarketPlaces = new ObservableCollection<bool>();
            Prices = new ObservableCollection<double>();
            Colors = new ObservableCollection<Color>();
            FitTypes = new ObservableCollection<bool>();
        }

        public object Clone()
        {
            dynamic target = new System.Dynamic.ExpandoObject();
            target.IsActive = IsActive;
            target.MarketPlaces = new ObservableCollection<bool>();
            if (MarketPlaces == null)
                target.MarketPlaces = null;
            else
                MarketPlaces.ToList().ForEach(x => target.MarketPlaces.Add(x));

            target.Prices = new ObservableCollection<double>();
            if (Prices == null)
                target.Price = null;
            else
                Prices.ToList().ForEach(x => target.Price.Add(x));

            if (this.Colors != null)
            {
                ObservableCollection<Color> colors = new ObservableCollection<Color>();
                this.Colors.ToList().ForEach(x => colors.Add(x));
                target.Colors = colors;
            }
            else
            {
                target.Colors = null;
            }
            target.FitTypes = new ObservableCollection<bool>(); //

            if (FitTypes == null)
                target.FitTypes = null;
            else
                FitTypes.ToList().ForEach(x => target.FitTypes.Add(x));

            target.TypeName = TypeName;
            return target;
        }
    }
}
