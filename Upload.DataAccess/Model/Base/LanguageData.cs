using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EzUpload.DataAccess.Model
{
    [DataContract]
    public class LanguageData: DataModelBase
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string BrandName { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string FeatureBullet1 { get; set; }
        [DataMember]
        public string FeatureBullet2 { get; set; }
        [DataMember]
        public string Description { get; set; }
        public LanguageData(int id)
        {
            this.Id = id;
        }
        public LanguageData() 
        {
            this.Id = 0;
        }
        public LanguageData(int id,string name)
        {
            this.Id = id;
            this.Name = Name;
        }
    }
}
