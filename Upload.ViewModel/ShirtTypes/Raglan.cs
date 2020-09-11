using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Upload.ViewModel
{
    public class Raglan:ShirtType
    {
        public Raglan() : base()
        {
            this.TypeName = "Raglan";
            this.IsActive = false;
            this.Colors = new ObservableCollection<Color> //Neeed to Complete
            {
                new Color("Black-Althetic Heather", true),
                new Color("Black-White", true),
                new Color("Dark Heather-White", true),
                new Color("Navy-Athletic Heather", true),
                new Color("Navy-White", true),
                new Color("Red-White", true),
                new Color("Royal Blue-White", true),
            };
            this.FitTypes = new ObservableCollection<bool> { true, true };
            this.MarketPlaces = new ObservableCollection<bool> { true,false,false};
            this.Prices = new ObservableCollection<double> { 23.99,17.99,19.99 };
        }
       
    }
}
