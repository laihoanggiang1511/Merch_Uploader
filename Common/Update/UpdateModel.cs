using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Update
{
    public class UpdateModel
    {
        public string UpdateURL { get; set; }
        public string ServerVersion { get; set; }
        public string IsProductValid { get; set; }
        public string Descriptions { get; set; }
        public int ProductId { get; set; }
    }
}
