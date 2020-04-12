using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upload.GUI;
using Upload.ViewModel;
using Upload.ViewModel.MVVMCore;

namespace Upload.Actions
{
    public class ImageEditorActions
    {
        public void ShowImageEditorWindow(string imageSource = null)
        {
            ImageEditorViewModel imageVM = new ImageEditorViewModel();
            imageVM.CropCmd = new RelayCommand(CropCmdInvoke);
            imageVM.BrowseCmd = new RelayCommand(BrowseCmd);
            if (!string.IsNullOrEmpty(imageSource))
            {
                imageVM.ImageSource = imageSource;
            }
            ImageEditor imageView = new ImageEditor();
            imageView.DataContext = imageVM;
            imageView.Show();
        }

        private void BrowseCmd(object obj)
        {
            if (obj is ImageEditorViewModel imageVM)
            {
                List<string> temp = Utils.BrowseForFilePath("PNG file |*.PNG| All Files |*.*", true).ToList();
                if (imageVM.ListInputPath == null)
                    imageVM.ListInputPath = new ObservableCollection<string>();
                temp.ForEach(x => imageVM.ListInputPath.Add(x));
                imageVM.ImageSource = imageVM.ListInputPath[0];
            }
        }
        private void CropCmdInvoke(object obj)
        {
            try
            {
                if (obj is ImageEditorViewModel imageVM)
                {
                    foreach (string imageSource in imageVM.ListInputPath)
                    {
                        imageVM.ImageSource = imageSource;
                        if (!string.IsNullOrEmpty(imageVM.ImageSource))
                        {
                            Image image = Image.FromFile(imageSource);
                            int inWidth = image.Width;
                            int inHeight = image.Height;
                            int x = 0, y = 0, outWidth = 4500, outHeight = 5400;

                            switch (imageVM.OutputTypeIndex)
                            {
                                case 0: //standard
                                    {
                                        outWidth = 4500;
                                        outHeight = 5400;
                                        break;
                                    }
                                case 1: //hoodie
                                    {
                                        outWidth = 4500;
                                        outHeight = 4050;
                                        break;
                                    }
                            }

                            switch (imageVM.SelectedModeIndex)
                            {
                                case 0://Crop top
                                    {
                                        x = Math.Abs(outWidth - inWidth) / 2;
                                        y = (outHeight - inHeight);
                                        break;
                                    };
                                case 1://Crop bottom
                                    {
                                        x = Math.Abs(outWidth - inWidth) / 2;
                                        y = 0;
                                        break;
                                    };
                                case 2://Crop both
                                    {
                                        x = Math.Abs(outWidth - inWidth) / 2;
                                        y = (outHeight - inHeight) / 2;
                                        break;
                                    };
                            }

                            Bitmap result;
                            if (imageVM.SelectedModeIndex == 3)
                            {
                            }//stretch
                            result = CropImage(image, x, y, outWidth, outHeight);

                            string folderName = Path.GetDirectoryName(imageSource);
                            string fileName = Path.GetFileName(imageSource);
                            string resultFileName = folderName + "\\" + fileName + "-" + result.Width + "x" + result.Height + "px.png";
                            using (FileStream fileStream = new FileStream(resultFileName, FileMode.OpenOrCreate))
                            {
                                result.Save(fileStream, ImageFormat.Png);
                                fileStream.Close();
                            }
                        }
                    }
                    Utils.ShowInfoMessageBox("Job Done!");
                }
            }
            catch (Exception ex)
            {
                Utils.ShowErrorMessageBox(ex.Message);
            }
        }

        Bitmap CropImage(Image source, int x, int y, int width, int height)
        {
            Rectangle crop = new Rectangle(x, y, width, height);

            var bmp = new Bitmap(crop.Width, crop.Height);
            using (var gr = Graphics.FromImage(bmp))
            {
                gr.DrawImage(source, new Rectangle(0, 0, bmp.Width, bmp.Height), crop, GraphicsUnit.Pixel);
            }
            return bmp;
        }
        Bitmap ResizeImage(Image imgToResize, int width, int height)
        {
            Size size = new Size(width, height);
            return (new Bitmap(imgToResize, size));
        }
    }
}
