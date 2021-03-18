using Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;

namespace EzUpload.Actions.Chrome
{
    public delegate bool LogIn();
    public class Helper
    {
        public static LogIn LogInCallBack;

        public static ChromeDriver StartChromeWithOptions(string userFolderPath)
        {
            ChromeDriver cDriver = null;

            string storedVariable = System.Environment.GetEnvironmentVariable("PATH");
            try
            {
                string executingFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                System.Environment.SetEnvironmentVariable("PATH", executingFolder);

                if (!string.IsNullOrEmpty(userFolderPath) && Directory.Exists(userFolderPath))
                {
                    ChromeOptions chrOption = new ChromeOptions();
                    chrOption.AddArguments("user-data-dir=" + userFolderPath);
                    var chromeDriverService = ChromeDriverService.CreateDefaultService();
                    chromeDriverService.HideCommandPromptWindow = false;
                    cDriver = new ChromeDriver(chromeDriverService, chrOption);
                }
                else
                {
                    ChromeOptions chrOption = new ChromeOptions();
                    var chromeDriverService = ChromeDriverService.CreateDefaultService();
                    chromeDriverService.HideCommandPromptWindow = false;
                    cDriver = new ChromeDriver(chromeDriverService, chrOption);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            System.Environment.SetEnvironmentVariable("PATH", storedVariable);
            return cDriver;
        }
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
        public static bool SendKeysElement(ChromeDriver driver, By by, string input)
        {
            try
            {
                Log.log.Info(string.Format("--Send Key Element {0}--", by));
                IWebElement webElement = GetElementWithWait(driver, by);
                if (webElement != null)
                {
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

        public static IWebElement GetElementWithWait(ChromeDriver driver, By by, int waitTime = 5)
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
            MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Information);
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

        [DllImport("user32.dll", SetLastError = true)]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        public static void SendString(string strToSend)
        {
            System.Windows.Forms.Keys[] keys = GetKeyByString(strToSend);
            foreach (System.Windows.Forms.Keys key in keys)
            {
                PressKey(key, false);
                Thread.Sleep(100);
                PressKey(key, true);
                Thread.Sleep(100);
            }
        }
        public static void Paste()
        {
            PressKey(System.Windows.Forms.Keys.Control, false);
            Thread.Sleep(100);
            PressKey(System.Windows.Forms.Keys.V, false);
            Thread.Sleep(200);
            PressKey(System.Windows.Forms.Keys.V, true);
            PressKey(System.Windows.Forms.Keys.Control, true);

        }
        public static void PressKey(System.Windows.Forms.Keys key, bool up)
        {
            const int KEYEVENTF_EXTENDEDKEY = 0x1;
            const int KEYEVENTF_KEYUP = 0x2;
            if (up)
            {
                keybd_event((byte)key, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, (UIntPtr)0);
            }
            else
            {
                keybd_event((byte)key, 0x45, KEYEVENTF_EXTENDEDKEY, (UIntPtr)0);
            }
        }
        public static System.Windows.Forms.Keys[] GetKeyByString(string chrInput)
        {
            List<System.Windows.Forms.Keys> result = new List<System.Windows.Forms.Keys>();
            char[] chrArray = chrInput.ToCharArray();
            foreach (char chr in chrArray)
            {
                if (chr == '.')
                {
                    result.Add(System.Windows.Forms.Keys.OemPeriod);
                }
                else
                {
                    result.Add((System.Windows.Forms.Keys)chr);
                }
            }
            return result.ToArray();
        }
    }

}

