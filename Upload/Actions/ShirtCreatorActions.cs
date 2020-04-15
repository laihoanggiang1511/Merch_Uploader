using Upload.GUI;
using Upload.ViewModel;
using Upload.ViewModel.MVVMCore;
using System.Linq;
using Upload.DataAccess;
using Upload.Model;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Controls.Primitives;
using Upload.Definitions;
using System.Windows;
using System.Windows.Forms;
using System.Collections.Generic;


namespace Upload.Actions
{
    internal class ShirtCreatorActions
    {
        ShirtCreatorViewModel shirtVM = null;
        public void ShowShirtCreatorWindow(Shirt editShirt = null)
        {
            ShirtCreatorView shirtCreatorWindow = new ShirtCreatorView();
            shirtVM = new ShirtCreatorViewModel();
            shirtVM.SaveCmd = new RelayCommand(SaveCmdInvoke);
            shirtVM.ClickFrontImageCmd = new RelayCommand(ClickFrontImageCmdInvoke);
            shirtVM.ClickBackImageCmd = new RelayCommand(ClickBackImageCmdInvoke);
            shirtVM.OpenCmd = new RelayCommand(OpenCmdInvoke);
            shirtVM.ChangeColorCmd = new RelayCommand(ChangeColorCmdInvoke);
            shirtVM.MouseEnterCmd = new RelayCommand(MouseEnterCmdInvoke);
            shirtVM.RemoveBackImageCmd = new RelayCommand(RemoveBackImageCmdInvoke);
            shirtVM.RemoveFrontImageCmd = new RelayCommand(RemoveFrontImageCmdInvoke);
            shirtVM.ReplaceCmd = new RelayCommand(ReplaceCmdInvoke);
            shirtVM.SaveAsCmd = new RelayCommand(SaveAsCmdInvoke);
            shirtVM.ImageEditCmd = new RelayCommand(ImageEditCmdInvoke);
            if (editShirt != null)
            {
                shirtVM.SelectedShirt = editShirt;
                shirtVM.CreateMode = false;
            }

            if (shirtVM.SelectedShirt.ShirtTypes != null)
                shirtVM.SelectedShirtType = shirtVM.SelectedShirt.ShirtTypes.FirstOrDefault(x => x.IsActive == true);
            shirtCreatorWindow.DataContext = shirtVM;
            shirtCreatorWindow.Show();
        }

        private void ImageEditCmdInvoke(object obj)
        {
            if (obj is ShirtCreatorViewModel shirtVM)
            {
                if (shirtVM.SelectedShirtType is StandardTShirt)
                {
                    new ImageEditorActions().ShowImageEditorWindow(shirtVM.FrontImagePath);
                }
            }
            else if (obj is string strImagePath)
            {
                new ImageEditorActions().ShowImageEditorWindow(strImagePath);
            }
        }

        private void SaveAsCmdInvoke(object obj)
        {
            if (obj is ShirtCreatorViewModel shirtVM)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    OverwritePrompt = true,
                    Filter = "Data file |*.xml",
                    FileName = "Template1",
                };
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    ShirtStatus errorCode = 0;
                    try
                    {
                        if (shirtVM != null)
                        {
                            if (shirtVM.SelectedShirt != null)
                            {
                                XMLDataAccess dataAccess = new XMLDataAccess(saveFileDialog.FileName);
                                dataAccess.SaveShirt(shirtVM.SelectedShirt);
                                System.Windows.MessageBox.Show(string.Format("Shirt saved in {0}", dataAccess.XmlFilePath), "Save Shirt", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            else
                            {
                                Utils.ShowWarningMessageBox(GetErrorMessage(errorCode));
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Utils.ShowErrorMessageBox(ex.Message);
                    }
                }
            }
        }


        private void ReplaceCmdInvoke(object obj)
        {
            ShirtCreatorViewModel shirtCreatorVM = obj as ShirtCreatorViewModel;

            if (shirtCreatorVM != null)
            {
                if (!string.IsNullOrEmpty(shirtCreatorVM.ReplaceText))
                {
                    string str = shirtCreatorVM.Descriptions.Replace(shirtCreatorVM.ReplaceText, shirtCreatorVM.ReplaceWithText);
                    shirtCreatorVM.Descriptions = str;
                }
            }
        }

        private void RemoveFrontImageCmdInvoke(object obj)
        {
            ShirtCreatorViewModel shirtCreatorVM = obj as ShirtCreatorViewModel;
            if (shirtCreatorVM.SelectedShirtType is LongSleeveTShirt ||
                    shirtCreatorVM.SelectedShirtType is PremiumTShirt ||
                    shirtCreatorVM.SelectedShirtType is Raglan ||
                    shirtCreatorVM.SelectedShirtType is StandardTShirt ||
                    shirtCreatorVM.SelectedShirtType is SweetShirt ||
                    shirtCreatorVM.SelectedShirtType is TankTop ||
                    shirtCreatorVM.SelectedShirtType is VNeckTShirt)
            {
                shirtCreatorVM.SelectedShirt.FrontStdPath = string.Empty;
                shirtCreatorVM.FrontImagePath = string.Empty;
            }
            else
            if (shirtCreatorVM.SelectedShirtType is PullOverHoodie ||
                        shirtCreatorVM.SelectedShirtType is ZipHoodie)
            {
                shirtCreatorVM.SelectedShirt.FrontHoodiePath = string.Empty;
                shirtCreatorVM.FrontImagePath = string.Empty;
            }
            else if (shirtCreatorVM.SelectedShirtType is PopSocketsGrip)
            {
                shirtCreatorVM.SelectedShirt.PopSocketsGripPath = string.Empty;
                shirtCreatorVM.FrontImagePath = string.Empty;
                shirtCreatorVM.BackImagePath = string.Empty;
            }
        }

        private void RemoveBackImageCmdInvoke(object obj)
        {
            ShirtCreatorViewModel shirtCreatorVM = obj as ShirtCreatorViewModel;
            if (shirtCreatorVM.SelectedShirtType is LongSleeveTShirt ||
                    shirtCreatorVM.SelectedShirtType is PremiumTShirt ||
                    shirtCreatorVM.SelectedShirtType is Raglan ||
                    shirtCreatorVM.SelectedShirtType is StandardTShirt ||
                    shirtCreatorVM.SelectedShirtType is SweetShirt ||
                    shirtCreatorVM.SelectedShirtType is TankTop ||
                    shirtCreatorVM.SelectedShirtType is VNeckTShirt)
            {
                shirtCreatorVM.SelectedShirt.BackStdPath = string.Empty;
                shirtCreatorVM.BackImagePath = string.Empty;
            }
            else
            if (shirtCreatorVM.SelectedShirtType is PullOverHoodie ||
                        shirtCreatorVM.SelectedShirtType is ZipHoodie)
            {
                shirtCreatorVM.SelectedShirt.BackHoodiePath = string.Empty;
                shirtCreatorVM.BackImagePath = string.Empty;
            }
            else if (shirtCreatorVM.SelectedShirtType is PopSocketsGrip)
            {
                shirtCreatorVM.SelectedShirt.PopSocketsGripPath = string.Empty;
                shirtCreatorVM.FrontImagePath = string.Empty;
                shirtCreatorVM.BackImagePath = string.Empty;
            }
        }

        private void OpenCmdInvoke(object obj)
        {
            if (obj is ShirtCreatorViewModel mainVM)
            {
                if (mainVM.CreateMode == true)
                {
                    List<Shirt> lstShirts = Utils.BrowseForShirts();
                    if (lstShirts.Count == 1)
                    {
                        mainVM.SelectedShirt = lstShirts[0];
                    }
                    else
                    {
                        lstShirts.ForEach(x => mainVM.Shirts.Add(x));
                        if (mainVM.SelectedShirt == null && mainVM.Shirts != null && mainVM.Shirts.Count > 0)
                        {
                            mainVM.SelectedShirt = mainVM.Shirts.FirstOrDefault();
                        }
                    }
                }
            }
        }

        private void ClickFrontImageCmdInvoke(object obj)
        {
            ShirtCreatorViewModel shirtCreatorVM = obj as ShirtCreatorViewModel;
            if (shirtCreatorVM != null)
            {
                string[] browseResult = Utils.BrowseForFilePath(multiselect: shirtCreatorVM.MultiMode);
                if (shirtCreatorVM.MultiMode == false)
                {
                    string imagePath = browseResult[0];
                    if (!string.IsNullOrEmpty(shirtCreatorVM.SelectedShirt.DesignTitle))
                        shirtCreatorVM.SelectedShirt.DesignTitle = Path.GetFileNameWithoutExtension(imagePath);
                    if (shirtCreatorVM.SelectedShirtType is LongSleeveTShirt ||
                        shirtCreatorVM.SelectedShirtType is PremiumTShirt ||
                        shirtCreatorVM.SelectedShirtType is Raglan ||
                        shirtCreatorVM.SelectedShirtType is StandardTShirt ||
                        shirtCreatorVM.SelectedShirtType is SweetShirt ||
                        shirtCreatorVM.SelectedShirtType is TankTop ||
                        shirtCreatorVM.SelectedShirtType is VNeckTShirt)
                    {
                        if (ValidateImage(imagePath, 4500, 5400))
                        {
                            shirtCreatorVM.SelectedShirt.FrontStdPath = imagePath;
                            shirtCreatorVM.FrontImagePath = shirtCreatorVM.SelectedShirt.FrontStdPath;
                        }

                    }
                    else
                    if (shirtCreatorVM.SelectedShirtType is PullOverHoodie ||
                            shirtCreatorVM.SelectedShirtType is ZipHoodie)
                    {
                        if (ValidateImage(imagePath, 4500, 4050))
                        {
                            shirtCreatorVM.SelectedShirt.FrontHoodiePath = imagePath;
                            shirtCreatorVM.FrontImagePath = shirtCreatorVM.SelectedShirt.FrontHoodiePath;
                        }
                    }
                    else if (shirtCreatorVM.SelectedShirtType is PopSocketsGrip)
                    {
                        if (ValidateImage(imagePath, 485, 485))
                        {
                            shirtCreatorVM.SelectedShirt.PopSocketsGripPath = imagePath;
                            shirtCreatorVM.FrontImagePath = imagePath;
                            shirtCreatorVM.BackImagePath = imagePath;
                        }
                    }
                }
                else
                {
                    if (shirtCreatorVM.Shirts == null)
                        shirtCreatorVM.Shirts = new System.Collections.ObjectModel.ObservableCollection<Shirt>();
                    foreach (string imagePath in browseResult)
                    {
                        Shirt shirt = shirtCreatorVM.SelectedShirt.Clone() as Shirt;

                        if (!string.IsNullOrEmpty(shirt.DesignTitle))
                            shirt.DesignTitle = Path.GetFileNameWithoutExtension(imagePath);

                        if (shirtCreatorVM.SelectedShirtType is LongSleeveTShirt ||
                             shirtCreatorVM.SelectedShirtType is PremiumTShirt ||
                             shirtCreatorVM.SelectedShirtType is Raglan ||
                             shirtCreatorVM.SelectedShirtType is StandardTShirt ||
                             shirtCreatorVM.SelectedShirtType is SweetShirt ||
                             shirtCreatorVM.SelectedShirtType is TankTop ||
                             shirtCreatorVM.SelectedShirtType is VNeckTShirt)
                        {
                            if (ValidateImage(imagePath, 4500, 5400))
                            {
                                shirt.FrontStdPath = imagePath;
                                shirtCreatorVM.FrontImagePath = shirtCreatorVM.SelectedShirt.FrontStdPath;
                            }
                        }
                        else
                        if (shirtCreatorVM.SelectedShirtType is PullOverHoodie ||
                                shirtCreatorVM.SelectedShirtType is ZipHoodie)
                        {
                            if (ValidateImage(imagePath, 4500, 4050))
                            {
                                shirt.FrontHoodiePath = imagePath;
                                shirtCreatorVM.FrontImagePath = shirtCreatorVM.SelectedShirt.FrontHoodiePath;
                            }
                        }
                        else if (shirtCreatorVM.SelectedShirtType is PopSocketsGrip)
                        {
                            if (ValidateImage(imagePath, 485, 485))
                            {
                                shirt.PopSocketsGripPath = imagePath;
                                shirtCreatorVM.FrontImagePath = imagePath;
                                shirtCreatorVM.BackImagePath = imagePath;
                            }
                        }
                        shirtCreatorVM.Shirts.Add(shirt);
                        shirt = null;
                    }
                }
            }
        }
        private void ClickBackImageCmdInvoke(object obj)
        {
            ShirtCreatorViewModel shirtCreatorVM = obj as ShirtCreatorViewModel;
            if (shirtCreatorVM != null)
            {
                string imagePath = Utils.BrowseForFilePath()[0];
                if (shirtCreatorVM.SelectedShirtType is LongSleeveTShirt ||
                    shirtCreatorVM.SelectedShirtType is PremiumTShirt ||
                    shirtCreatorVM.SelectedShirtType is Raglan ||
                    shirtCreatorVM.SelectedShirtType is StandardTShirt ||
                    shirtCreatorVM.SelectedShirtType is SweetShirt ||
                    shirtCreatorVM.SelectedShirtType is TankTop ||
                    shirtCreatorVM.SelectedShirtType is VNeckTShirt)
                {
                    if (ValidateImage(imagePath, 4500, 5400))
                    {
                        shirtCreatorVM.SelectedShirt.BackStdPath = imagePath;
                        shirtCreatorVM.BackImagePath = shirtCreatorVM.SelectedShirt.BackStdPath;
                    }
                }
                else
                if (shirtCreatorVM.SelectedShirtType is PullOverHoodie ||
                        shirtCreatorVM.SelectedShirtType is ZipHoodie)
                {
                    if (ValidateImage(imagePath, 4500, 5400))
                    {
                        shirtCreatorVM.SelectedShirt.BackHoodiePath = imagePath;
                        shirtCreatorVM.BackImagePath = shirtCreatorVM.SelectedShirt.BackHoodiePath;
                    }
                }
                else if (shirtCreatorVM.SelectedShirtType is PopSocketsGrip)
                {
                    if (ValidateImage(imagePath, 485, 485))
                    {
                        shirtCreatorVM.SelectedShirt.PopSocketsGripPath = imagePath;
                        shirtCreatorVM.FrontImagePath = imagePath;
                        shirtCreatorVM.BackImagePath = imagePath;
                    }
                }
            }
        }
        internal void OnExit()
        {
            ////Save Setting
            //Properties.Settings.Default.aaa = shirtVM.ToString();
            //Properties.Settings.Default.Save();
            ////Close
        }

        private void SaveCmdInvoke(object input)
        {
            try
            {
                ShirtStatus errorCode = 0;
                if (input != null)
                {
                    ShirtCreatorViewModel shirtCreatorVM = input as ShirtCreatorViewModel;
                    if (shirtCreatorVM != null)
                    {
                        if (shirtCreatorVM.SelectedShirt != null && ValidateShirt(shirtCreatorVM.SelectedShirt, ref errorCode))
                        {
                            XMLDataAccess dataAccess = new XMLDataAccess();
                            dataAccess.SaveShirt(shirtCreatorVM.SelectedShirt);
                            System.Windows.MessageBox.Show(string.Format("Shirt saved in {0}", dataAccess.XmlFilePath), "Save Shirt", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            Utils.ShowWarningMessageBox(GetErrorMessage(errorCode));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utils.ShowErrorMessageBox(ex.Message);
            }
        }

        public static string GetErrorMessage(ShirtStatus errorCode)
        {
            string result = "Shirt is invalid!\n\n";
            switch (errorCode)
            {
                case ShirtStatus.ImageDimensionFail:
                    result += "Invalid image, please check dimensions!";
                    break;
                case ShirtStatus.EmptyPath:
                    result += "No image selected!";
                    break;
                case ShirtStatus.BrandNameFail:
                    result += "Brand must be 3-50 characters, please check again";
                    break;
                case ShirtStatus.TitleFail:
                    result += "Design Title must be 3-60 characters, please check again";
                    break;
                case ShirtStatus.FeatureBulletFail:
                    result += "Feature Bullet must be 256 characters or fewer, please check again";
                    break;
                case ShirtStatus.DescriptionFail:
                    result += "Description must be 75-2000 characters, please check again";
                    break;
                case ShirtStatus.ColorFail:
                    result += "Number of color selected must be from 0 to 10, please check again";
                    break;
            }
            return result;
        }

        private bool ValidateImage(string path, double width, double height)
        {
            try
            {
                if (!string.IsNullOrEmpty(path))
                {
                    Image a = Image.FromFile(path);
                    if (a.Width == width && a.Height == height)
                    {
                        return true;
                    }
                    else
                    {
                        MessageBoxResult result = System.Windows.MessageBox.Show("Wrong Image Dimension! \nDo you want to resize this image?",
                            "Error opening image", MessageBoxButton.YesNoCancel, MessageBoxImage.Error);
                        if (result == MessageBoxResult.Yes ||
                            result == MessageBoxResult.OK)
                        {
                            ImageEditCmdInvoke(path);
                        }
                        return false;
                    }
                }
                else return false;
            }
            catch
            {
                Utils.ShowErrorMessageBox("Failed to open selected image!");
                return false;
            }
        }

        public static bool ValidateShirt(Shirt shirt, ref ShirtStatus shirtError)
        {
            try
            {
                if (shirt == null)
                    return false;

                if (shirt.ShirtTypes == null)
                {
                    shirtError = ShirtStatus.NoShirtType;
                    return false;
                }
                else
                if (shirt.ShirtTypes.Where(x => x.IsActive).Count() == 0)
                {
                    shirtError = ShirtStatus.NoShirtType;
                    return false;
                }

                if (string.IsNullOrEmpty(shirt.FrontStdPath) &&
                    string.IsNullOrEmpty(shirt.BackStdPath) &&
                    string.IsNullOrEmpty(shirt.FrontHoodiePath) &&
                    string.IsNullOrEmpty(shirt.BackHoodiePath) &&
                    string.IsNullOrEmpty(shirt.PopSocketsGripPath))
                {
                    shirtError = ShirtStatus.EmptyPath;
                    return false;
                }
                //BrandName
                bool brandNameEnglish = true;
                bool brandNameGerman = true;
                if (string.IsNullOrEmpty(shirt.BrandName))
                    brandNameEnglish = false;
                else
                if ((shirt.BrandName.Length < 3 ||
                     shirt.BrandName.Length > 50))
                {
                    shirtError = ShirtStatus.BrandNameFail;
                    return false;
                }
                if (string.IsNullOrEmpty(shirt.BrandNameGerman))
                {
                    brandNameGerman = false;
                }
                else
                if ((shirt.BrandNameGerman.Length < 3 ||
                        shirt.BrandNameGerman.Length > 50))
                {
                    shirtError = ShirtStatus.BrandNameFail;
                    return false;
                }
                if (!brandNameEnglish && !brandNameGerman)
                {
                    shirtError = ShirtStatus.BrandNameFail;
                    return false;
                }
                //Design Title
                bool titleEnglish = true;
                bool titleGerman = true;
                if (string.IsNullOrEmpty(shirt.DesignTitle))
                    titleEnglish = false;
                else
                if ((shirt.DesignTitle.Length < 3 ||
                     shirt.DesignTitle.Length > 60))
                {
                    shirtError = ShirtStatus.TitleFail;
                    return false;
                }
                if (string.IsNullOrEmpty(shirt.BrandNameGerman))
                {
                    titleGerman = false;
                }
                else
                if ((shirt.DesignTitleGerman.Length < 3 ||
                        shirt.DesignTitleGerman.Length > 60))
                {
                    shirtError = ShirtStatus.TitleFail;
                    return false;
                }
                if (!titleEnglish && !titleGerman)
                {
                    shirtError = ShirtStatus.TitleFail;
                    return false;
                }
                //Feature Bullet
                if (shirt.FeatureBullet1.Length > 256 ||
                    shirt.FeatureBullet2.Length > 256 ||
                    shirt.FeatureBullet1German.Length > 256 ||
                    shirt.FeatureBullet2German.Length > 256)
                {
                    shirtError = ShirtStatus.FeatureBulletFail;
                    return false;
                }
                //Description
                if (!string.IsNullOrEmpty(shirt.Description) &&
                    (shirt.Description.Length > 2000 || shirt.Description.Length < 75) ||
                    !string.IsNullOrEmpty(shirt.DescriptionGerman) &&
                    (shirt.DescriptionGerman.Length > 2000 || shirt.DescriptionGerman.Length < 75))
                {
                    shirtError = ShirtStatus.DescriptionFail;
                    return false;
                }
                //Color
                foreach (ShirtBase s in shirt.ShirtTypes.Where(x => x.IsActive == true))
                {
                    if (s.Colors == null)
                    {
                        shirtError = ShirtStatus.ColorFail;
                        return false;
                    }
                    if (s.Colors.Where(x => x.IsActive).Count() > 10 || s.Colors.Where(x => x.IsActive).Count() == 0)
                    {
                        shirtError = ShirtStatus.ColorFail;
                        return false;
                    }
                }
                //FitType
                return true;
            }
            catch
            {
                shirtError = ShirtStatus.Fail;
                return false;
            }
        }
        private void ChangeColorCmdInvoke(object obj)
        {
            try
            {
                ToggleButton button = obj as ToggleButton;
                if (button != null)
                {
                    string colorName = button.ToolTip.ToString();
                    if (button.IsChecked == true)
                    {

                        shirtVM.FrontMockup = ShirtCreatorViewModel.RootFolderPath + shirtVM.SelectedShirtType.TypeName + "/" + colorName.ToUpper() + ".png";
                        shirtVM.BackMockup = ShirtCreatorViewModel.RootFolderPath + shirtVM.SelectedShirtType.TypeName + "Back" + "/" + colorName.ToUpper() + ".png";

                    }
                    else if (button.IsChecked == false)
                    {
                        Upload.Definitions.Color activeColor = shirtVM.SelectedShirtType.Colors.FirstOrDefault(x => x.IsActive == true);
                        shirtVM.FrontMockup = ShirtCreatorViewModel.RootFolderPath + shirtVM.SelectedShirtType.TypeName + "/" + activeColor.ColorName + ".png";
                        shirtVM.BackMockup = ShirtCreatorViewModel.RootFolderPath + shirtVM.SelectedShirtType.TypeName + "Back" + "/" + activeColor.ColorName + ".png";
                    }
                }
                shirtVM.CountColor = shirtVM.SelectedShirtType.Colors.Where(x => x.IsActive == true).ToArray().Length;
            }
            catch
            {
                shirtVM.FrontMockup = ShirtCreatorViewModel.RootFolderPath + "StandardTShirt/Asphalt.png";
                shirtVM.BackMockup = ShirtCreatorViewModel.RootFolderPath + "StandardTShirtBack/Asphalt.png";
            }
        }
        private void MouseEnterCmdInvoke(object obj)
        {
            try
            {
                ToggleButton button = obj as ToggleButton;
                if (button != null)
                {
                    string colorName = button.ToolTip.ToString();
                    shirtVM.FrontMockup = ShirtCreatorViewModel.RootFolderPath + shirtVM.SelectedShirtType.TypeName + "/" + colorName.ToUpper() + ".png";
                    shirtVM.BackMockup = ShirtCreatorViewModel.RootFolderPath + shirtVM.SelectedShirtType.TypeName + "Back" + "/" + colorName.ToUpper() + ".png";

                }
            }
            catch
            {
                shirtVM.FrontMockup = ShirtCreatorViewModel.RootFolderPath + "StandardTShirt/Asphalt.png";
                shirtVM.BackMockup = ShirtCreatorViewModel.RootFolderPath + "StandardTShirtBack/Asphalt.png";
            }
        }
    }
}