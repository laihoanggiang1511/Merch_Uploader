using Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using MessageBox = System.Windows.MessageBox;

namespace EzUpload.Actions.Chrome
{
   public delegate bool LogIn();
   public class ChromeHelper
   {
      public static LogIn LogInCallBack;
      public static ChromeDriver Driver { get; set; }
      public static void QuitDriver()
      {
         try
         {
            if (Driver != null)
            {
               Driver.Quit();
               Driver = null;
            }
         }
         catch (Exception ex)
         {
            Log.log.Fatal(ex);
         }

      }
      public static void StartChromeWithOptions(string userFolderPath)
      {
         if (Driver == null)
         {
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
                  chromeDriverService.HideCommandPromptWindow = true;
                  Driver = new ChromeDriver(chromeDriverService, chrOption);
               }
               else
               {
                  ChromeOptions chrOption = new ChromeOptions();
                  var chromeDriverService = ChromeDriverService.CreateDefaultService();
                  chromeDriverService.HideCommandPromptWindow = true;
                  Driver = new ChromeDriver(chromeDriverService, chrOption);
               }
            }
            catch (Exception ex)
            {
               MessageBox.Show(ex.Message);
            }
            System.Environment.SetEnvironmentVariable("PATH", storedVariable);
         }
      }
      public static bool PasteContent(By by, string content)
      {
         try
         {
            IWebElement element = GetElementWithWait(by);
            if (element != null)
            {
               //element.SendKeys("1");
               System.Windows.Forms.Clipboard.SetText(content);
               OpenQA.Selenium.Interactions.Actions action = new OpenQA.Selenium.Interactions.Actions(Driver);
               action.MoveToElement(element).Perform();
               action.DoubleClick().Perform();
               element.SendKeys(OpenQA.Selenium.Keys.Control + 'v');
               return true;
            }
         }
         catch (Exception ex)
         {
            Log.log.Fatal(ex);
         }
         return false;
      }
      public static bool ClickElement(By by, By secondOption = null)
      {
         try
         {
            Log.log.Info(string.Format("--Click Element {0}--", by));
            IWebElement webElement = GetElementWithWait(by, secondOption);
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
      public static bool SendKeysElement(By by, string input)
      {
         try
         {
            Log.log.Info(string.Format("--Send Key Element {0}--", by));
            IWebElement webElement = GetElementWithWait(by);
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
      public static bool ClickCheckBox(string xPath, bool click = true)
      {
         try
         {
            Log.log.Info(string.Format("--Click Check Box {0}, {1}--", xPath, click.ToString()));
            IWebElement webElement = GetElementWithWait(By.XPath(xPath));
            if (webElement != null)
            {
               string childXpath = xPath + "/i";
               IWebElement iChildWebElement = GetElementWithWait(By.XPath(childXpath));
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
               else if (click == true)
               {
                  webElement.Click();
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

      public static bool SelectByIndex(By by, int index)
      {
         try
         {
            IWebElement element = GetElementWithWait(by);
            if (element != null)
            {
               SelectElement selectElement = new SelectElement(element);
               selectElement.SelectByIndex(index);
            }
         }
         catch (Exception ex)
         {
            Log.log.Fatal(ex);
         }
         return false;
      }
      public static bool SelectByText(By by, string text)
      {
         try
         {
            IWebElement element = GetElementWithWait(by);
            if (element != null)
            {
               SelectElement selectElement = new SelectElement(element);
               selectElement.SelectByText(text);
            }
         }
         catch (Exception ex)
         {
            Log.log.Fatal(ex);
         }
         return false;
      }

      public static IWebElement GetElementWithWait(By firstOption, By secondOption, int waitTime = 3)
      {
         IWebElement element = GetElementWithWait(firstOption, waitTime);
         if (element == null)
         {
            element = GetElementWithWait(secondOption, waitTime);
         }
         return element;
      }

      public static IWebElement GetElementWithWait(By by, int waitTime = 3)
      {
         try
         {
            if (Driver != null)
            {
               WebDriverWait webdriverWait = new WebDriverWait(Driver, TimeSpan.FromSeconds(waitTime));
               IWebElement webElement = webdriverWait.Until(ExpectedConditions.ElementExists(by));
               Random random = new Random();
               int r = random.Next(500, 3000);
               Thread.Sleep(r);
               return webElement;
            }
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

