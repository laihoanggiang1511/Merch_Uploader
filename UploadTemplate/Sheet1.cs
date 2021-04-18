using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Tools.Excel;
using Microsoft.VisualStudio.Tools.Applications.Runtime;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;

namespace UploadTemplate
{
    public partial class Sheet1
    {
        private void Sheet1_Startup(object sender, System.EventArgs e)
        {
            Cells[1, 1].Select();
            //Load Dictionary
        }

        private void Sheet1_Shutdown(object sender, System.EventArgs e)
        {
        }
        private void Sheet1_SelectionChange1(Range Target)
        {
            //TODO something here
            // Static OldRange As Range
            // If Not OldRange Is Nothing Then
            if (Target.Value != null)
            {
                string content = Target.Value?.ToString();
                if ((Target.Column - 6) % 5 == 0)
                {
                    Globals.Sheet1.Cells[3, Target.Column] = "(" + System.Convert.ToString(content.Length) + "/50" + ")";
                    if (content.Length > 50)
                        Target.Font.ColorIndex = 3;
                    else
                        Target.Font.ColorIndex = 1;
                }
                if ((Target.Column - 6) % 5 == 1)
                {
                    Globals.Sheet1.Cells[3, Target.Column] = "(" + System.Convert.ToString(content.Length) + "/60" + ")";
                    if (content.Length > 60)
                        Target.Font.ColorIndex = 3;
                    else
                        Target.Font.ColorIndex = 1;
                }
                if ((Target.Column - 6) % 5 == 2)
                {
                    Globals.Sheet1.Cells[3, Target.Column] = "(" + System.Convert.ToString(content.Length) + "/256" + ")";
                    if (content.Length > 256)
                        Target.Font.ColorIndex = 3;
                    else
                        Target.Font.ColorIndex = 1;
                }
                if ((Target.Column - 6) % 5 == 3)
                {
                    Globals.Sheet1.Cells[3, Target.Column] = "(" + System.Convert.ToString(content.Length) + "/256" + ")";
                    if (content.Length > 256)
                        Target.Font.ColorIndex = 3;
                    else
                        Target.Font.ColorIndex = 1;
                }
                if ((Target.Column - 6) % 5 == 4)
                {
                    Globals.Sheet1.Cells[3, Target.Column] = "(" + System.Convert.ToString(content.Length) + "/2000" + ")";
                    if (content.Length > 2000 || (content.Length > 0 && content.Length < 70))
                        Target.Font.ColorIndex = 3;
                    else
                        Target.Font.ColorIndex = 1;
                }
            }

        }
        #region VSTO Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(Sheet1_Startup);
            this.Shutdown += new System.EventHandler(Sheet1_Shutdown);
            this.SelectionChange += Sheet1_SelectionChange1;
        }





        #endregion

    }
}
