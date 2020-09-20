using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Upload.ViewModel
{
    public class Language
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string BrandName { get; set; }
        public string Title { get; set; }
        public string FeatureBullet1 { get; set; }
        public string FeatureBullet2 { get; set; }
        public string Description { get; set; }
        public Language(int Id)
        {
            this.Id = Id;
        }
        public Language()
        {
            this.Id = 0;
        }
        public Language(int id, string name)
        {
            this.Id = 0;
            this.Name = name;
        }
    }
}
