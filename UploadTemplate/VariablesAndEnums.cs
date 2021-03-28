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
        MainTag = 4,
        SupportingTags = 5,
        Description = 6
    }
}
