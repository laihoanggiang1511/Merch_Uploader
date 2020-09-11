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
                new LanguageData (0), //English
                new LanguageData(1),
                new LanguageData(2),
                new LanguageData(3),
                new LanguageData (4),
            };
            ShirtTypes = new List<ShirtTypeData>();
        }
    }
}
