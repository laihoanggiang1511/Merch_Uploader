using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.MVVMCore;

namespace Upload.ViewModel
{
    public class MultiReplaceViewModel : ViewModelBase
    {
        public RelayCommand GetFileNameCmd { get; set; }
        public RelayCommand ReplaceAllCmd { get; set; }
        public RelayCommand CloseCmd { get; set; }

        public RelayCommand SaveCmd { get; set; }
        public RelayCommand EnterKeyCmd { get; set; }


        public MultiReplaceViewModel()
        {
            this.ListShirts = new ObservableCollection<DataGridModel>();
        }
        public ObservableCollection<DataGridModel> ListShirts{get;set;}
        
    }
    public class DataGridModel:ViewModelBase
    {
        private string pNGPath = string.Empty;

        public string PNGPath
        {
            get
            {
                return pNGPath;
            }
            set
            {
                if (pNGPath != value)
                {
                    pNGPath = value;
                    RaisePropertyChanged("PNGPath");
                }
            }
        }

        private string descriptions=string.Empty;

        public string Descriptions
        {
            get
            {
                return descriptions;
            }
            set
            {
                if (descriptions != value)
                {
                    descriptions = value;
                    RaisePropertyChanged("Descriptions");
                }
            }
        }
        private string replaceText = string.Empty;

        public string ReplaceText
        {
            get
            {
                return replaceText;
            }
            set
            {
                if (replaceText != value)
                {
                    replaceText = value;
                    RaisePropertyChanged("ReplaceText");
                }
            }
        }
        private string replaceByText = string.Empty;

        public string ReplaceByText
        {
            get
            {
                return replaceByText;
            }
            set
            {
                if (replaceByText != value)
                {
                    replaceByText = value;
                    RaisePropertyChanged("ReplaceByText");
                }
            }
        }

    }
}
