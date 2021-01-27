using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Common.MVVMCore;
using System.IO;

namespace Upload.ViewModel
{
    public class UploadWindowViewModel : ViewModelBase
    {
        public ICommand OpenChromeCmd { get; set; }
        public ICommand BrowseCmd { get; set; }
        public ICommand UploadCmd { get; set; }
        public ICommand AbortCmd { get; set; }
        public ICommand DeleteCmd { get; set; }
        public ICommand ChooseFolderCmd { get; set; }
        public ICommand SaveXmlCmd { get; set; }
        public ICommand EditShirtCmd { get; set; }
        public ICommand ShowConfigurationCmd { get; set; }
        public ICommand RemoveFolderCmd { get; set; }

        public string password;
        public ObservableCollection<Shirt> Shirts { get; set; }
        private Shirt selectedShirt;
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

                    if (selectedShirt != null)
                    {
                        AllowDelete = true;
                        EnableDescription = true;
                        ImagePath = SelectedShirt.ImagePath;
                        Descriptions = string.Empty;
                        if (selectedShirt.Languages != null && selectedShirt.Languages.Count > 0)
                        {
                            Language activeLanguage = selectedShirt.Languages.FirstOrDefault(x => string.IsNullOrEmpty(x.Title) == false);
                            if (activeLanguage != null)
                            {
                                Descriptions += "Brand: " + activeLanguage.BrandName + "\n" + "\n";
                                Descriptions += "Design Title: " + activeLanguage.Title + "\n" + "\n";
                                Descriptions += "Feature Bullet 1: " + activeLanguage.FeatureBullet1 + "\n" + "\n";
                                Descriptions += "Feature Bullet 2: " + activeLanguage.FeatureBullet2 + "\n" + "\n";
                                Descriptions += "Description: " + activeLanguage.Description;
                            }
                        }

                        //if (!string.IsNullOrEmpty(selectedShirt.FrontStdPath))
                        //    ImagePath = selectedShirt.FrontStdPath;
                        //else if (!string.IsNullOrEmpty(selectedShirt.BackStdPath))
                        //    ImagePath = selectedShirt.BackStdPath;
                        //else if (!string.IsNullOrEmpty(selectedShirt.FrontHoodiePath))
                        //    ImagePath = selectedShirt.FrontHoodiePath;
                        //else if (!string.IsNullOrEmpty(selectedShirt.BackHoodiePath))
                        //    ImagePath = selectedShirt.BackHoodiePath;
                        //else if (!string.IsNullOrEmpty(selectedShirt.PopSocketsGripPath))
                        //    ImagePath = selectedShirt.PopSocketsGripPath;

                        //Descriptions = string.Empty;
                        // if (!string.IsNullOrEmpty(SelectedShirt.BrandName))
                        // {
                        //     Descriptions += "Brand: " + SelectedShirt.BrandName + "\n" + "\n";
                        //     Descriptions += "Design Title: " + SelectedShirt.DesignTitle + "\n" + "\n";
                        //     Descriptions += "Feature Bullet 1: " + SelectedShirt.FeatureBullet1 + "\n" + "\n";
                        //     Descriptions += "Feature Bullet 2: " + SelectedShirt.FeatureBullet2 + "\n" + "\n";
                        //     Descriptions += "Description: " + SelectedShirt.Description;
                        // }
                        // else if (!string.IsNullOrEmpty(SelectedShirt.BrandNameGerman))
                        // {
                        //     Descriptions += "Brand: " + SelectedShirt.BrandNameGerman + "\n" + "\n";
                        //     Descriptions += "Design Title: " + SelectedShirt.DesignTitleGerman + "\n" + "\n";
                        //     Descriptions += "Feature Bullet 1: " + SelectedShirt.FeatureBullet1 + "\n" + "\n";
                        //     Descriptions += "Feature Bullet 2: " + SelectedShirt.FeatureBullet2 + "\n" + "\n";
                        //     Descriptions += "Description: " + SelectedShirt.DescriptionGerman;
                        // }
                    }
                    else
                    {
                        EnableDescription = false;
                        AllowDelete = false;
                    }

                    RaisePropertyChanged("SelectedShirt");
                }
            }
        }
        private bool enableDescription;
        public bool EnableDescription
        {
            get
            {
                return enableDescription;
            }
            set
            {
                if (enableDescription != value)
                {
                    enableDescription = value;
                    RaisePropertyChanged("EnableDescription");
                }
            }
        }
        private bool showConfiguration = false;
        public bool ShowConfiguration
        {
            get
            {
                return showConfiguration;
            }
            set
            {
                if (showConfiguration != value)
                {
                    showConfiguration = value;
                    RaisePropertyChanged("ShowConfiguration");
                }
            }
        }
        private bool allowDelete = false;
        public bool AllowDelete
        {
            get
            {
                return allowDelete;
            }
            set
            {
                if (allowDelete != value)
                {
                    allowDelete = value;
                    RaisePropertyChanged("AllowDelete");
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
                    descriptions = value;
                    RaisePropertyChanged("Descriptions");
                }
            }
        }

        private string imagePath = string.Empty;
        public string ImagePath
        {
            get
            {
                return imagePath;
            }
            set
            {
                if (imagePath != value)
                {
                    imagePath = value;
                    RaisePropertyChanged("ImagePath");
                }
            }
        }


        private string userFolderPath;
        public string UserFolderPath
        {
            get { return userFolderPath; }
            set
            {
                if (userFolderPath != value)
                {
                    userFolderPath = value;
                    if (!string.IsNullOrEmpty(userFolderPath) && userFolderPath.Contains("\\"))
                    {
                        SelectedPath = userFolderPath.Split('\\').Last();
                    }
                    else
                    {
                        SelectedPath = string.Empty;
                    }
                    RaisePropertyChanged("UserFolderPath");
                }
            }
        }

        private string addFolderName;
        public string AddFolderName
        {
            get { return addFolderName; }
            set
            {
                if (addFolderName != value)
                {
                    addFolderName = value;
                    RaisePropertyChanged("AddFolderName");
                }
            }
        }


        private string selectedPath;
        public string SelectedPath
        {
            get { return selectedPath; }
            set
            {
                if (selectedPath != value)
                {
                    selectedPath = value;
                    if (!string.IsNullOrEmpty(selectedPath))
                    {
                        userFolderPath = Path.Combine(GetDataDirectory(), selectedPath);
                    }
                    else
                    {
                        userFolderPath = string.Empty;
                    }
                    RaisePropertyChanged("SelectedPath");
                }
            }
        }
        public string GetDataDirectory()
        {
            string dataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            dataFolder += "\\Upload\\UserFolders";
            return dataFolder;
        }

        private ObservableCollection<string> userFolders = new ObservableCollection<string>();
        public ObservableCollection<string> UserFolders
        {
            get => userFolders;
            set
            {
                if (userFolders != value)
                {
                    userFolders = value;
                    RaisePropertyChanged("UserFolders");
                }
            }
        }

        private bool isUploading = false;
        public bool IsUploading
        {
            get { return isUploading; }
            set
            {
                if (isUploading != value)
                {
                    isUploading = value;
                    RaisePropertyChanged("IsUploading");
                }
            }
        }
        private string email;
        public string Email
        {
            get { return email; }
            set
            {
                if (email != value)
                {
                    email = value;
                    RaisePropertyChanged("Email");
                }
            }
        }

        public UploadWindowViewModel()
        {
            Shirts = new ObservableCollection<Shirt>();
        }
    }
}
