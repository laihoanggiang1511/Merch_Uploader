using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EzUpload.DataAccess.Model
{
    [DataContract]
    public class ShirtTypeData: DataModelBase
    {
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public List<bool> MarketPlaces { get; set; }
        [DataMember]
        public List<double> Prices { get; set; }
        [DataMember]
        public List<ColorData> Colors { get; set; }
        [DataMember]
        public List<bool> FitTypes { get; set; }
        [DataMember]
        public string TypeName { get; set; }

        public ShirtTypeData()
        {
            MarketPlaces = new List<bool>();
            Prices = new List<double>();
            Colors = new List<ColorData>();
            FitTypes = new List<bool>();
        }
        //public virtual object Clone()
        //{
        //    dynamic target = new System.Dynamic.ExpandoObject();
        //    target.IsActive = IsActive;
        //    target.MarketPlaces = MarketPlaces.Clone() as bool[];
        //    target.Prices = Prices.Clone() as double[];
        //    if (this.Colors != null)
        //    {
        //        List<Color> colors = new List<Color>();
        //        this.Colors.ToList().ForEach(x=>colors.Add(x));
        //        target.Colors = colors.ToArray();
        //    }
        //    else
        //    {
        //        target.Colors = null;
        //    }
        //    target.FitTypes = FitTypes;
        //    target.TypeName = TypeName;

        //    return target;
        //}
    }
}
