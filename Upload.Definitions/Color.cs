using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upload.Definitions
{
    public class Color
    {
        public string ColorName { get; set; }
        public bool IsActive { get; set; }
        public Color()
        {
            IsActive = false;
            ColorName = string.Empty;
        }
        public Color(bool IsActive)
        {
            this.IsActive = IsActive;
            this.ColorName = string.Empty;
        }
        public Color(string Name,bool IsActive)
        {
            this.IsActive = IsActive;
            this.ColorName = Name;
        }
    }
}
