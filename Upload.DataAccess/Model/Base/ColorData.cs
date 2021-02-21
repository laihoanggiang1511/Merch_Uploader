using System.Runtime.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzUpload.DataAccess.Model
{
    [DataContract]
    public class ColorData: DataModelBase
    {
        [DataMember]
        public string ColorName { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
    }
}
