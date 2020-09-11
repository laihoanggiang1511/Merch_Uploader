using System.Runtime.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.MVVMCore;

namespace Upload.ViewModel
{
    public class Color : ViewModelBase  
    {
        private string colorName = string.Empty;
        public string ColorName
        {
            get => colorName;
            set
            {
                if (colorName != value)
                {
                    colorName = value;
                    RaisePropertyChanged("ColorName");
                }
            }

        }
        private bool isActive = false;
        public bool IsActive
        {
            get => isActive;
            set
            {
                if (isActive != value)
                {
                    isActive = value;
                    RaisePropertyChanged("IsActive");
                }
            }
        }

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
        public Color(string Name, bool IsActive)
        {
            this.IsActive = IsActive;
            this.ColorName = Name;
        }
    }

}
