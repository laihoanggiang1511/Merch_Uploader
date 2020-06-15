using Upload.ViewModel.MVVMCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Upload.Model;

namespace Upload.ViewModel
{
    public class ShirtCreatorViewModel : ViewModelBase
    {
        public const string RootFolderPath = "Image/";
        public ICommand SaveCmd { get; set; }
        public ICommand OpenCmd { get; set; }
        public ICommand DeleteCmd { get; set; }
        public ICommand RemoveShirtCmd { get; set; }

        public ICommand SaveAllCmd { get; set; }
        public ICommand ClickFrontImageCmd { get; set; }
        public ICommand ClickBackImageCmd { get; set; }
        public ICommand ChangeColorCmd { get; set; }
        public ICommand MouseEnterCmd { get; set; }
        public ICommand RemoveFrontImageCmd { get; set; }
        public ICommand RemoveBackImageCmd { get; set; }
        public ICommand ReplaceCmd { get; set; }
        public ICommand SaveAsCmd { get; set; }
        public ICommand MultiReplaceCmd { get; set; }
        public ICommand EnterKeyCmd { get; set; }


        public ICommand ImageEditCmd { get; set; }


        public ShirtCreatorViewModel()
        {
            Shirt s = new Shirt();
            Shirts = new ObservableCollection<Shirt>();
            Shirts.Add(s);
            SelectedShirt = Shirts[0];
        }

        public MultiReplaceViewModel MultiReplaceVM { get; set; }

        bool createMode = true;
        public bool CreateMode
        {
            get
            {
                return createMode;
            }
            set
            {
                if (createMode != value)
                {
                    createMode = value;
                    if (createMode == false)
                    {
                        
                    }
                    RaisePropertyChanged("CreateMode");
                }
            }
        }
        private bool multiMode = false;
        public bool MultiMode
        {
            get
            {
                return multiMode;
            }
            set
            {
                if (multiMode != value)
                {
                    multiMode = value;
                    RaisePropertyChanged("MultiMode");
                }
            }
        }

        private Shirt selectedShirt = null;
        public Shirt SelectedShirt
        {
            get
            {
                return selectedShirt;
            }
            set
            {
                if (selectedShirt != value)
                {
                    selectedShirt = value;
                    if (SelectedShirt != null)
                    {
                        SelectedShirtType = SelectedShirt.ShirtTypes.FirstOrDefault(x => x.IsActive == true);
                        SelectedDescriptionIndex = 0;
                        UpdateDescriptionsFromShirt(SelectedShirt);

                    }
                    RaisePropertyChanged("SelectedShirt");
                    RaisePropertyChanged("FrontImagePath");
                    RaisePropertyChanged("BackImagePath");
                }

            }
        }
        private ShirtBase selectedShirtType = null;
        public ShirtBase SelectedShirtType
        {
            get
            {
                return selectedShirtType;
            }
            set
            {
                if (selectedShirtType != value)
                {
                    selectedShirtType = value;
                    RaisePropertyChanged("SelectedShirtType");

                    if (selectedShirtType != null)
                    {
                        NullToFalseConverter(selectedShirtType.FitTypes, FitTypesVisibility);
                        NullToFalseConverter(selectedShirtType.MarketPlaces, MarketPlacesVisibility);
                        RaisePropertyChanged("MarketPlacesVisibility");
                        RaisePropertyChanged("FitTypesVisibility");
                        RaisePropertyChanged("FrontImagePath");
                        RaisePropertyChanged("BackImagePath");
                        RaisePropertyChanged("CountColor");
                        if (SelectedShirtType.Colors != null)
                        {
                            var firstActiveColor = SelectedShirtType.Colors[0];
                            FrontMockup = RootFolderPath + SelectedShirtType.TypeName + "/" + firstActiveColor.ColorName + ".png";
                            BackMockup = RootFolderPath + SelectedShirtType.TypeName + "Back" + "/" + firstActiveColor.ColorName + ".png";
                        }
                        else //PopsocketsGrip
                        {
                            FrontMockup = RootFolderPath + SelectedShirtType.TypeName + "/PopSocketsGrip.png";
                            BackMockup = RootFolderPath + SelectedShirtType.TypeName + "/PopSocketsGrip.png";
                        }
                    }
                }

            }
        }

        private bool isOpenPopup = false;
        public bool IsOpenPopup
        {
            get
            {
                return isOpenPopup;
            }
            set
            {
                if (isOpenPopup != value)
                {
                    isOpenPopup = value;
                    RaisePropertyChanged("IsOpenPopup");
                }
            }
        }
        private string popupText = string.Empty;
        public string PopupText
        {
            get
            {
                return popupText;
            }
            set
            {
                if (popupText != value)
                {
                    popupText = value;
                    RaisePropertyChanged("PopupText");
                }
            }
        }

        private bool[] fitTypesVisibility = new bool[3];
        public bool[] FitTypesVisibility
        {
            get
            {
                return fitTypesVisibility;
            }
            set
            {
                if (fitTypesVisibility != value)
                {
                    fitTypesVisibility = value;
                    RaisePropertyChanged("FitTypesVisibility");
                }
            }
        }
        private bool[] marketPlacesVisibility = new bool[3];
        public bool[] MarketPlacesVisibility
        {
            get
            {
                return marketPlacesVisibility;
            }
            set
            {
                if (marketPlacesVisibility != value)
                {
                    marketPlacesVisibility = value;
                    RaisePropertyChanged("MarketPlacesVisibility");
                }
            }
        }


        public ObservableCollection<Shirt> Shirts { get; set; }

        private int countColor = 0;
        public int CountColor
        {
            get
            {
                if (SelectedShirtType != null)
                {
                    if (SelectedShirtType.Colors != null)
                        return SelectedShirtType.Colors.Where(x => x.IsActive == true).ToArray().Length;
                    else return countColor;
                }
                else return countColor;
            }
            set
            {
                if (countColor != value)
                {
                    countColor = value;
                    RaisePropertyChanged("CountColor");
                }
            }
        }
        private string replaceText = null;
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
        private string replaceWithText = null;
        public string ReplaceWithText
        {
            get
            {
                return replaceWithText;
            }
            set
            {
                if (replaceWithText != value)
                {
                    replaceWithText = value;
                    RaisePropertyChanged("ReplaceWithText");
                }
            }
        }

        #region Descriptions
        private int selectedDescriptionIndex = 0;
        public int SelectedDescriptionIndex
        {
            get
            {
                return selectedDescriptionIndex;
            }
            set
            {
                if (selectedDescriptionIndex != value)
                {
                    selectedDescriptionIndex = value;
                    UpdateDescriptionsFromShirt(SelectedShirt);
                    RaisePropertyChanged("SelectedDescriptionIdex");
                }
            }
        }
        private string descriptions = string.Empty;
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
                    UpdateDescriptionsToShirt(SelectedShirt, value);
                    //string strDescription = value;
                    //strDescription = strDescription.Replace("\r", "");
                    //char c = char.Parse("\n");
                    //string[] temp = strDescription.Split(c);
                    //for (int i = 0; i < temp.Length; i++)
                    //{
                    //    switch (i)
                    //    {
                    //        case 0:
                    //            BrandNameLength = temp[0].Length;
                    //            if (SelectedDescriptionIndex == 0)
                    //                SelectedShirt.BrandName = temp[i];
                    //            else
                    //                SelectedShirt.BrandNameGerman = temp[i];
                    //            break;
                    //        case 1:
                    //            DesignTitleLength = temp[1].Length;
                    //            if (SelectedDescriptionIndex == 0)
                    //                SelectedShirt.DesignTitle = temp[i];
                    //            else
                    //                SelectedShirt.DesignTitleGerman = temp[i];
                    //            break;
                    //        case 2:
                    //            Feature1Length = temp[2].Length;
                    //            if (SelectedDescriptionIndex == 0)
                    //                SelectedShirt.FeatureBullet1 = temp[i];
                    //            else
                    //                SelectedShirt.FeatureBullet1German = temp[i];
                    //            break;
                    //        case 3:
                    //            Feature2Length = temp[3].Length;
                    //            if (SelectedDescriptionIndex == 0)
                    //                SelectedShirt.FeatureBullet2 = temp[i];
                    //            else
                    //                SelectedShirt.FeatureBullet2German = temp[i];
                    //            break;
                    //        case 4:
                    //            DescriptionLength = temp[4].Length;
                    //            if (SelectedDescriptionIndex == 0)
                    //                SelectedShirt.Description = temp[i];
                    //            else
                    //                SelectedShirt.DescriptionGerman = temp[i];
                    //            break;
                    //    }
                    //}
                    descriptions = value;
                    RaisePropertyChanged("Descriptions");
                }
            }
        }

        private int brandNameLength = 0;
        public int BrandNameLength
        {
            get
            {
                return brandNameLength;
            }
            set
            {
                if (brandNameLength != value)
                {
                    brandNameLength = value;
                    RaisePropertyChanged("BrandNameLength");
                }
            }
        }
        private int designTitleLength = 0;
        public int DesignTitleLength
        {
            get
            {
                return designTitleLength;
            }
            set
            {
                if (designTitleLength != value)
                {
                    designTitleLength = value;
                    RaisePropertyChanged("DesignTitleLength");
                }
            }
        }
        private int feature1Length = 0;
        public int Feature1Length
        {
            get
            {
                return feature1Length;
            }
            set
            {
                if (feature1Length != value)
                {
                    feature1Length = value;
                    RaisePropertyChanged("Feature1Length");
                }
            }
        }
        private int feature2Length = 0;
        public int Feature2Length
        {
            get
            {
                return feature2Length;
            }
            set
            {
                if (feature2Length != value)
                {
                    feature2Length = value;
                    RaisePropertyChanged("Feature2Length");
                }
            }
        }
        private int descriptionLength = 0;
        public int DescriptionLength
        {
            get
            {
                return descriptionLength;
            }
            set
            {
                if (descriptionLength != value)
                {
                    descriptionLength = value;
                    RaisePropertyChanged("DescriptionLength");
                }
            }
        }
        public void UpdateDescriptionsFromShirt(Shirt shirt)
        {
            if (SelectedDescriptionIndex == 0)
            {
                Descriptions = shirt.BrandName + "\n" +
                    shirt.DesignTitle + "\n" +
                    shirt.FeatureBullet1 + "\n" +
                    shirt.FeatureBullet2 + "\n" +
                    shirt.Description + "\n";
            }
            else
            {
                Descriptions = shirt.BrandNameGerman + "\n" +
                    shirt.DesignTitleGerman + "\n" +
                    shirt.FeatureBullet1German + "\n" +
                    shirt.FeatureBullet2German + "\n" +
                    shirt.DescriptionGerman + "\n";
            }
        }
        public void UpdateDescriptionsToShirt(Shirt shirt,string input)
        {
            string strDescription = input;
            strDescription = strDescription.Replace("\r", "");
            char c = char.Parse("\n");
            string[] temp = strDescription.Split(c);
            for (int i = 0; i < temp.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        BrandNameLength = temp[0].Length;
                        if (SelectedDescriptionIndex == 0)
                            shirt.BrandName = temp[i];
                        else
                            shirt.BrandNameGerman = temp[i];
                        break;
                    case 1:
                        DesignTitleLength = temp[1].Length;
                        if (SelectedDescriptionIndex == 0)
                            shirt.DesignTitle = temp[i];
                        else
                            shirt.DesignTitleGerman = temp[i];
                        break;
                    case 2:
                        Feature1Length = temp[2].Length;
                        if (SelectedDescriptionIndex == 0)
                            shirt.FeatureBullet1 = temp[i];
                        else
                            shirt.FeatureBullet1German = temp[i];
                        break;
                    case 3:
                        Feature2Length = temp[3].Length;
                        if (SelectedDescriptionIndex == 0)
                            shirt.FeatureBullet2 = temp[i];
                        else
                            shirt.FeatureBullet2German = temp[i];
                        break;
                    case 4:
                        DescriptionLength = temp[4].Length;
                        if (SelectedDescriptionIndex == 0)
                            shirt.Description = temp[i];
                        else
                            shirt.DescriptionGerman = temp[i];
                        break;
                }
            }
        }


        #endregion

        private string frontMockup = RootFolderPath + "StandardTShirt" + "/Asphalt.png";
        public string FrontMockup
        {
            get
            {
                return frontMockup;
            }
            set
            {
                if (frontMockup != value)
                {
                    frontMockup = value;
                    RaisePropertyChanged("FrontMockup");
                }
            }
        }
        private string backMockup = RootFolderPath + "StandardTShirtBack" + "/Asphalt.png";
        public string BackMockup
        {
            get
            {
                return backMockup;
            }
            set
            {
                if (backMockup != value)
                {
                    backMockup = value;
                    RaisePropertyChanged("BackMockup");
                }
            }
        }
        private string frontImagePath = string.Empty;
        public string FrontImagePath
        {
            get
            {
                if (SelectedShirt != null)
                {
                    if (SelectedShirtType is PullOverHoodie ||
                        SelectedShirtType is ZipHoodie)
                    {
                        return SelectedShirt.FrontHoodiePath;
                    }
                    else if (SelectedShirtType is PopSocketsGrip)
                    {
                        return SelectedShirt.PopSocketsGripPath;
                    }
                    else return SelectedShirt.FrontStdPath;
                }
                else return string.Empty;
            }
            set
            {
                if (frontImagePath != value)
                {
                    frontImagePath = value;
                    RaisePropertyChanged("FrontImagePath");
                }
            }
        }
        private string backImagePath = string.Empty;
        public string BackImagePath
        {
            get
            {
                if (SelectedShirt != null)
                {
                    if (SelectedShirtType is PullOverHoodie ||
                        SelectedShirtType is ZipHoodie)
                    {
                        return SelectedShirt.BackHoodiePath;
                    }
                    else if (SelectedShirtType is PopSocketsGrip)
                    {
                        return SelectedShirt.PopSocketsGripPath;
                    }
                    else return SelectedShirt.BackStdPath;
                }
                else return string.Empty;
            }
            set
            {
                if (backImagePath != value)
                {
                    backImagePath = value;
                    RaisePropertyChanged("BackImagePath");
                }
            }
        }


        private void NullToFalseConverter(Array source, bool[] target)
        {
            int startIndex = 0;
            if (source != null)
            {
                startIndex = source.Length;
            }
            for (int i = 0; i < target.Length; i++)
            {
                if (i < startIndex)
                {
                    target[i] = true;
                }
                else
                    target[i] = false;
            }
        }
    }
}
