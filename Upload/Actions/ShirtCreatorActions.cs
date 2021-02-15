using Upload.GUI;
using Upload.ViewModel;
using Common.MVVMCore;
using System.Linq;
using Upload.DataAccess;
using System;
using System.Drawing;
using System.IO;
using System.IO.Pipes;
using System.Windows.Controls.Primitives;
using System.Windows;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Office.Interop.Excel;
using Upload.DataAccess.Model;
using System.Text.RegularExpressions;
using Upload.DataAccess.DTO;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace Upload.Actions
{
    public class ShirtCreatorActions
    {
        public void ShowWindow(string jsonStringPath)
        {
            try
            {
                string jsonString = File.ReadAllText(jsonStringPath);
                File.Delete(jsonStringPath);
                ShirtData sData = JsonConvert.DeserializeObject<ShirtData>(jsonString);
                Shirt s = ShirtDTO.MapData(sData, typeof(Shirt)) as Shirt;
                ShirtCreatorView shirtCreatorWindow = new ShirtCreatorView();
                ShirtCreatorViewModel shirtVM = CreateShirtViewModel(s);
                shirtVM.ExcelMode = true;
                shirtCreatorWindow.DataContext = shirtVM;
                shirtCreatorWindow.Show();

            }
            catch
            {
                Utils.ShowErrorMessageBox("Error opening file");
            }
        }
        public void ShowWindow(Shirt editShirt = null)
        {
            ShirtCreatorView shirtCreatorWindow = new ShirtCreatorView();
            ShirtCreatorViewModel shirtVM = CreateShirtViewModel(editShirt);
            shirtCreatorWindow.DataContext = shirtVM;
            shirtCreatorWindow.Show();
        }

        public ShirtCreatorViewModel CreateShirtViewModel(Shirt editShirt = null)
        {
            ShirtCreatorViewModel shirtVM = new ShirtCreatorViewModel();
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
            shirtVM.DeleteCmd = new RelayCommand(DeleteCmdInvoke);
            shirtVM.SaveAllCmd = new RelayCommand(SaveAllCmdInvoke);
            shirtVM.MultiReplaceCmd = new RelayCommand(ExportToExcel);
            shirtVM.RemoveShirtCmd = new RelayCommand(RemoveShirtCmdInvoke);
            shirtVM.ImportFromExcelCmd = new RelayCommand(ImportFromExcel);
            shirtVM.CopyShirtCmd = new RelayCommand(CopyShirtCmdInvoke);
            if (editShirt != null)
            {
                shirtVM.SelectedShirt = editShirt;
                shirtVM.CreateMode = false;
                if (shirtVM.SelectedShirt.ShirtTypes != null)
                    shirtVM.SelectedShirtType = shirtVM.SelectedShirt.ShirtTypes.FirstOrDefault(x => x.IsActive == true);
                if (editShirt == null)
                {
                    shirtVM.LightColor = true;
                }
            }
            return shirtVM;
        }

        private void CopyShirtCmdInvoke(object obj)
        {
            object[] objParams = obj as object[];
            ShirtCreatorViewModel shirtVM = objParams[0] as ShirtCreatorViewModel;
            System.Windows.Window window = objParams[1] as System.Windows.Window;
            try
            {
                if (shirtVM != null && window != null)
                {
                    System.Windows.Controls.ListView listView = window.FindName("listShirts") as System.Windows.Controls.ListView;
                    Shirt currentShirt = shirtVM.SelectedShirt.Clone() as Shirt;
                    ObservableCollection<ShirtType> shirtTypes = currentShirt.ShirtTypes;
                    for (int i = 1; i < listView.SelectedItems.Count; i++)
                    {
                        (listView.SelectedItems[i] as Shirt).ShirtTypes = shirtTypes;
                    }
                    ShowPopup(shirtVM, "Shirt Style Copied!");
                }
            }
            catch
            {
                ShowPopup(shirtVM, "Copy Failed!");
            }
        }

        private void RemoveShirtCmdInvoke(object obj)
        {
            object[] objParams = obj as object[];
            ShirtCreatorViewModel shirtVM = objParams[0] as ShirtCreatorViewModel;
            Shirt s = objParams[1] as Shirt;
            if (shirtVM != null && s != null && shirtVM.Shirts != null && shirtVM.Shirts.Count > 1 && shirtVM.Shirts.Contains(s))
            {
                shirtVM.Shirts.Remove(s);
                shirtVM.SelectedShirt = shirtVM.Shirts.Last();
            }
        }

        private void SaveAllCmdInvoke(object obj)
        {
            try
            {
                if (obj is ShirtCreatorViewModel shirtVM)
                {
                    Dictionary<int, ShirtStatus> dictError = new Dictionary<int, ShirtStatus>();
                    for (int i=0;i<shirtVM.Shirts.Count;i++)
                    {
                        Shirt shirt = shirtVM.Shirts[i];
                        ShirtStatus errorCode = 0;
                        if (!string.IsNullOrEmpty(shirt.ImagePath))
                        {
                            if (ValidateShirt(shirt, ref errorCode))
                            {
                                JsonDataAccess dataAccess = new JsonDataAccess();
                                ShirtData sData = ShirtDTO.MapData(shirt, typeof(ShirtData)) as ShirtData;
                                dataAccess.SaveShirt(sData);
                            }
                            else
                            {
                                dictError.Add(i, errorCode);
                            }
                        }
                    }
                    if (dictError.Count > 0)
                    {
                        string errorMessage = "Following shirt(s) are invalid:\n";
                        foreach (var error in dictError)
                        {
                            errorMessage += error.Key + ": " + GetErrorMessage(error.Value) + "\n";
                        }
                        Utils.ShowErrorMessageBox(errorMessage);
                    }
                    else
                    {
                        ShowPopup(shirtVM, "Shirts saved!");
                    }
                }
            }
            catch (Exception ex)
            {
                Utils.ShowErrorMessageBox(ex.Message);
            }

        }
        public void ShowPopup(ShirtCreatorViewModel viewModel, string message)
        {

            if (viewModel != null)
            {
                viewModel.PopupText = message;
                viewModel.IsOpenPopup = true;
                Wait(5000);
                viewModel.IsOpenPopup = false;
            }
        }
        private void Wait(int milliseconds)
        {
            System.Windows.Forms.Timer timer1 = new System.Windows.Forms.Timer();
            if (milliseconds == 0 || milliseconds < 0) return;
            //Console.WriteLine("start wait timer");
            timer1.Interval = milliseconds;
            timer1.Enabled = true;
            timer1.Start();
            timer1.Tick += (s, e) =>
            {
                timer1.Enabled = false;
                timer1.Stop();
                //Console.WriteLine("stop wait timer");
            };
            while (timer1.Enabled)
            {
                System.Windows.Forms.Application.DoEvents();
            }
        }
        private void DeleteCmdInvoke(object obj)
        {
            if (obj is ShirtCreatorViewModel shirtVM)
            {
                if (shirtVM.Shirts != null)
                {
                    while (shirtVM.SelectedShirt != null && shirtVM.Shirts.Count > 1)
                        shirtVM.Shirts.Remove(shirtVM.SelectedShirt);
                }
                shirtVM.SelectedShirt = shirtVM.Shirts.Last();
            }
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
                                JsonDataAccess dataAccess = new JsonDataAccess();
                                var sData = ShirtDTO.MapData(shirtVM.SelectedShirt, typeof(ShirtData)) as ShirtData;
                                dataAccess.SaveShirt(sData);
                                System.Windows.MessageBox.Show("Shirt saved!", "Save Shirt", MessageBoxButton.OK, MessageBoxImage.Information);
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
            shirtCreatorVM.SelectedShirt.ImagePath = string.Empty;
            shirtCreatorVM.FrontImagePath = string.Empty;
            shirtCreatorVM.BackImagePath = string.Empty;

            //if (shirtCreatorVM.SelectedShirtType is LongSleeveTShirt ||
            //        shirtCreatorVM.SelectedShirtType is PremiumTShirt ||
            //        shirtCreatorVM.SelectedShirtType is Raglan ||
            //        shirtCreatorVM.SelectedShirtType is StandardTShirt ||
            //        shirtCreatorVM.SelectedShirtType is SweetShirt ||
            //        shirtCreatorVM.SelectedShirtType is TankTop ||
            //        shirtCreatorVM.SelectedShirtType is VNeckTShirt)
            //{
            //    shirtCreatorVM.SelectedShirt.FrontStdPath = string.Empty;
            //    shirtCreatorVM.FrontImagePath = string.Empty;
            //}
            //else
            //if (shirtCreatorVM.SelectedShirtType is PullOverHoodie ||
            //            shirtCreatorVM.SelectedShirtType is ZipHoodie)
            //{
            //    shirtCreatorVM.SelectedShirt.FrontHoodiePath = string.Empty;
            //    shirtCreatorVM.FrontImagePath = string.Empty;
            //}
            //else if (shirtCreatorVM.SelectedShirtType is PopSocketsGrip)
            //{
            //    shirtCreatorVM.SelectedShirt.PopSocketsGripPath = string.Empty;
            //    shirtCreatorVM.FrontImagePath = string.Empty;
            //    shirtCreatorVM.BackImagePath = string.Empty;
            //}
        }

        private void RemoveBackImageCmdInvoke(object obj)
        {
            ShirtCreatorViewModel shirtCreatorVM = obj as ShirtCreatorViewModel;
            shirtCreatorVM.SelectedShirt.ImagePath = string.Empty;
            shirtCreatorVM.FrontImagePath = string.Empty;
            shirtCreatorVM.BackImagePath = string.Empty;
            //ShirtCreatorViewModel shirtCreatorVM = obj as ShirtCreatorViewModel;
            //if (shirtCreatorVM.SelectedShirtType is LongSleeveTShirt ||
            //        shirtCreatorVM.SelectedShirtType is PremiumTShirt ||
            //        shirtCreatorVM.SelectedShirtType is Raglan ||
            //        shirtCreatorVM.SelectedShirtType is StandardTShirt ||
            //        shirtCreatorVM.SelectedShirtType is SweetShirt ||
            //        shirtCreatorVM.SelectedShirtType is TankTop ||
            //        shirtCreatorVM.SelectedShirtType is VNeckTShirt)
            //{
            //    shirtCreatorVM.SelectedShirt.BackStdPath = string.Empty;
            //    shirtCreatorVM.BackImagePath = string.Empty;
            //}
            //else
            //if (shirtCreatorVM.SelectedShirtType is PullOverHoodie ||
            //            shirtCreatorVM.SelectedShirtType is ZipHoodie)
            //{
            //    shirtCreatorVM.SelectedShirt.BackHoodiePath = string.Empty;
            //    shirtCreatorVM.BackImagePath = string.Empty;
            //}
            //else if (shirtCreatorVM.SelectedShirtType is PopSocketsGrip)
            //{
            //    shirtCreatorVM.SelectedShirt.PopSocketsGripPath = string.Empty;
            //    shirtCreatorVM.FrontImagePath = string.Empty;
            //    shirtCreatorVM.BackImagePath = string.Empty;
            //}
        }

        private void OpenCmdInvoke(object obj)
        {
            if (obj is ShirtCreatorViewModel mainVM)
            {
                if (mainVM.CreateMode == true)
                {
                    List<Shirt> lstShirts = Utils.BrowseForShirts();
                    if (lstShirts != null && lstShirts.Count > 0)
                    {
                        if (lstShirts.Count == 1)
                        {
                            mainVM.SelectedShirt = lstShirts[0];
                        }
                        else
                        {
                            mainVM.MultiMode = true;
                            lstShirts.ForEach(x => mainVM.Shirts.Add(x));
                            if (mainVM.SelectedShirt == null && mainVM.Shirts != null && mainVM.Shirts.Count > 0)
                            {
                                mainVM.SelectedShirt = mainVM.Shirts.FirstOrDefault();
                            }
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
                if (shirtCreatorVM.MultiMode && !string.IsNullOrEmpty(shirtCreatorVM.BackImagePath))
                {
                    ShowPopup(shirtCreatorVM, "Multi-Mode only support 1 image per shirt");
                    return;
                }
                string[] browseResult = Utils.BrowseForFilePath(multiselect: shirtCreatorVM.MultiMode);
                if (shirtCreatorVM.MultiMode == false)
                {
                    string imagePath = browseResult[0];

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
                            shirtCreatorVM.SelectedShirt.ImagePath = imagePath;
                            shirtCreatorVM.SelectedShirt.ImageType = (int)PNGImageType.Standard_Front;
                            shirtCreatorVM.FrontImagePath = imagePath;

                        }

                    }
                    else
                    if (shirtCreatorVM.SelectedShirtType is PullOverHoodie ||
                            shirtCreatorVM.SelectedShirtType is ZipHoodie)
                    {
                        if (ValidateImage(imagePath, 4500, 4050))
                        {
                            shirtCreatorVM.SelectedShirt.ImagePath = imagePath;
                            shirtCreatorVM.FrontImagePath = imagePath;
                            shirtCreatorVM.SelectedShirt.ImageType = (int)PNGImageType.Hoodie_Front;
                        }
                    }
                    else if (shirtCreatorVM.SelectedShirtType is PopSocketsGrip)
                    {
                        if (ValidateImage(imagePath, 485, 485))
                        {
                            shirtCreatorVM.SelectedShirt.ImagePath = imagePath;
                            shirtCreatorVM.FrontImagePath = imagePath;
                            shirtCreatorVM.SelectedShirt.ImageType = (int)PNGImageType.Popsockets;
                        }
                    }
                    else if (shirtCreatorVM.SelectedShirtType is IPhoneCase)
                    {
                        if (ValidateImage(imagePath, 1800, 3200))
                        {
                            shirtCreatorVM.SelectedShirt.ImagePath = imagePath;
                            shirtCreatorVM.FrontImagePath = imagePath;
                            shirtCreatorVM.SelectedShirt.ImageType = (int)PNGImageType.iPhoneCase;
                        }
                    }
                    else if (shirtCreatorVM.SelectedShirtType is SamsungCase)
                    {
                        if (ValidateImage(imagePath, 1800, 3200))
                        {
                            shirtCreatorVM.SelectedShirt.ImagePath = imagePath;
                            shirtCreatorVM.FrontImagePath = imagePath;
                            shirtCreatorVM.SelectedShirt.ImageType = (int)PNGImageType.SamsungCase;
                        }
                    }

                }
                else
                {
                    //if (shirtCreatorVM.Shirts == null)
                    //    shirtCreatorVM.Shirts = new System.Collections.ObjectModel.ObservableCollection<Shirt>();

                    //foreach (string imagePath in browseResult)
                    //{
                    //    Shirt shirt = shirtCreatorVM.SelectedShirt.Clone() as Shirt;

                    //    if (string.IsNullOrEmpty(shirt.DesignTitle))
                    //        shirt.DesignTitle = Path.GetFileNameWithoutExtension(imagePath);

                    //    if (shirtCreatorVM.SelectedShirtType is LongSleeveTShirt ||
                    //         shirtCreatorVM.SelectedShirtType is PremiumTShirt ||
                    //         shirtCreatorVM.SelectedShirtType is Raglan ||
                    //         shirtCreatorVM.SelectedShirtType is StandardTShirt ||
                    //         shirtCreatorVM.SelectedShirtType is SweetShirt ||
                    //         shirtCreatorVM.SelectedShirtType is TankTop ||
                    //         shirtCreatorVM.SelectedShirtType is VNeckTShirt)
                    //    {
                    //        if (/*ValidateImage(imagePath, 4500, 5400)*/true)
                    //        {
                    //            shirt.FrontStdPath = imagePath;
                    //            shirtCreatorVM.FrontImagePath = shirtCreatorVM.SelectedShirt.FrontStdPath;
                    //        }
                    //    }
                    //    else
                    //    if (shirtCreatorVM.SelectedShirtType is PullOverHoodie ||
                    //            shirtCreatorVM.SelectedShirtType is ZipHoodie)
                    //    {
                    //        if (/*ValidateImage(imagePath, 4500, 4050)*/true)
                    //        {
                    //            shirt.FrontHoodiePath = imagePath;
                    //            shirtCreatorVM.FrontImagePath = shirtCreatorVM.SelectedShirt.FrontHoodiePath;
                    //        }
                    //    }
                    //    else if (shirtCreatorVM.SelectedShirtType is PopSocketsGrip)
                    //    {
                    //        if (/*ValidateImage(imagePath, 485, 485)*/true)
                    //        {
                    //            shirt.PopSocketsGripPath = imagePath;
                    //            shirtCreatorVM.FrontImagePath = imagePath;
                    //            shirtCreatorVM.BackImagePath = imagePath;
                    //        }
                    //    }
                    //    shirtCreatorVM.Shirts.Add(shirt);
                    //}
                }
            }
        }
        private void ClickBackImageCmdInvoke(object obj)
        {
            ShirtCreatorViewModel shirtCreatorVM = obj as ShirtCreatorViewModel;
            if (shirtCreatorVM != null)
            {
                if (shirtCreatorVM.MultiMode && !string.IsNullOrEmpty(shirtCreatorVM.BackImagePath))
                {
                    ShowPopup(shirtCreatorVM, "Multi-Mode only support 1 image per shirt");
                    return;
                }
                string[] browseResult = Utils.BrowseForFilePath(multiselect: shirtCreatorVM.MultiMode);
                if (shirtCreatorVM.MultiMode == false)
                {
                    string imagePath = browseResult[0];
                    if (string.IsNullOrEmpty(imagePath))
                        shirtCreatorVM.SelectedShirt.ImagePath = Path.GetFileNameWithoutExtension(imagePath);

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
                            shirtCreatorVM.SelectedShirt.ImagePath = imagePath;
                            shirtCreatorVM.BackImagePath = imagePath;
                            shirtCreatorVM.SelectedShirt.ImageType = (int)PNGImageType.Standard_Back;

                        }
                    }
                    else
                    if (shirtCreatorVM.SelectedShirtType is PullOverHoodie ||
                            shirtCreatorVM.SelectedShirtType is ZipHoodie)
                    {
                        if (ValidateImage(imagePath, 4500, 4050))
                        {
                            shirtCreatorVM.SelectedShirt.ImagePath = imagePath;
                            shirtCreatorVM.BackImagePath = imagePath;
                            shirtCreatorVM.SelectedShirt.ImageType = (int)PNGImageType.Hoodie_Back;

                        }
                    }
                    else if (shirtCreatorVM.SelectedShirtType is PopSocketsGrip)
                    {
                        if (ValidateImage(imagePath, 485, 485))
                        {
                            shirtCreatorVM.SelectedShirt.ImagePath = imagePath;
                            shirtCreatorVM.FrontImagePath = imagePath;
                            shirtCreatorVM.SelectedShirt.ImageType = (int)PNGImageType.Popsockets;
                        }
                    }
                    else if (shirtCreatorVM.SelectedShirtType is IPhoneCase)
                    {
                        if (ValidateImage(imagePath, 1800, 3200))
                        {
                            shirtCreatorVM.SelectedShirt.ImagePath = imagePath;
                            shirtCreatorVM.FrontImagePath = imagePath;
                            shirtCreatorVM.SelectedShirt.ImageType = (int)PNGImageType.iPhoneCase;
                        }
                    }
                    else if (shirtCreatorVM.SelectedShirtType is SamsungCase)
                    {
                        if (ValidateImage(imagePath, 1800, 3200))
                        {
                            shirtCreatorVM.SelectedShirt.ImagePath = imagePath;
                            shirtCreatorVM.FrontImagePath = imagePath;
                            shirtCreatorVM.SelectedShirt.ImageType = (int)PNGImageType.SamsungCase;
                        }
                    }
                }
                else
                {
                    //if (shirtCreatorVM.Shirts == null)
                    //    shirtCreatorVM.Shirts = new System.Collections.ObjectModel.ObservableCollection<Shirt>();
                    //foreach (string imagePath in browseResult)
                    //{
                    //    Shirt shirt = shirtCreatorVM.SelectedShirt.Clone() as Shirt;

                    //    if (string.IsNullOrEmpty(shirt.DesignTitle))
                    //        shirt.DesignTitle = Path.GetFileNameWithoutExtension(imagePath);

                    //    if (shirtCreatorVM.SelectedShirtType is LongSleeveTShirt ||
                    //         shirtCreatorVM.SelectedShirtType is PremiumTShirt ||
                    //         shirtCreatorVM.SelectedShirtType is Raglan ||
                    //         shirtCreatorVM.SelectedShirtType is StandardTShirt ||
                    //         shirtCreatorVM.SelectedShirtType is SweetShirt ||
                    //         shirtCreatorVM.SelectedShirtType is TankTop ||
                    //         shirtCreatorVM.SelectedShirtType is VNeckTShirt)
                    //    {
                    //        if (ValidateImage(imagePath, 4500, 5400))
                    //        {
                    //            shirt.BackStdPath = imagePath;
                    //            shirtCreatorVM.BackImagePath = shirtCreatorVM.SelectedShirt.BackStdPath;
                    //        }
                    //    }
                    //    else
                    //    if (shirtCreatorVM.SelectedShirtType is PullOverHoodie ||
                    //            shirtCreatorVM.SelectedShirtType is ZipHoodie)
                    //    {
                    //        if (ValidateImage(imagePath, 4500, 4050))
                    //        {
                    //            shirt.BackHoodiePath = imagePath;
                    //            shirtCreatorVM.BackImagePath = shirtCreatorVM.SelectedShirt.BackHoodiePath;
                    //        }
                    //    }
                    //    else if (shirtCreatorVM.SelectedShirtType is PopSocketsGrip)
                    //    {
                    //        if (ValidateImage(imagePath, 485, 485))
                    //        {
                    //            shirt.PopSocketsGripPath = imagePath;
                    //            shirtCreatorVM.BackImagePath = imagePath;
                    //            shirtCreatorVM.BackImagePath = imagePath;
                    //        }
                    //    }
                    //    shirtCreatorVM.Shirts.Add(shirt);
                    //    shirt = null;
                    //}
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
                        if (shirtCreatorVM.ExcelMode == true)
                        {
                            ShirtData sData = ShirtDTO.MapData(shirtCreatorVM.SelectedShirt, typeof(ShirtData)) as ShirtData;
                            string strJson = JsonConvert.SerializeObject(sData);
                            using (var clientPipe = new NamedPipeClientStream("MerchUploaderPipe"))
                            {
                                clientPipe.Connect();
                                using (StreamWriter sWriter = new StreamWriter(clientPipe))
                                {
                                    sWriter.Write(strJson);
                                }
                            }
                            Environment.Exit(0);
                        }
                        else
                        if (shirtCreatorVM.SelectedShirt != null && ValidateShirt(shirtCreatorVM.SelectedShirt, ref errorCode))
                        {
                            JsonDataAccess dataAccess = new JsonDataAccess();
                            ShirtData sData = ShirtDTO.MapData(shirtCreatorVM.SelectedShirt, typeof(ShirtData)) as ShirtData;
                            dataAccess.SaveShirt(sData);
                            ShowPopup(shirtCreatorVM, "Shirt saved!");
                        }
                        else
                        {
                            ShowPopup(shirtCreatorVM, GetErrorMessage(errorCode));
                            //Utils.ShowWarningMessageBox(GetErrorMessage(errorCode));
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
            string result = string.Empty;
            switch (errorCode)
            {
                case ShirtStatus.ImageDimensionFail:
                    result += "Invalid image, please check dimensions!";
                    break;
                case ShirtStatus.EmptyPath:
                    result += "No image selected!";
                    break;
                case ShirtStatus.BrandNameFail:
                    result += "Brand must be 3-50 characters";
                    break;
                case ShirtStatus.TitleFail:
                    result += "Design Title must be 3-60 characters";
                    break;
                case ShirtStatus.FeatureBulletFail:
                    result += "Feature Bullet must be 256 characters or fewer";
                    break;
                case ShirtStatus.DescriptionFail:
                    result += "Description must be 75-2000 characters";
                    break;
                case ShirtStatus.ColorFail:
                    result += "Please select at least 1 or at most 10 colors ";
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
                        MessageBoxResult result = System.Windows.MessageBox.Show($"The image must be {width}x{height}px\nDo you want to resize this image?",
                            "Error opening image", MessageBoxButton.YesNoCancel, MessageBoxImage.Error);
                        if (result == MessageBoxResult.Yes ||
                            result == MessageBoxResult.OK)
                        {
                            ImageEditCmdInvoke(path);
                        }
                        System.Windows.MessageBox.Show("Wrong Image Dimension!");
                        //MessageBoxResult result = System.Windows.MessageBox.Show("Wrong Image Dimension! \nDo you want to resize this image?",
                        //    "Error opening image", MessageBoxButton.YesNoCancel, MessageBoxImage.Error);
                        //if (result == MessageBoxResult.Yes ||
                        //    result == MessageBoxResult.OK)
                        //{
                        //    ImageEditCmdInvoke(path);
                        //}
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

                if (string.IsNullOrEmpty(shirt.ImagePath))
                {
                    shirtError = ShirtStatus.EmptyPath;
                    return false;
                }
                ////BrandName
                //bool brandNameEnglish = true;
                //bool brandNameGerman = true;
                //if (string.IsNullOrEmpty(shirt.BrandName))
                //    brandNameEnglish = false;
                //else
                //if ((shirt.BrandName.Length < 3 ||
                //     shirt.BrandName.Length > 50))
                //{
                //    shirtError = ShirtStatus.BrandNameFail;
                //    return false;
                //}
                //if (string.IsNullOrEmpty(shirt.BrandNameGerman))
                //{
                //    brandNameGerman = false;
                //}
                //else
                //if ((shirt.BrandNameGerman.Length < 3 ||
                //        shirt.BrandNameGerman.Length > 50))
                //{
                //    shirtError = ShirtStatus.BrandNameFail;
                //    return false;
                //}
                //if (!brandNameEnglish && !brandNameGerman)
                //{
                //    shirtError = ShirtStatus.BrandNameFail;
                //    return false;
                //}
                ////Design Title
                //bool titleEnglish = true;
                //bool titleGerman = true;
                //if (string.IsNullOrEmpty(shirt.DesignTitle))
                //    titleEnglish = false;
                //else
                //if ((shirt.DesignTitle.Length < 3 ||
                //     shirt.DesignTitle.Length > 60))
                //{
                //    shirtError = ShirtStatus.TitleFail;
                //    return false;
                //}
                //if (string.IsNullOrEmpty(shirt.BrandNameGerman))
                //{
                //    titleGerman = false;
                //}
                //else
                //if ((shirt.DesignTitleGerman.Length < 3 ||
                //        shirt.DesignTitleGerman.Length > 60))
                //{
                //    shirtError = ShirtStatus.TitleFail;
                //    return false;
                //}
                //if (!titleEnglish && !titleGerman)
                //{
                //    shirtError = ShirtStatus.TitleFail;
                //    return false;
                //}
                ////Feature Bullet
                //if (shirt.FeatureBullet1.Length > 256 ||
                //    shirt.FeatureBullet2.Length > 256 ||
                //    shirt.FeatureBullet1German.Length > 256 ||
                //    shirt.FeatureBullet2German.Length > 256)
                //{
                //    shirtError = ShirtStatus.FeatureBulletFail;
                //    return false;
                //}
                ////Description
                //if (!string.IsNullOrEmpty(shirt.Description) &&
                //    (shirt.Description.Length > 2000 || shirt.Description.Length < 75) ||
                //    !string.IsNullOrEmpty(shirt.DescriptionGerman) &&
                //    (shirt.DescriptionGerman.Length > 2000 || shirt.DescriptionGerman.Length < 75))
                //{
                //    shirtError = ShirtStatus.DescriptionFail;
                //    return false;
                //}
                //Color
                foreach (ShirtType s in shirt.ShirtTypes.Where(x => x.IsActive == true))
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
                object[] objParams = obj as object[];
                ShirtCreatorViewModel shirtVM = objParams[0] as ShirtCreatorViewModel;
                ToggleButton button = objParams[1] as ToggleButton;

                if (shirtVM != null && button != null)
                {
                    shirtVM.FrontMockup = ShirtCreatorViewModel.RootFolderPath + "StandardTShirt/Asphalt.png";
                    shirtVM.BackMockup = ShirtCreatorViewModel.RootFolderPath + "StandardTShirtBack/Asphalt.png";

                    string colorName = button.ToolTip.ToString();
                    if (button.IsChecked == true)
                    {

                        shirtVM.FrontMockup = ShirtCreatorViewModel.RootFolderPath + shirtVM.SelectedShirtType.TypeName + "/" + colorName.ToUpper() + ".png";
                        shirtVM.BackMockup = ShirtCreatorViewModel.RootFolderPath + shirtVM.SelectedShirtType.TypeName + "Back" + "/" + colorName.ToUpper() + ".png";

                    }
                    else if (button.IsChecked == false)
                    {
                        ViewModel.Color activeColor = shirtVM.SelectedShirtType.Colors.FirstOrDefault(x => x.IsActive == true);
                        if (activeColor != null)
                        {
                            shirtVM.FrontMockup = ShirtCreatorViewModel.RootFolderPath + shirtVM.SelectedShirtType.TypeName + "/" + activeColor.ColorName + ".png";
                            shirtVM.BackMockup = ShirtCreatorViewModel.RootFolderPath + shirtVM.SelectedShirtType.TypeName + "Back" + "/" + activeColor.ColorName + ".png";
                        }
                    }
                }
                shirtVM.CountColor = shirtVM.SelectedShirtType.Colors.Where(x => x.IsActive == true).ToArray().Length;
            }
            catch
            {
            }
        }
        private void MouseEnterCmdInvoke(object obj)
        {
            try
            {
                object[] objParams = obj as object[];
                ShirtCreatorViewModel shirtVM = objParams[0] as ShirtCreatorViewModel;
                ToggleButton button = objParams[1] as ToggleButton;

                if (shirtVM != null && button != null)
                {
                    string colorName = button.ToolTip.ToString();
                    shirtVM.FrontMockup = ShirtCreatorViewModel.RootFolderPath + shirtVM.SelectedShirtType.TypeName + "/" + colorName.ToUpper() + ".png";
                    shirtVM.BackMockup = ShirtCreatorViewModel.RootFolderPath + shirtVM.SelectedShirtType.TypeName + "Back" + "/" + colorName.ToUpper() + ".png";

                }
            }
            catch
            {
            }
        }
        #region Batch Replace
        private void ExportToExcel(object obj)
        {
            //if (obj is ShirtCreatorViewModel shirtVM)
            //{
            //    ExcelActions xlActions = new ExcelActions();
            //    if (shirtVM.Shirts != null && shirtVM.Shirts.Count > 0)
            //    {
            //        xlActions.ExportToExel(shirtVM.Shirts);
            //    }

            //    //ShowMultiReplaceWindow(shirtVM);
            //}
        }

        private void ImportFromExcel(object obj)
        {
            //if (obj is ShirtCreatorViewModel shirtVM)
            //{
            //    ExcelActions xlActions = new ExcelActions();
            //    if (xlActions.ImportFromExcel(shirtVM.Shirts))
            //    {
            //        shirtVM.UpdateDescriptionsFromShirt(shirtVM.SelectedShirt);
            //        ShowPopup(shirtVM, "Descriptions imported from excel");
            //    }
            //    else
            //    {
            //        ShowPopup(shirtVM, "Import from excel failed");
            //    }
            //    //ShowMultiReplaceWindow(shirtVM);
            //}
        }

        private void ShowMultiReplaceWindow(ShirtCreatorViewModel shirtVM)
        {
            if (shirtVM != null)
            {
                shirtVM.MultiReplaceVM = new MultiReplaceViewModel();
                if (shirtVM.MultiReplaceVM.ListShirts == null)
                    shirtVM.MultiReplaceVM.ListShirts = new System.Collections.ObjectModel.ObservableCollection<DataGridModel>();
                shirtVM.MultiReplaceVM.ListShirts.Clear();
                foreach (Shirt s in shirtVM.Shirts)
                {
                    shirtVM.UpdateDescriptionsFromShirt(s);
                    shirtVM.MultiReplaceVM.ListShirts.Add(new DataGridModel()
                    {
                        PNGPath = Path.GetFileName(s.ImagePath),
                        Descriptions = shirtVM.Descriptions
                    });
                }
                MultiReplace multiReplaceView = new MultiReplace();
                shirtVM.MultiReplaceVM.SaveCmd = new RelayCommand(ReplaceSaveCmdInvoke);
                shirtVM.MultiReplaceVM.ReplaceAllCmd = new RelayCommand(ReplaceAllCmdInvoke);
                shirtVM.MultiReplaceVM.CloseCmd = new RelayCommand(ReplaceCloseCmdInvoke);
                shirtVM.MultiReplaceVM.GetFileNameCmd = new RelayCommand(ReplaceGetFileNameCmdInvoke);
                shirtVM.MultiReplaceVM.EnterKeyCmd = new RelayCommand(EnterKeyCmdInvoke);
                //shirtVM.EnterKeyCmd = new RelayCommand(EnterKeyCmdInvoke);

                multiReplaceView.DataContext = shirtVM;
                multiReplaceView.ShowDialog();


            }
        }

        private void EnterKeyCmdInvoke(object obj)
        {
            if (obj is System.Windows.Controls.TextBox txb)
            {
                txb.Text += "\n";
                //shirtVM.MultiReplaceVM.
            }

        }

        private void ReplaceGetFileNameCmdInvoke(object obj)
        {
            if (obj is MultiReplaceViewModel replaceVM)
            {
                foreach (DataGridModel item in replaceVM.ListShirts)
                {
                    string s = item.PNGPath;
                    if (!string.IsNullOrEmpty(s))
                    {
                        item.ReplaceByText = s.TrimEnd(new char[] { '.', 'p', 'n', 'g' });
                    }
                }
            }
        }

        private void ReplaceCloseCmdInvoke(object obj)
        {
            if (obj is MultiReplace replaceView)
            {
                replaceView.Close();
            }
        }

        private void ReplaceAllCmdInvoke(object obj)
        {
            if (obj is MultiReplaceViewModel replaceVM)
            {
                foreach (DataGridModel item in replaceVM.ListShirts)
                {
                    if (!string.IsNullOrEmpty(item.ReplaceText))
                    {
                        string s = item.Descriptions.Replace(item.ReplaceText, item.ReplaceByText);
                        item.Descriptions = s;
                    }
                }
            }
        }

        private void ReplaceSaveCmdInvoke(object obj)
        {
            if (obj is ShirtCreatorViewModel shirtVM)
            {

                for (int i = 0; i < shirtVM.Shirts.Count; i++)
                {
                    shirtVM.UpdateDescriptionsToShirt(shirtVM.Shirts[i], shirtVM.MultiReplaceVM.ListShirts[i].Descriptions);
                }
                shirtVM.SelectedShirt = shirtVM.Shirts[0];
            }
        }


        #endregion
    }

}