using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UploadTemplate
{
    public class Helper
    {
        public static string[] BrowseForFilePath(string filter = "PNG file |*.PNG| All Files |*.*", bool multiselect = false)
        {
            OpenFileDialog openFile = new OpenFileDialog
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



    }
}
