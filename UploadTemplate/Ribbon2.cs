﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Tools.Ribbon;

namespace UploadTemplate
{
    public partial class Ribbon2
    {
        private void Ribbon2_Load(object sender, RibbonUIEventArgs e)
        {

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
            string[] images = Helper.BrowseForFilePath("PNG file |*.PNG| All Files |*.*", true);
            int startRow = Globals.Sheet1.Application.ActiveCell.Row;
            if (startRow < 4)
            {
                startRow = 4;
            }
            for (int i = 0; i < images.Length; i++)
            {
                Globals.Sheet1.Cells[startRow + i, 1].Value = Path.GetFileName(images[i]);
                Globals.Sheet1.Cells[startRow + i, 2].Value = Path.GetDirectoryName(images[i]);

            }
        }

        private void Btn_SaveFile_Click(object sender, RibbonControlEventArgs e)
        {

        }
    }
}
