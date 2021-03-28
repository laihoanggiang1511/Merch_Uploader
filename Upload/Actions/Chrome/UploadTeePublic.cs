using Common;
using EzUpload.ViewModel;
using OpenQA.Selenium;
using System;
using System.Threading;
using System.Windows.Forms;

namespace EzUpload.Actions.Chrome
{
   public class UploadTeePublic : IUpload
   {
      private string _email;
      private string _password;
      private string _rootXPath = "/html/body/div[2]";

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
               if (shirt.Languages != null && shirt.Languages.Count > 0)
               {                                                            
                  ChromeHelper.SendKeysElement(By.Id("design_design_title"),
                      shirt.Languages[0].Title);

                  ChromeHelper.SendKeysElement(By.Id("design_design_description"),
                      shirt.Languages[0].FeatureBullet1);
               }
               ChromeHelper.SendKeysElement(By.Id("design_primary_tag"),
                   shirt.MainTag);

              IWebElement supportingTags = ChromeHelper.GetElementWithWait(By.XPath("/html/body/div[2]/div/div[2]/div[3]/div/form/div[2]/div[1]/div[1]/div[2]/div[2]/div[2]/ul[1]/li/input"),
                  By.XPath("second option"));
               if (supportingTags != null)
               {
                  supportingTags.SendKeys(shirt.SupportingTags);
               }
               //TODO Maintags to Supporting tags
               Thread.Sleep(2000);
               //Default black color
               //Dropdown button               
               ChromeHelper.ClickElement(By.XPath("/html/body/div[2]/div/div[2]/div[3]/div/form/div[2]/div[2]/div[3]/div[2]/div/table/tbody/tr[2]/td/div[1]/div[1]/div/div/span"),
                  By.XPath("second option"));
               ChromeHelper.ClickElement(By.XPath("/html/body/div[2]/div/div[2]/div[3]/div/form/div[2]/div[2]/div[3]/div[2]/div/table/tbody/tr[2]/td/div[1]/div[1]/div/ul/li[4]/a"),
                  By.XPath("second option"));

               //All color                         
               ChromeHelper.ClickElement(By.XPath("/html/body/div[2]/div/div[2]/div[3]/div/form/div[2]/div[2]/div[3]/div[4]/div[1]/div/a[1]"),
                  By.XPath("second option"));
               //Check box I agree
               ChromeHelper.ClickElement(By.Id("terms"));
               ChromeHelper.ClickElement(By.Id("design_content_flag_false"));
               //Publish
               
               ChromeHelper.ClickElement(By.XPath("/html/body/div[2]/div/div[2]/div[3]/div/form/div[2]/div[4]/div[2]/button[1]"),
                  By.XPath("second option goes here"));
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
               ChromeHelper.ClickElement(By.Id("login"));
               Thread.Sleep(2000);
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

      public void QuitDriver()
      {
         ChromeHelper.QuitDriver();
      }
   }
}
