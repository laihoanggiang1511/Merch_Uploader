using EzUpload.ViewModel;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzUpload.Actions.Chrome
{
    public interface IUpload
    {
        void GoToUploadPage();
        void OpenChrome(string userFolderPath);
        bool Upload(Shirt shirt);
        bool LogIn();
        void QuitDriver();
    }
}
