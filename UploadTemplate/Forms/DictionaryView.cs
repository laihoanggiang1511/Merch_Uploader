using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UploadTemplate
{
    public partial class DictionaryView : Form
    {
        public DictionaryView()
        {
            InitializeComponent();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            grid_Dict.Rows.Add();
        }

        private void btn_Remove_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in grid_Dict.SelectedRows)
            {
                grid_Dict.Rows.RemoveAt(row.Index);
            }
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            GlobalVariables.replaceDict.Clear();
            foreach (DataGridViewRow row in grid_Dict.Rows)
            {
                if (row.Cells[0].Value != null && row.Cells[1].Value != null)
                {
                    string key = row.Cells[0].Value.ToString();
                    string value = row.Cells[1].Value.ToString();
                    if (!GlobalVariables.replaceDict.ContainsKey(key))
                    {
                        GlobalVariables.replaceDict.Add(key, value);
                    }
                    else
                    {
                        MessageBox.Show("2 keys is duplicate, the second item will be ignored!");
                    }
                }
            }
            DictionaryActions.SaveDictionary(GlobalVariables.replaceDict);
            DictionaryActions.ReloadDictionary();
            this.Close();
        }

        private void DictionaryView_Load(object sender, EventArgs e)
        {
            foreach( var pair in GlobalVariables.replaceDict)
            {
                DataGridViewRow row = new DataGridViewRow();
                DataGridViewTextBoxCell cellKey = new DataGridViewTextBoxCell();
                cellKey.Value = pair.Key;
                DataGridViewTextBoxCell cellValue = new DataGridViewTextBoxCell();
                cellValue.Value = pair.Value;
                row.Cells.Add(cellKey);
                row.Cells.Add(cellValue);
                grid_Dict.Rows.Add(row);
            }
        }

        private void btn_OpenDict_Click(object sender, EventArgs e)
        {
            string appFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string dictPath = Path.Combine(appFolder, DictionaryActions.dictFile);
            if (File.Exists(dictPath))
            {
                Process.Start(@"notepad.exe", dictPath);
            }
            else
            {
                MessageBox.Show("File not created!");
            }
        }
    }
}
