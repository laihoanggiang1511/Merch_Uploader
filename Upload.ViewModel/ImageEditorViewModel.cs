using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Upload.ViewModel.MVVMCore;

namespace Upload.ViewModel
{
    public class ImageEditorViewModel : ViewModelBase
    {
        public RelayCommand CropCmd { get; set; }
        public RelayCommand BrowseCmd { get; set; }
        public ImageEditorViewModel()
        {
            ListInputPath = new ObservableCollection<string>();
        }
        private string imageSource = string.Empty;
        public string ImageSource
        {
            get
            {
                return imageSource;
            }
            set
            {
                if (imageSource != value)
                {
                    imageSource = value;
                    RaisePropertyChanged("ImageSource");
                }
            }
        }
        private Image currentImage;
        public Image CurrentImage
        {
            get
            {
                return currentImage;
            }
            set
            {
                if (currentImage == value)
                {
                    currentImage = value;
                }
            }
        }
        public ObservableCollection<string> ListInputPath { get; set; }

        private string inputFilePath = string.Empty;
        public string InputFilePath
        {
            get
            {
                return inputFilePath;
            }
            set
            {
                if (inputFilePath != value)
                {
                    inputFilePath = value;
                    RaisePropertyChanged("InputFilePath");
                }
            }
        }
        private int selectedModeIndex = 0;
        public int SelectedModeIndex
        {
            get
            {
                return selectedModeIndex;
            }
            set
            {
                if (selectedModeIndex != value)
                {
                    selectedModeIndex = value;
                    RaisePropertyChanged("SelectedModeIndex");
                }
                switch (SelectedModeIndex)
                {
                    case 0: //Crop Top
                    case 1: break; //Crop Bottom
                    case 2: break; //Crop Both
                    case 3: break; //stretch
                }
            }
        }

        private bool enableSelectMode = true;
        public bool EnableSelectMode
        {
            get
            {
                return enableSelectMode;
            }
            set
            {
                if (enableSelectMode != value)
                {
                    enableSelectMode = value;
                    RaisePropertyChanged("EnableSelectMode");
                }
            }
        }
        private int outputTypeIndex = 0;
        public int OutputTypeIndex
        {
            get
            {
                return outputTypeIndex;
            }
            set
            {
                if (outputTypeIndex != value)
                {
                    outputTypeIndex = value;
                    if (outputTypeIndex == 0)//standard t-shirt
                    {
                        EnableSelectMode = false;
                        TopHeight = 0;
                        BotHeight = 0;
                    }
                    else if (outputTypeIndex == 1)//hoodie
                    {
                        EnableSelectMode = true;
                        SelectedModeIndex = 2;//crop both
                    }

                    RaisePropertyChanged("OutputTypeIndex");
                }
            }
        }
        private int topHeight = 0;

        public int TopHeight
        {
            get
            {
                return topHeight;
            }
            set
            {
                if (topHeight != value)
                {
                    topHeight = value;
                    RaisePropertyChanged("TopHeight");
                }
            }
        }
        private int midHeight = 0;

        public int MidHeight
        {
            get
            {
                return midHeight;
            }
            set
            {
                if (midHeight != value)
                {
                    midHeight = value;
                    RaisePropertyChanged("MidHeight");
                }
            }
        }
        private int botHeight = 0;

        public int BotHeight
        {
            get
            {
                return botHeight;
            }
            set
            {
                if (botHeight != value)
                {
                    botHeight = value;
                    RaisePropertyChanged("BotHeight");
                }
            }
        }

        private int topWidth = 0;

        public int TopWidth
        {
            get
            {
                return topWidth;
            }
            set
            {
                if (topWidth != value)
                {
                    topWidth = value;
                    RaisePropertyChanged("TopWidth");
                }
            }
        }
        private int midWidth = 0;

        public int MidWidth
        {
            get
            {
                return midWidth;
            }
            set
            {
                if (midWidth != value)
                {
                    midWidth = value;
                    RaisePropertyChanged("MidWidth");
                }
            }
        }
        private int botWidth = 0;
        public int BotWidth
        {
            get
            {
                return botWidth;
            }
            set
            {
                if (botWidth != value)
                {
                    botWidth = value;
                    RaisePropertyChanged("BotWidth");
                }
            }
        }
    }

}
