using Common;
using EzUpload.ViewModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace EzUpload.Actions.Chrome
{
   public class UploadTeePublic : IUpload
   {
      private string _email;
      private string _password;

      public UploadTeePublic(string email, string password)
      {
         ChromeHelper.LogInCallBack = new LogIn(LogIn);
         _email = email;
         _password = password;
      }
      public bool Upload(Shirt shirt)
      {
         try
         {
            if (shirt != null)
            {
               Log.log.Info($"-----------Start Upload-------------");
               UploadFilePNG(shirt);
               ChromeHelper.ClickElement(By.Id("design_content_flag_false"));
               if (shirt.Languages != null && shirt.Languages.Count > 0)
               {
                  ChromeHelper.SendKeysElement(By.Id("design_design_title"),
                      shirt.Languages[0].Title);

                  ChromeHelper.SendKeysElement(By.Id("design_design_description"),
                      shirt.Languages[0].FeatureBullet1);
               }

               ChromeHelper.SendKeysElement(By.Id("design_primary_tag"), shirt.MainTag);
               ChromeHelper.ClickElement(By.XPath("/html/body/ul/li[1]"));
               Thread.Sleep(2000);

               IWebElement supportingTagsElement = ChromeHelper.GetElementWithWait(By.XPath("/html/body/div[2]/div/div[2]/div[3]/div/form/div[2]/div[1]/div[1]/div[2]/div[2]/div[2]/ul[1]/li/input"),
                   By.XPath("/html/body/div[3]/div/div[2]/div[3]/div/form/div[2]/div[1]/div[1]/div[2]/div[2]/div[2]/ul[1]/li/input"));

               if (supportingTagsElement != null)
               {
                  List<string> supportingTags = Utils.ExtractTags(shirt.SupportingTags);
                  foreach (string tag in supportingTags)
                  {
                     supportingTagsElement.SendKeys(tag);
                     supportingTagsElement.SendKeys(",");
                     Thread.Sleep(1000);
                  }
               }
               //Default black color

               //Dropdown button               
               ChromeHelper.ClickElement(By.XPath("/html/body/div[2]/div/div[2]/div[3]/div/form/div[2]/div[2]/div[3]/div[2]/div/table/tbody/tr[2]/td/div[1]/div[1]/div/div/span"),
                  By.XPath("/html/body/div[3]/div/div[2]/div[3]/div/form/div[2]/div[2]/div[3]/div[2]/div/table/tbody/tr[2]/td/div[1]/div[1]/div/div/span"));
               ChromeHelper.ClickElement(By.XPath("/html/body/div[2]/div/div[2]/div[3]/div/form/div[2]/div[2]/div[3]/div[2]/div/table/tbody/tr[2]/td/div[1]/div[1]/div/ul/li[4]/a"),
                  By.XPath("/html/body/div[3]/div/div[2]/div[3]/div/form/div[2]/div[2]/div[3]/div[2]/div/table/tbody/tr[2]/td/div[1]/div[1]/div/ul/li[4]/a"));
               Thread.Sleep(2000);

               //Base ball
               ChromeHelper.ClickElement(By.XPath("/html/body/div[2]/div/div[2]/div[3]/div/form/div[2]/div[2]/div[3]/div[2]/div/table/tbody/tr[7]/td/div/div[1]/div/div/span"),
                  By.XPath("/html/body/div[3]/div/div[2]/div[3]/div/form/div[2]/div[2]/div[3]/div[2]/div/table/tbody/tr[7]/td/div/div[1]/div/div/span"));
               ChromeHelper.ClickElement(By.XPath("/html/body/div[2]/div/div[2]/div[3]/div/form/div[2]/div[2]/div[3]/div[2]/div/table/tbody/tr[7]/td/div[1]/div[1]/div/ul/li[2]/a"),
                  By.XPath("/html/body/div[3]/div/div[2]/div[3]/div/form/div[2]/div[2]/div[3]/div[2]/div/table/tbody/tr[7]/td/div/div[1]/div/ul/li[2]/a"));

               //All color                         
               ChromeHelper.ClickElement(By.XPath("/html/body/div[2]/div/div[2]/div[3]/div/form/div[2]/div[2]/div[3]/div[4]/div[1]/div/a[1]"),
                  By.XPath("/html/body/div[3]/div/div[2]/div[3]/div/form/div[2]/div[2]/div[3]/div[4]/div[1]/div/a[1]"));
               //Check box I agree
               ChromeHelper.ClickElement(By.Id("terms"));
               Thread.Sleep(3000);
               //Publish
               if (ChromeHelper.ClickElement(By.XPath("/html/body/div[2]/div/div[2]/div[3]/div/form/div[2]/div[4]/div[2]/button[1]"),
                  By.XPath("/html/body/div[3]/div/div[2]/div[3]/div/form/div[2]/div[4]/div[2]/button[1]")) == false)
               {
                  return false;
               }
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
            IWebElement fileInputElement = ChromeHelper.GetElementWithWait(By.Name("file"));
            fileInputElement.SendKeys(shirt.ImagePath);
            IWebElement statusElement = ChromeHelper.GetElementWithWait(By.XPath("/html/body/div[2]/div/div[2]/div[3]/div/form/div[1]/div[2]/div[2]"),
               By.XPath("/html/body/div[3]/div/div[2]/div[3]/div/form/div[1]/div[2]/div[2]"));

            while (statusElement.Text.Length > 1)
            {

            }
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
         string storedVariable = System.Environment.GetEnvironmentVariable("PATH");
         try
         {
            Log.log.Info("---Open Chrome---");
            Cursor.Current = Cursors.WaitCursor;
            if (ChromeHelper.Driver != null)
            {
               QuitDriver();
            }
            string executingFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            System.Environment.SetEnvironmentVariable("PATH", executingFolder);

            if (!string.IsNullOrEmpty(userFolderPath) && Directory.Exists(userFolderPath))
            {
               ChromeOptions chrOption = new ChromeOptions();
               chrOption.AddArguments("user-data-dir=" + userFolderPath);
               chrOption.AddArguments("--disable-blink-features=AutomationControlled");
               chrOption.AddArguments("--user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/89.0.4389.90 Safari/537.36 Edg/89.0.774.63");
               chrOption.AddArguments("--disable-extensions");
               chrOption.AddArguments("--profile-directory=Default");
               //chrOption.AddArguments("--incognito");
               chrOption.AddArguments("--disable-plugins-discovery");
               chrOption.AddArguments("--start-maximized");
               var chromeDriverService = ChromeDriverService.CreateDefaultService();
               chromeDriverService.HideCommandPromptWindow = false;
               ChromeHelper.Driver = new ChromeDriver(chromeDriverService, chrOption);
               ChromeHelper.Driver.ExecuteScript("Object.defineProperty(navigator, 'webdriver', {get: () => undefined})");
            }
            else
            {
               ChromeOptions chrOption = new ChromeOptions();
               chrOption.AddArguments("--user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/89.0.4389.90 Safari/537.36 Edg/89.0.774.63");
               chrOption.AddArguments("--disable-blink-features=AutomationControlled");
               chrOption.AddArguments("--disable-extensions");
               chrOption.AddArguments("--profile-directory=Default");
               //chrOption.AddArguments("--incognito");
               chrOption.AddArguments("--disable-plugins-discovery");
               chrOption.AddArguments("--start-maximized");
               var chromeDriverService = ChromeDriverService.CreateDefaultService();
               chromeDriverService.HideCommandPromptWindow = false;
               ChromeHelper.Driver = new ChromeDriver(chromeDriverService, chrOption);
               ChromeHelper.Driver.ExecuteScript("Object.defineProperty(navigator, 'webdriver', {get: () => undefined})");
            }
         }
         catch (Exception ex)
         {
            Log.log.Fatal(ex);
            MessageBox.Show(ex.Message);
         }
         System.Environment.SetEnvironmentVariable("PATH", storedVariable);
      }

      public void GoToUploadPage()
      {
         try
         {
            Log.log.Info("---Go to upload page---");
            ChromeHelper.Driver.Navigate().GoToUrl("https://www.teepublic.com/design/quick_create");
            Log.log.Info("---Go to upload page success---");

         }
         catch (Exception ex)
         {
            Log.log.Fatal(ex);
         }
      }

      public bool LogIn()
      {
         if (!ChromeHelper.Driver.Url.Contains("https://www.teepublic.com/designs"))
         {
            Log.log.Info("---Start Log In---");
            try
            {
               CheckCaptcha();
               OpenQA.Selenium.Interactions.Actions actions = new OpenQA.Selenium.Interactions.Actions(ChromeHelper.Driver);
               IWebElement logInDropDownElement = ChromeHelper.GetElementWithWait(By.XPath("/html/body/div[2]/div/header/div[1]/div/nav/div[2]/div[2]/div/a/i"),
                  By.XPath("/html/body/div[3]/div/header/div/nav/div[3]/div/div[2]/div[1]/a"));

               if (logInDropDownElement != null)
               {
                  actions.MoveToElement(logInDropDownElement).Build().Perform();
                  IWebElement logInLinkElement = ChromeHelper.GetElementWithWait(By.XPath("/html/body/div[2]/div/header/div[1]/div/nav/div[2]/div[2]/div/div[2]/nav/a[2]"),
                     By.XPath("/html/body/div[3]/div/header/div/nav/div[3]/div/div[2]/div[2]/nav/div[1]/p/a[1]"));
                  if (logInLinkElement != null)
                  {
                     logInLinkElement.Click();
                  }
               }
               ChromeHelper.SendKeysElement(By.Id("session_email"), _email);
               ChromeHelper.SendKeysElement(By.Id("session_password"), _password);
               Thread.Sleep(3000);
               ChromeHelper.ClickElement(By.Id("login"));
               CheckCaptcha();
               Thread.Sleep(3000);
               Log.log.Info("--- Log In Success---");
            }
            catch (Exception ex)
            {
               Log.log.Fatal(ex);
            }
            GoToUploadPage();
         }
         return true;
      }

      private void CheckCaptcha()
      {
         while (true)
         {
            IWebElement element = ChromeHelper.GetElementWithWait(By.Id("main-iframe"),
               By.XPath("/html/body/div[2]/div[3]/div[1]/div/div/span/div[1]"));
            if (element == null)
            {
               break;
            }
         }
      }

      public void QuitDriver()
      {
         ChromeHelper.QuitDriver();
      }

   }
}
