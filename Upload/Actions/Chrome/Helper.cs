using Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;

namespace Upload.Actions.Chrome
{
    public delegate bool LogIn();
    public class Helper
    {
        public static LogIn LogInCallBack;
        

        public static bool ClickElement(ChromeDriver driver, By by)
        {
            try
            {
                Log.log.Info(string.Format("--Click Element {0}--", by));
                IWebElement webElement = GetElementWithWait(driver, by);
                if (webElement != null)
                {
                    webElement.Click();
                    Log.log.Info(string.Format("--Success--"));
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.log.Fatal(ex);
            }
            Log.log.Info(string.Format("--Failed--"));
            return false;
        }

        public static bool PasteToElement(ChromeDriver driver, By by, string input)
        {
            try
            {
                Log.log.Info(string.Format("--Paste To Element {0}--", by));
                IWebElement webElement = GetElementWithWait(driver, by);
                if (webElement != null)
                {
                    System.Windows.Clipboard.SetText(input);
                    webElement.SendKeys(OpenQA.Selenium.Keys.Control + 'a');
                    Thread.Sleep(1000);
                    webElement.SendKeys(OpenQA.Selenium.Keys.Control + 'v');
                    Log.log.Info(string.Format("--Success--"));
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.log.Fatal(ex);
            }
            Log.log.Info(string.Format("--Failed--"));
            return false;
        }
        public static bool SendKeysElement(ChromeDriver driver, By by, string input, bool clear = false)
        {
            try
            {
                Log.log.Info(string.Format("--Send Key Element {0}--", by));
                IWebElement webElement = GetElementWithWait(driver, by);
                if (webElement != null)
                {
                    if(clear)      
                        webElement.Clear();
                    Thread.Sleep(1000);
                    webElement.SendKeys(input);
                    Log.log.Info(string.Format("--Success--"));
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.log.Fatal(ex);
            }
            Log.log.Info(string.Format("--Failed--"));
            return false;
        }
        public static bool ClickCheckBox(ChromeDriver driver, string xPath, bool click = true)
        {
            try
            {
                Log.log.Info(string.Format("--Click Check Box {0}, {1}--", xPath, click.ToString()));
                IWebElement webElement = GetElementWithWait(driver, By.XPath(xPath));
                if (webElement != null)
                {
                    string childXpath = xPath + "/i";
                    IWebElement iChildWebElement = GetElementWithWait(driver, By.XPath(childXpath));
                    if (iChildWebElement != null)
                    {
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
                        Log.log.Info(string.Format("--Success--"));
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.log.Fatal(ex);
            }
            Log.log.Info(string.Format("--Failed--"));
            return false;
        }

        public static IWebElement GetElementWithWait(ChromeDriver driver, By by, int waitTime = 3)
        {
            try
            {
                if (LogInCallBack != null && !LogInCallBack.Invoke())
                {
                    return null;
                }
                WebDriverWait webdriverWait = new WebDriverWait(driver, TimeSpan.FromSeconds(waitTime));
                IWebElement webElement = webdriverWait.Until(ExpectedConditions.ElementExists(by));
                Random random = new Random();
                int r = random.Next(100, 200);
                Thread.Sleep(r);
                return webElement;
            }
            catch (Exception ex)
            {
                Log.log.Fatal(ex);
            }
            return null;
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

