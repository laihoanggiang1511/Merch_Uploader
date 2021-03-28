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

namespace EzUpload.ViewModel
{
   public class UploadWindowViewModel : ViewModelBase
   {
      public UploadPlatform UploadPlatform { get; set; }
      private string _userFolderDirectory { get; }

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
      public ICommand FolderChangeCmd { get; set; }
      public ICommand AutoModeEnableCmd { get; set; }
      public ICommand BrowseUploadFolderCmd { get; set; }
      public ICommand StartAutoUploadCmd { get; set; }

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
                        Descriptions += "Description: " + activeLanguage.Description + "\n" + "\n";
                        Descriptions += "Main Tags: " + selectedShirt.MainTag + "\n" + "\n";
                        Descriptions += "Supporting Tags: " + selectedShirt.SupportingTags + "\n" + "\n";
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
                  userFolderPath = Path.Combine(_userFolderDirectory, selectedPath);
               }
               else
               {
                  userFolderPath = string.Empty;
               }
               if (FolderChangeCmd != null)
               {
                  FolderChangeCmd.Execute(this);
               }
               RaisePropertyChanged("SelectedPath");
            }
         }
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
      private int dailyUploadLimit = 50;
      public int DailyUploadLimit
      {
         get { return dailyUploadLimit; }
         set
         {
            if (dailyUploadLimit != value)
            {
               dailyUploadLimit = value;
               RaisePropertyChanged("DailyUploadLimit");
            }
         }
      }

      private string uploadFolder;
      public string UploadFolder
      {
         get { return uploadFolder; }
         set
         {
            if (uploadFolder != value)
            {
               uploadFolder = value;
               RaisePropertyChanged("UploadFolder");
            }
         }
      }

      private bool autoUploadModeEnable = false;
      public bool AutoUploadModeEnable
      {
         get { return autoUploadModeEnable; }
         set
         {
            if (autoUploadModeEnable != value)
            {
               autoUploadModeEnable = value;
               RaisePropertyChanged("AutoUploadModeEnable");
            }
         }
      }

      private string autoUploadLog = String.Empty;
      public string AutoUploadLog
      {
         get { return autoUploadLog; }
         set
         {
            if (autoUploadLog != value)
            {
               autoUploadLog = value;
               RaisePropertyChanged("AutoUploadLog");
            }
         }
      }
      public UploadWindowViewModel(string dataFolder)
      {
         Shirts = new ObservableCollection<Shirt>();
         AutoModeEnableCmd = new RelayCommand(x => { (x as UploadWindowViewModel).AutoUploadModeEnable = true; });
         _userFolderDirectory = dataFolder;
      }
   }
}
