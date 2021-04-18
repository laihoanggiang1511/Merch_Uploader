﻿using Microsoft.Office.Core;
using Microsoft.Office.Interop.Excel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EzUpload.DataAccess.Model;

namespace UploadTemplate
{
    public class Actions
    {
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
            Range allRow = Globals.Sheet1.Rows[row];
            allRow.WrapText = true;
            allRow.RowHeight = 100;
            allRow.Activate();
            Range rJson = Globals.Sheet1.Cells[row, (int)ColumnDefinitions.JSON];
            Range rName = Globals.Sheet1.Cells[row, (int)ColumnDefinitions.Name];
            Range rFolder = Globals.Sheet1.Cells[row, (int)ColumnDefinitions.Folder];
            Range rTags = Globals.Sheet1.Cells[row, (int)ColumnDefinitions.MainTag];
            Range rSupportingTags = Globals.Sheet1.Cells[row, (int)ColumnDefinitions.SupportingTags];
            if (sData != null)
            {
                if (!string.IsNullOrEmpty(sData.ImagePath))
                {
                    rName.Value = Path.GetFileName(sData.ImagePath);
                    rFolder.Value = Path.GetDirectoryName(sData.ImagePath);
                    sData.ImagePath = null;
                }
                string stringJSON = JsonConvert.SerializeObject(sData);
                rJson.Value = stringJSON;
                rTags.Value = sData.MainTag;
                rSupportingTags.Value = sData.SupportingTags;
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
                Range rTags = Globals.Sheet1.Cells[row, (int)ColumnDefinitions.MainTag];
                Range rSupportingTags = Globals.Sheet1.Cells[row, (int)ColumnDefinitions.SupportingTags];

                string jsonData = rJson.Value as string;
                if (!string.IsNullOrEmpty(jsonData))
                {
                    ShirtData sData = JsonConvert.DeserializeObject<ShirtData>(jsonData);
                    sData.ImagePath = Path.Combine(rFolder.Value, rName.Value);
                    sData.MainTag = rTags.Value?.ToString();
                    sData.SupportingTags = rSupportingTags.Value?.ToString();
                    for (int i = 0; i < sData.Languages.Count; i++)
                    {
                        sData.Languages[i].BrandName = Globals.Sheet1.Cells[row, (int)ColumnDefinitions.Description + 5 * i].Value?.ToString();
                        sData.Languages[i].Title = Globals.Sheet1.Cells[row, (int)ColumnDefinitions.Description + 1 + 5 * i].Value?.ToString();
                        sData.Languages[i].FeatureBullet1 = Globals.Sheet1.Cells[row, (int)ColumnDefinitions.Description + 2 + 5 * i].Value?.ToString();
                        sData.Languages[i].FeatureBullet2 = Globals.Sheet1.Cells[row, (int)ColumnDefinitions.Description + 3 + 5 * i].Value?.ToString();
                        sData.Languages[i].Description = Globals.Sheet1.Cells[row, (int)ColumnDefinitions.Description + 4 + 5 * i].Value?.ToString();
                    }
                    return sData;
                }
            }
            catch (Exception ex)
            {
            }
            return null;
        }
    }
}
