using Microsoft.Office.Core;
using Microsoft.Office.Interop.Excel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Upload.DataAccess.Model;

namespace UploadTemplate
{
    public delegate void EditShirt(object s);
    public class Actions
    {
        public static EditShirt EditShirtCallBack;
        public static string[] BrowseForFilePath(string filter = "PNG file |*.PNG| All Files |*.*", bool multiselect = false)
        {
            OpenFileDialog openFile = new OpenFileDialog()
            {
                Filter = filter,
                Multiselect = multiselect,
            };
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                return openFile.FileNames;
            }
            else return new string[] { string.Empty };
        }
        public static bool MapShirtToExcel(ShirtData sData, int row)
        {
            Range rJson = Globals.Sheet1.Cells[row, (int)ColumnDefinitions.JSON];
            Range rName = Globals.Sheet1.Cells[row, (int)ColumnDefinitions.Name];
            Range rFolder = Globals.Sheet1.Cells[row, (int)ColumnDefinitions.Folder];
            Range r = Globals.Sheet1.Cells[row, (int)ColumnDefinitions.Folder];

            Range rPrice_US = Globals.Sheet1.Cells[row, (int)ColumnDefinitions.Price_US];
            Range rPrice_UK = Globals.Sheet1.Cells[row, (int)ColumnDefinitions.Price_US];

            if (sData != null)
            {
                if (sData.ImagePath != null)
                {
                    rName.Value = Path.GetFileName(sData.ImagePath);
                    rFolder.Value = Path.GetDirectoryName(sData.ImagePath);
                }
                string stringJSON = JsonConvert.SerializeObject(sData);
                rJson.WrapText = true;
                rJson.RowHeight = 100;
                rJson.Value = stringJSON;
                for (int i = 0; i < sData.Languages.Count; i++)
                {
                    Globals.Sheet1.Cells[row, (int)ColumnDefinitions.Description + 5 * i].Value2 = sData.Languages[i].BrandName;
                    Globals.Sheet1.Cells[row, (int)ColumnDefinitions.Description + 1 + 5 * i].Value2 = sData.Languages[i].Title;
                    Globals.Sheet1.Cells[row, (int)ColumnDefinitions.Description + 2 + 5 * i].Value2 = sData.Languages[i].FeatureBullet1;
                    Globals.Sheet1.Cells[row, (int)ColumnDefinitions.Description + 3 + 5 * i].Value2 = sData.Languages[i].FeatureBullet1;
                    Globals.Sheet1.Cells[row, (int)ColumnDefinitions.Description + 4 + 5 * i].Value2 = sData.Languages[i].Description;
                }

            }

            return true;
        }

        public static ShirtData MapExcelToShirt(int row)
        {
            try
            {
                Range rJson = Globals.Sheet1.Cells[row, (int)ColumnDefinitions.JSON];
                Range rName = Globals.Sheet1.Cells[row, (int)ColumnDefinitions.Name];
                Range rFolder = Globals.Sheet1.Cells[row, (int)ColumnDefinitions.Folder];

                string jsonData = rJson.Value as string;
                if (!string.IsNullOrEmpty(jsonData))
                {
                    ShirtData sData = JsonConvert.DeserializeObject<ShirtData>(jsonData);
                    sData.ImagePath = Path.Combine(rFolder.Value, rName.Value);
                    for (int i = 0; i < sData.Languages.Count; i++)
                    {
                        sData.Languages[i].BrandName = Globals.Sheet1.Cells[row, (int)ColumnDefinitions.Description + 5 * i].Value;
                        sData.Languages[i].Title = Globals.Sheet1.Cells[row, (int)ColumnDefinitions.Description + 1 + 5 * i].Value;
                        sData.Languages[i].FeatureBullet1 = Globals.Sheet1.Cells[row, (int)ColumnDefinitions.Description + 2 + 5 * i].Value;
                        sData.Languages[i].FeatureBullet1 = Globals.Sheet1.Cells[row, (int)ColumnDefinitions.Description + 3 + 5 * i].Value;
                        sData.Languages[i].Description = Globals.Sheet1.Cells[row, (int)ColumnDefinitions.Description + 4 + 5 * i].Value;
                    }
                    return sData;
                }
            }
            catch { }
            return null;
        }
    }
}
