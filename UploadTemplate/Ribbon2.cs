using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Tools.Ribbon;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using Upload.DataAccess;
using Upload.DataAccess.Model;
using Upload.DataAccess.DTO;
using Upload.ViewModel;

namespace UploadTemplate
{

    public partial class Ribbon2
    {
        private void Ribbon2_Load(object sender, RibbonUIEventArgs e)
        {
            cbb_Language.Text = "German";
        }

        private void Btn_Translate_Click(object sender, RibbonControlEventArgs e)
        {
            int startRow = Globals.Sheet1.Application.ActiveCell.Row;
            if (startRow < 4)
            {
                startRow = 4;
            }
            int startCell = 6;

            for (int i = startCell; i <= startCell + 4; i++)
            {
                if (Globals.Sheet1.Cells[startRow, i].Value != null)
                {
                    string textToTrans = (string)Globals.Sheet1.Cells[startRow, i].Value;
                    switch (cbb_Language.Text)
                    {
                        case "German":
                            Globals.Sheet1.Cells[startRow, i + 6] = TranslateAPI.Translate(textToTrans, "de");
                            break;
                        case "French":
                            Globals.Sheet1.Cells[startRow, i + 12] = TranslateAPI.Translate(textToTrans, "fr");
                            break;
                        case "Italian":
                            Globals.Sheet1.Cells[startRow, i + 18] = TranslateAPI.Translate(textToTrans, "it");
                            break;
                        case "Spanish":
                            Globals.Sheet1.Cells[startRow, i + 24] = TranslateAPI.Translate(textToTrans, "es");
                            break;
                    }

                }
            }

        }

        private void Btn_Browse_Click(object sender, RibbonControlEventArgs e)
        {
            string[] images = Actions.BrowseForFilePath("PNG file |*.PNG| All Files |*.*", true);
            int startRow = Globals.Sheet1.Application.ActiveCell.Row;
            if (startRow < 4)
            {
                startRow = 4;
            }
            for (int i = 0; i < images.Length; i++)
            {
                Shirt shirt = new Shirt();
                ShirtData sData = ShirtDTO.MapData(shirt, typeof(ShirtData)) as ShirtData;
                sData.ImagePath = images[i];
                Actions.MapShirtToExcel(sData, startRow + i);
            }
        }

        private void Btn_SaveFile_Click(object sender, RibbonControlEventArgs e)
        {
            int startRow = 4;
            int i = startRow;
            int countBlank = 0;
            while (true)
            {
                string strJSON = Globals.Sheet1.Cells[i, (int)ColumnDefinitions.JSON].Value;
                if (string.IsNullOrEmpty(strJSON))
                {
                    countBlank++;
                }
                else
                {
                    countBlank = 0;
                    ShirtData sData = Actions.MapExcelToShirt(i);
                    if (sData != null && !string.IsNullOrEmpty(sData.ImagePath))
                    {
                        new JsonDataAccess().SaveShirt(sData);
                    }
                }
                if (countBlank > 10)
                    break;
                i++;
            }
            MessageBox.Show("Saved!");
        }

        private void btn_Edit_Click(object sender, RibbonControlEventArgs e)
        {
            int startRow = Globals.Sheet1.Application.ActiveCell.Row;
            if (startRow < 4)
            {
                startRow = 4;
            }
            ShirtData sData = Actions.MapExcelToShirt(startRow);
            //string strJson = JsonConvert.SerializeObject(sData);
            //string strJsonFileName = Path.GetTempFileName();
            //File.WriteAllText(strJsonFileName, strJson);

            //string excelFileName = Globals.ThisWorkbook.Name;
            //string argument = string.Format("{0} {1} {2}",
            //                                strJsonFileName,excelFileName, startRow);
            //string folder = Path.GetDirectoryName(new Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath);
            //string excelFile = "Upload.exe";
            //excelFile = Path.Combine(folder, excelFile);
            //Process.Start(excelFile,argument);

            if (Actions.EditShirtCallBack != null)
            {
                Actions.EditShirtCallBack.Invoke(sData);
            }

        }
    }
}
