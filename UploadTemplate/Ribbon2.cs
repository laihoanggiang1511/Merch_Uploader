using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Pipes;
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
using RestSharp.Serialization.Json;
using System.Threading;
using System.Windows.Threading;

namespace UploadTemplate
{

    public partial class Ribbon2
    {
        static Dispatcher MainDispatcher = null; 
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
            int startCell = 4;

            for (int i = startCell; i <= startCell + 4; i++)
            {
                if (Globals.Sheet1.Cells[startRow, i].Value != null)
                {
                    string textToTrans = (string)Globals.Sheet1.Cells[startRow, i].Value;
                    switch (cbb_Language.Text)
                    {
                        case "German":
                            Globals.Sheet1.Cells[startRow, i + 5] = TranslateAPI.Translate(textToTrans, "de");
                            break;
                        case "French":
                            Globals.Sheet1.Cells[startRow, i + 10] = TranslateAPI.Translate(textToTrans, "fr");
                            break;
                        case "Italian":
                            Globals.Sheet1.Cells[startRow, i + 15] = TranslateAPI.Translate(textToTrans, "it");
                            break;
                        case "Spanish":
                            Globals.Sheet1.Cells[startRow, i + 20] = TranslateAPI.Translate(textToTrans, "es");
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
            //sData.ImagePath = string.Empty;
            JsonSerializerSettings settings = new JsonSerializerSettings();
            string strJson = JsonConvert.SerializeObject(sData,Formatting.None);
            string strJsonFileName = Path.GetTempFileName();
            File.WriteAllText(strJsonFileName, strJson);

            string folder = Path.GetDirectoryName(new Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath);
            string exeFile = "Upload.exe";
            exeFile = Path.Combine(folder, exeFile);

            Process proc = new Process();
            proc.StartInfo.FileName = exeFile;
            proc.StartInfo.Arguments = strJsonFileName;
            proc.Start();
            //Start Listener
            MainDispatcher = Dispatcher.CurrentDispatcher;
            ParameterizedThreadStart thrdStart = new ParameterizedThreadStart(EditShirtInvoke);
            Thread thrd = new Thread(thrdStart);
            thrd.Start(startRow);
        }

        public void EditShirtInvoke(object startRow)
        {
            try
            {
                using (var serverPipe = new NamedPipeServerStream("MerchUploaderPipe", PipeDirection.InOut))
                {
                    serverPipe.WaitForConnection();
                    using (StreamReader sr = new StreamReader(serverPipe))
                    {
                        string jsonString = sr.ReadToEnd();
                        if (!string.IsNullOrEmpty(jsonString))
                        {
                            ShirtData sData = JsonConvert.DeserializeObject<ShirtData>(jsonString);
                            MainDispatcher.Invoke(delegate() { Actions.MapShirtToExcel(sData, (int)startRow); });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btn_CheckTM_Click(object sender, RibbonControlEventArgs e)
        {
            MessageBox.Show("Comming Soon");
        }

        private void btn_EditDict_Click(object sender, RibbonControlEventArgs e)
        {
            //MainDispatcher = Dispatcher.CurrentDispatcher;
            //MainDispatcher.Invoke(delegate () { Actions.MapShirtToExcel(sData, (int)startRow); });

            DictionaryView dictView = new DictionaryView();
            dictView.ShowDialog();
        }

        private void btn_UseDictionary_Click(object sender, RibbonControlEventArgs e)
        {
            Range cell = Globals.Sheet1.Application.ActiveCell;
            string key = this.cbb_Dictionary.Text;
            string value = string.Empty;
            if (!string.IsNullOrEmpty(key) &&
                GlobalVariables.replaceDict.TryGetValue(key, out value))
            {
                Globals.Sheet1.Application.ActiveCell.Value = value;
            }
        }
    }
}
