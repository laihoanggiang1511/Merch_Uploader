using Common;
using EzUpload.ViewModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace EzUpload.Actions.Chrome
{
    public class UploadTeePublic : IUpload
    {
        public UploadTeePublic()
        {
            ChromeHelper.LogInCallBack = new LogIn(Log_In);
        }
        public bool Log_In()
        {
            // Log In 
            ChromeHelper.Driver.Navigate().GoToUrl("https://www.teepublic.com/design/quick_create");
            return true;
        }
        public bool Upload(Shirt shirt)
        {
            try
            {
                if (shirt != null)
                {
                    Log.log.Info($"-----------Start Upload-------------");


                    UploadFilePNG(shirt);
                    Thread.Sleep(10000);
                    if (shirt.Languages != null && shirt.Languages.Count > 0)
                    {
                        ChromeHelper.SendKeysElement(By.XPath("/html/body/div[2]/div/div[2]/div[3]/div/form/div[2]/div[1]/div[1]/div[1]/div[1]/input"),
                            shirt.Languages[0].Title);

                        ChromeHelper.SendKeysElement(By.XPath("/html/body/div[2]/div/div[2]/div[3]/div/form/div[2]/div[1]/div[1]/div[1]/div[2]/textarea"),
                            shirt.Languages[0].FeatureBullet1);
                    }
                    ChromeHelper.SendKeysElement(By.XPath("/html/body/div[2]/div/div[2]/div[3]/div/form/div[2]/div[1]/div[1]/div[2]/div[1]/input"),
                        shirt.MainTags);
                    ChromeHelper.SendKeysElement(By.XPath("/html/body/div[2]/div/div[2]/div[3]/div/form/div[2]/div[1]/div[1]/div[2]/div[2]/div[2]/ul[1]/li/input"),
                        shirt.MainTags);
                    //TODO Maintags to Supporting tags

                    //Default black color
                    ChromeHelper.ClickElement(By.XPath("/html/body/div[2]/div/div[3]/div[3]/div/form/div[2]/div[2]/div[3]/div[2]/div/table/tbody/tr[2]/td/div[1]/div[1]/div/div/a/label"));
                    ChromeHelper.ClickElement(By.XPath("/html/body/div[2]/div/div[3]/div[3]/div/form/div[2]/div[2]/div[3]/div[2]/div/table/tbody/tr[2]/td/div[1]/div[1]/div/ul/li[4]/a/label"));
                    //Baseball color
                    ChromeHelper.ClickElement(By.XPath("/html/body/div[2]/div/div[3]/div[3]/div/form/div[2]/div[2]/div[3]/div[2]/div/table/tbody/tr[7]/td/div/div[1]/div/div/a/label"));
                    ChromeHelper.ClickElement(By.XPath("/html/body/div[2]/div/div[3]/div[3]/div/form/div[2]/div[2]/div[3]/div[2]/div/table/tbody/tr[7]/td/div/div[1]/div/ul/li[2]/a/label"));

                    //All color
                    ChromeHelper.ClickElement(By.XPath("/html/body/div[2]/div/div[3]/div[3]/div/form/div[2]/div[2]/div[3]/div[4]/div[1]/div/a[1]"));
                    //Check box I agree
                    ChromeHelper.ClickCheckBox("/html/body/div[2]/div/div[3]/div[3]/div/form/div[2]/div[4]/label/input");
                    //Publish
                    ChromeHelper.ClickElement(By.XPath("/html/body/div[2]/div/div[3]/div[3]/div/form/div[2]/div[4]/div[2]/button[1]"));
                }
            }
            catch (Exception ex)
            {
                string strLog = $"---Fail at shirt {shirt.ImagePath}---\n{ex}";
                Log.log.Fatal(strLog);
                return false;
            }
            return true;
        }

        private bool UploadFilePNG(Shirt shirt)
        {
            try
            {
                Log.log.Info("---Start UploadFilePNG---");
                IWebElement fileInputElement = ChromeHelper.GetElementWithWait(By.XPath("/html/body/div[3]/div/div[2]/div[3]/div/form/div[1]/div[2]/input"));
                fileInputElement.SendKeys(shirt.ImagePath);
                Thread.Sleep(3000);
                Log.log.Info("---End UploadFilePNG---");
                return true;
            }
            catch (Exception ex)
            {
                Log.log.Fatal(ex);
                return false;
            }
        }

        public void OpenChrome(string userFolderPath = null)
        {
            try
            {
                Log.log.Info("---Open Chrome---");
                Cursor.Current = Cursors.WaitCursor;
                if (ChromeHelper.Driver != null)
                {
                    QuitDriver();
                }
                ChromeHelper.StartChromeWithOptions(userFolderPath);
            }
            catch (Exception ex)
            {
                Log.log.Fatal(ex);
                MessageBox.Show(ex.Message);
            }
        }

        public void GoToUploadPage()
        {
            try
            {
                ChromeHelper.Driver.Navigate().GoToUrl("https://www.teepublic.com/design/quick_create");
            }
            catch (Exception ex)
            {
                Log.log.Fatal(ex);
            }
        }

        public bool LogIn()
        {
            GoToUploadPage();
            return true;
        }

        public void QuitDriver()
        {
            ChromeHelper.QuitDriver();
        }
    }
}
