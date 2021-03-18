using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UploadTemplate
{

    public class GlobalVariables 
    {
        public static Dictionary<string, string> replaceDict = new Dictionary<string, string>();
    }
    internal enum ColumnDefinitions
    {
        Name = 1,
        Folder = 2,
        JSON = 3,
        Tags = 4,
        Description = 5
    }
}
