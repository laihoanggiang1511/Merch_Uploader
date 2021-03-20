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
    public class UploadTeePublic
    {
        public static ChromeDriver _driver;
        public UploadTeePublic()
        {
        }
        public bool Log_In()
        {
            // Log In 
            try
            {
                _driver.Navigate().GoToUrl("https://www.teepublic.com/design/quick_create");
            }
            catch (Exception ex)
            {
                Log.log.Fatal(ex);
            }
            return false;
        }
        public bool Upload(Shirt shirt)
        {
            try
            {
                if (_driver != null && shirt != null)
                {
                    ChromeHelper.LogInCallBack = new LogIn(Log_In);
                    Log.log.Info($"-----------Start Upload-------------");

                    _driver.Navigate().GoToUrl("https://www.teepublic.com/design/quick_create");

                    UploadFilePNG(shirt);
                    Thread.Sleep(10000);
                    if (shirt.Languages != null && shirt.Languages.Count > 0)
                        ChromeHelper.SendKeysElement(_driver, By.XPath("/html/body/div[2]/div/div[2]/div[3]/div/form/div[2]/div[1]/div[1]/div[1]/div[1]/input"),
                            shirt.Languages[0].Title);
                    ChromeHelper.SendKeysElement(_driver, By.XPath("/html/body/div[2]/div/div[2]/div[3]/div/form/div[2]/div[1]/div[1]/div[1]/div[2]/textarea"),
                        shirt.Languages[0].FeatureBullet1);
                    ChromeHelper.SendKeysElement(_driver, By.XPath("/html/body/div[2]/div/div[2]/div[3]/div/form/div[2]/div[1]/div[1]/div[2]/div[1]/input"),
                        shirt.MainTags);
                    ChromeHelper.SendKeysElement(_driver, By.XPath("/html/body/div[2]/div/div[2]/div[3]/div/form/div[2]/div[1]/div[1]/div[2]/div[2]/div[2]/ul[1]/li/input"),
                        shirt.MainTags);

                    //Default black color
                    ChromeHelper.ClickElement(_driver, By.XPath("/html/body/div[2]/div/div[3]/div[3]/div/form/div[2]/div[2]/div[3]/div[2]/div/table/tbody/tr[2]/td/div[1]/div[1]/div/div/a/label"));
                    ChromeHelper.ClickElement(_driver, By.XPath("/html/body/div[2]/div/div[3]/div[3]/div/form/div[2]/div[2]/div[3]/div[2]/div/table/tbody/tr[2]/td/div[1]/div[1]/div/ul/li[4]/a/label"));
                    //Baseball color
                    ChromeHelper.ClickElement(_driver, By.XPath("/html/body/div[2]/div/div[3]/div[3]/div/form/div[2]/div[2]/div[3]/div[2]/div/table/tbody/tr[7]/td/div/div[1]/div/div/a/label"));
                    ChromeHelper.ClickElement(_driver, By.XPath("/html/body/div[2]/div/div[3]/div[3]/div/form/div[2]/div[2]/div[3]/div[2]/div/table/tbody/tr[7]/td/div/div[1]/div/ul/li[2]/a/label"));

                    //All color
                    ChromeHelper.ClickElement(_driver, By.XPath("/html/body/div[2]/div/div[3]/div[3]/div/form/div[2]/div[2]/div[3]/div[4]/div[1]/div/a[1]"));
                    //Check box I agree
                    ChromeHelper.ClickCheckBox(_driver, "/html/body/div[2]/div/div[3]/div[3]/div/form/div[2]/div[4]/label/input");
                    //Publish
                    ChromeHelper.ClickElement(_driver, By.XPath("/html/body/div[2]/div/div[3]/div[3]/div/form/div[2]/div[4]/div[2]/button[1]"));
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

        private bool InputDetail(Shirt shirt)
        {
            try
            {

                return true;
            }
            catch (Exception ex)
            {
                Log.log.Fatal(ex);
                return false;
            }
        }
        private bool SelectProduct(Shirt shirt)
        {
            try
            {

                return true;
            }
            catch (Exception ex)
            {
                Log.log.Fatal(ex);
                return false;
            }
        }

        private bool UploadFilePNG(Shirt shirt)
        {
            try
            {
                Log.log.Info("---Start UploadFilePNG---");
                if (_driver != null)
                {
                    IWebElement fileInputElement = ChromeHelper.GetElementWithWait(_driver, By.XPath("/html/body/div[3]/div/div[2]/div[3]/div/form/div[1]/div[2]/input"));
                    fileInputElement.SendKeys(shirt.ImagePath);
                    Log.log.Info("---End UploadFilePNG---");
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                Log.log.Fatal(ex);
                return false;
            }
        }
        public static void QuitDriver()
        {
            //try
            //{
            //    Log.log.Info("---Quit Driver---");
            //    if (driver != null)
            //        driver.Close();
            //    driver = null;
            //}
            //catch (Exception ex)
            //{
            //    Log.log.Fatal(ex);
            //}
            //finally
            //{
            if (_driver != null)
            {
                _driver.Quit();
            }
            //}

        }

        public void OpenChrome(string userFolderPath = null)
        {
            try
            {

                Log.log.Info("---Open Chrome---");
                Cursor.Current = Cursors.WaitCursor;
                if (_driver != null)
                {
                    QuitDriver();
                }
                _driver = ChromeHelper.StartChromeWithOptions(userFolderPath);
            }
            catch (Exception ex)
            {
                Log.log.Fatal(ex);
                MessageBox.Show(ex.Message);
            }
        }


    }
}
