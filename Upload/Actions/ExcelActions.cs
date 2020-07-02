using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using Upload.Model;

namespace Upload.Actions
{
    public class ExcelActions
    {
        static Application xlApp = null;
        static Workbook xlWorkBook = null;

        public void ExportToExel(ObservableCollection<Shirt> listShirts)
        {
            try
            {
                xlApp = new Application();
                xlApp.Visible = true;
                string tempFile = Path.GetTempFileName();
                File.Copy(@"C:\Upload.xlsm", tempFile, true);
                Assembly thisAssembly = Assembly.GetExecutingAssembly();
                string fileName = Path.GetDirectoryName(thisAssembly.Location);
                fileName = Path.Combine(fileName, "Upload.xlsm");
                if (File.Exists(fileName))
                {
                    string cloneFile = Path.GetTempFileName();
                    File.Copy(fileName, cloneFile, true);
                    xlWorkBook = xlApp.Workbooks.Open(cloneFile, Editable: true);

                    int i = 0;
                    while (i < listShirts.Count)
                    {
                        ShirtToExcel(listShirts[i], i + 2);
                        i++;
                    }
                    Worksheet englishSheet = xlWorkBook.Sheets["English"] as Worksheet;
                    Worksheet germanSheet = xlWorkBook.Sheets["German"] as Worksheet;
                    Range mRange = englishSheet.Range[englishSheet.Cells[1, 1], englishSheet.Cells[i+10, 9]] as Range;
                    mRange.WrapText = true;
                    englishSheet.Activate();
                }
            }
            catch (Exception e)
            {
                Utils.ShowErrorMessageBox(e.Message);
            }
        }
        public void ImportFromExcel(ObservableCollection<Shirt> listShirts)
        {
            try
            {
                if (xlApp != null && xlWorkBook != null)
                {
                    int row = 2;
                    while (row < listShirts.Count + 10)
                    {
                        Worksheet englishSheet = xlWorkBook.Sheets["English"] as Worksheet;
                        Worksheet germanSheet = xlWorkBook.Sheets["German"] as Worksheet;
                        string PNGPath = GetValueFromCell(englishSheet.Cells[row, 2]) + "\\" + GetValueFromCell(englishSheet.Cells[row, 1]);
                        if (!string.IsNullOrEmpty(PNGPath))
                        {
                            Shirt shirt = listShirts.FirstOrDefault(x => x.DefaultPNGPath == PNGPath);
                            if (shirt != null)
                            {
                                shirt.BrandName = GetValueFromCell(englishSheet.Cells[row, 3]);
                                shirt.DesignTitle = GetValueFromCell(englishSheet.Cells[row, 4]);
                                shirt.FeatureBullet1 = GetValueFromCell(englishSheet.Cells[row, 5]);
                                shirt.FeatureBullet2 = GetValueFromCell(englishSheet.Cells[row, 6]);
                                shirt.Description = GetValueFromCell(englishSheet.Cells[row, 7]);

                                shirt.BrandNameGerman = GetValueFromCell(germanSheet.Cells[row, 3]);
                                shirt.DesignTitleGerman = GetValueFromCell(germanSheet.Cells[row, 4]);
                                shirt.FeatureBullet1German = GetValueFromCell(germanSheet.Cells[row, 5]);
                                shirt.FeatureBullet2German = GetValueFromCell(germanSheet.Cells[row, 6]);
                                shirt.DescriptionGerman = GetValueFromCell(germanSheet.Cells[row, 7]);
                            }
                        }
                        row++;
                    }
                    if (xlWorkBook != null)
                    {
                        xlWorkBook.Close(false);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkBook);
                    }
                    if (xlApp != null)
                    {
                        xlApp.Quit();
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp);
                    }
                    xlWorkBook = null;
                    xlApp = null;
                }
            }
            catch (Exception ex)
            {
                Utils.ShowErrorMessageBox(ex.Message);
            }
        }
        public void ShirtToExcel(Shirt shirt, int row)
        {
            Worksheet englishSheet = xlWorkBook.Sheets["English"] as Worksheet;
            Worksheet germanSheet = xlWorkBook.Sheets["German"] as Worksheet;
            englishSheet.Cells[row, 1] = Path.GetFileName(shirt.DefaultPNGPath);
            englishSheet.Cells[row, 2] = Path.GetDirectoryName(shirt.DefaultPNGPath);
            englishSheet.Cells[row, 3] = shirt.BrandName;
            englishSheet.Cells[row, 4] = shirt.DesignTitle;
            englishSheet.Cells[row, 5] = shirt.FeatureBullet1;
            englishSheet.Cells[row, 6] = shirt.FeatureBullet2;
            englishSheet.Cells[row, 7] = shirt.Description;

            germanSheet.Cells[row, 1] = Path.GetFileName(shirt.DefaultPNGPath);
            germanSheet.Cells[row, 2] = Path.GetDirectoryName(shirt.DefaultPNGPath);
            germanSheet.Cells[row, 3] = shirt.BrandNameGerman;
            germanSheet.Cells[row, 4] = shirt.DesignTitleGerman;
            germanSheet.Cells[row, 5] = shirt.FeatureBullet1German;
            germanSheet.Cells[row, 6] = shirt.FeatureBullet2German;
            germanSheet.Cells[row, 7] = shirt.DescriptionGerman;
        }
        public string GetValueFromCell(Range range)
        {
            try
            {
                if (range.Value == null)
                {
                    return string.Empty;
                }
                else
                {
                    return (string)range.Value;
                }
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}

