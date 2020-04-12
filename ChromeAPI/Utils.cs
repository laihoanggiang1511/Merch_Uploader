using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;

namespace ChromeAPI
{
    public class Utils
    {
        public static ChromeDriver OpenChrome(ChromeDriver driver = null, string userFolderPath = null)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (driver == null)
                {
                    if (userFolderPath != null && userFolderPath != string.Empty)
                    {
                        ChromeOptions chrOption = new ChromeOptions();
                        //chrOption.AddArgument("headless");
                        chrOption.AddArguments("disable-infobars");
                        chrOption.AddArguments("user-data-dir=" + userFolderPath);
                        var chromeDriverService = ChromeDriverService.CreateDefaultService();
                        chromeDriverService.HideCommandPromptWindow = true;
                        driver = new ChromeDriver(chromeDriverService, chrOption);
                    }
                    else
                        driver = new ChromeDriver();
                }
                else
                {
                    driver.Navigate().GoToUrl("https://google.com/");
                }
                return driver;
            }
            catch (Exception ex)
            {
                if (driver != null)
                {
                    driver.Quit();
                }
                if (ex is InvalidOperationException || ex is WebDriverException)
                {
                    return OpenChrome(null, userFolderPath);
                }
                else
                {
                    ShowErrorMessageBox(ex.Message);
                    return null;
                }
            }
        }
        public static bool ClickElement(ChromeDriver driver, By by)
        {
            IWebElement webElement = GetElementWithWait(driver, by);
            if (webElement != null)
            {
                webElement.Click();
                return true;
            }
            else return false;
        }
        public static bool SendKeysElement(ChromeDriver driver, By by, string input)
        {
            IWebElement webElement = GetElementWithWait(driver, by);
            if (webElement != null)
            {
                webElement.Clear();
                Thread.Sleep(1000);
                webElement.SendKeys(input);
                return true;
            }
            else return false;
        }
        public static bool ClickCheckBox(ChromeDriver driver, string xPath, bool click = true)
        {
            try
            {
                IWebElement webElement = GetElementWithWait(driver, By.XPath(xPath));
                string childXpath = xPath + "/i";
                IWebElement iChildWebElement = GetElementWithWait(driver, By.XPath(childXpath));
                bool IsChecked = true;
                string classAttribute = iChildWebElement.GetAttribute("class");
                if (classAttribute.Equals("sci-icon sci-check-box-outline-blank") ||
                    classAttribute.Equals("sci-icon"))// sci-check checkmark
                {
                    IsChecked = false;
                }
                if (IsChecked != click)
                {
                    webElement.Click();
                }
                return true;
            }
            catch
            {
                return false;
            }

        }

        public static IWebElement GetElementWithWait(ChromeDriver driver, By by, int waitTime = 10)
        {
            try
            {
                WebDriverWait webdriverWait = new WebDriverWait(driver, TimeSpan.FromSeconds(waitTime));
                IWebElement webElement = webdriverWait.Until(ExpectedConditions.ElementExists(by));
                Thread.Sleep(200);
                return webElement;
            }
            catch
            {
                return null;
            }
        }
        public static void ShowErrorMessageBox(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        public static void ShowInfoMessageBox(string message, string caption = null)
        {
            MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Error);
        }
        public static bool ShowWarningMessageBox(string message, string caption = null)
        {
            MessageBoxResult result = MessageBox.Show(message, caption, MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.OK)
            {
                return true;
            }
            else
                return false;
        }
    }

}

