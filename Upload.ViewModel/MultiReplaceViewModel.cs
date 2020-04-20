using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upload.Model;
using Upload.ViewModel.MVVMCore;

namespace Upload.ViewModel
{
    public class MultiReplaceViewModel : ViewModelBase
    {
        public RelayCommand GetFileNameCmd { get; set; }
        public RelayCommand ReplaceCmd { get; set; }
        public RelayCommand CloseCmd { get; set; }

        public MultiReplaceViewModel()
        {
            this.ListShirts = new ObservableCollection<DataGridModel>();
        }
        public ObservableCollection<DataGridModel> ListShirts{get;set;}
        
    }
    public class DataGridModel
    {
        public string PNGPath { get; set; }
        public string Descriptions { get; set; }
        public string Replace { get; set; }
        public string ReplaceWith { get; set; }

    }
}
