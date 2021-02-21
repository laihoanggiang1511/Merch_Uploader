using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzUpload.ViewModel
{
    public class TankTop : ShirtType
    {
        public TankTop() : base()
        {
            this.TypeName = "TankTop";
            this.IsActive = false;
            this.MarketPlaces = new ObservableCollection<bool> { false };
            this.Colors = new ObservableCollection<Color>
            {
                new Color("Black",true),
                new Color("Dark Heather",true),
                new Color("Heather Grey",true),
                new Color("Navy",true),
                new Color("Pink",false),
                new Color("Purple",true),
                new Color("Red",false),
                new Color("Royal Blue",true),
                new Color("Sapphire",false),
                new Color("White",false),
            };
            this.FitTypes = new ObservableCollection<bool> { true, true };
            this.MarketPlaces = new ObservableCollection<bool> { true ,false,false};
            this.Prices = new ObservableCollection<double> { 19.99,16.69,17.99 };
        }
       
    }
}
