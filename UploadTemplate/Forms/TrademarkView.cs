using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UploadTemplate
{
    public partial class TrademarkView : Form
    {
        List<List<string>> listTradeMark;
        public TrademarkView()
        {
            InitializeComponent();
        }
        public TrademarkView(List<List<string>> listTradeMark)
        {
            InitializeComponent();
            this.listTradeMark = listTradeMark;
        }

        private void TrademarkView_Load(object sender, EventArgs e)
        {
            ShowTradeMarkList(listTradeMark);
        }

        private void ShowTradeMarkList(List<List<string>> listTM)
        {
            grid_Trademark.Rows.Clear();
            foreach (List<string> tmItem in listTM)
            {
                DataGridViewRow row = new DataGridViewRow();
                foreach (string strValue in tmItem)
                {
                    DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();
                    cell.Value = strValue;
                    row.Cells.Add(cell);
                }
                grid_Trademark.Rows.Add(row);
            }
        }

        private void checkbox_LiveAndText_CheckedChanged(object sender, EventArgs e)
        {
            if(checkbox_LiveAndText.Checked == true)
            {
                List<List<string>> listLiveAndText = listTradeMark.Where(x => (x.Count >= 5 && x[2].ToUpper() == "LIVE" && x[3].ToUpper() == "TEXT")).ToList();
                ShowTradeMarkList(listLiveAndText);
                checkbox_LiveAndText.Text = "Show All";
            }
            else
            {
                ShowTradeMarkList(listTradeMark);
                checkbox_LiveAndText.Text = "Show only \"Live\" and \"Text/Typeset\"";

            }
        }
    }
}
