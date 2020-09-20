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
using Upload.ViewModel;
using UploadTemplate;

namespace Upload.Actions
{
    public class ExcelActions
    {
        public static Application ExcelApp { get; set; }
        public static Workbook ExcelWorkBook { get; set; }

        public void StartExcel()
        {
            string folder = Path.GetDirectoryName(this.GetType().Assembly.Location);
            string excelFile = "UploadTemplate.xltx";
            excelFile = Path.Combine(folder, excelFile);
            ExcelApp = new Application();
            ExcelApp.Visible = true;
            ExcelWorkBook = ExcelApp.Workbooks.Open(excelFile, ReadOnly:false);
            UploadTemplate.Actions.EditShirtCallBack = new EditShirt(EditShirtInvoke);
        }
        private void EditShirtInvoke(object obj)
        {
            System.Windows.MessageBox.Show("HAHAHA");
        }
    }
}

