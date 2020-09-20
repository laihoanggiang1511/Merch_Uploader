using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Upload.DataAccess.Model
{
    [DataContract]
    public class ShirtData
    {
        [DataMember]
        public string ImagePath { get; set; }
        [DataMember]
        public int ImageType { get; set; }
        [DataMember]
        public List<LanguageData> Languages { get; set; }
        [DataMember]
        public List<ShirtTypeData> ShirtTypes { get; set; }
        public ShirtData()
        {
            this.ImageType = 0;
            Languages = new List<LanguageData>()
            {
                new LanguageData (0,"English"), //English
                new LanguageData(1, "German"),
                new LanguageData(2, "French"),
                new LanguageData(3, "Italian"),
                new LanguageData (4, "Spanish"),
            };
            ShirtTypes = new List<ShirtTypeData>();
        }
    }
}
