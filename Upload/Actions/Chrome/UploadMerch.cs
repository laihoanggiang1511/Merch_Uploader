using Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using EzUpload.ViewModel;
using OpenQA.Selenium.Interactions;
using System.Reflection;
using System.Threading;
using System.Text.RegularExpressions;

namespace EzUpload.Actions.Chrome
{
   public class UploadMerch : IUpload
   {
      private string _email;
      private string _password;
      public UploadMerch(string password, string email)
      {
         this._password = password;
         this._email = email;
         ChromeHelper.LogInCallBack = new LogIn(LogIn);
      }
      public bool LogIn()
      {
         // Log In 
         try
         {
            if (ChromeHelper.Driver != null)
            {
               if (ChromeHelper.Driver.Url.Contains("www.amazon.com/ap/signin"))
               {
                  Log.log.Info("------Start Log In----------");
                  if (!string.IsNullOrEmpty(_email))
                  {
                     ChromeHelper.SendKeysElement(By.Id("ap_email"), _email);
                  }
                  ChromeHelper.SendKeysElement(By.Id("ap_password"), _password);
                  ChromeHelper.ClickElement(By.Id("signInSubmit"));
                  System.Threading.Thread.Sleep(2000);
                  if (ChromeHelper.Driver.Url.Contains("www.amazon.com/ap/accountfixup"))
                  {
                     ChromeHelper.ClickElement(By.Id("ap-account-fixup-phone-skip-link"));
                  }
                  System.Threading.Thread.Sleep(2000);
                  Log.log.Info("------LogIn Sucess----------");
               }
               return true;
            }
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
            if (shirt != null)
            {
               Log.log.Info($"-----------Start Upload-------------");
               GoToUploadPage();
               LogIn();
               if (ChromeHelper.GetElementWithWait(By.Id("select-marketplace-button")) == null)
               {
                  GoToUploadPage();
               }
               //Set descriptions
               Log.log.Info("---Descriptions---");
               ChromeHelper.ClickElement(By.Id("acc-control-all"));

               for (int i = 0; i <= 4; i++)
               {
                  if (shirt.Languages.Count > i)
                  {
                     string rootXPath = $"/html/body/div[1]/div/app-root/div/ng-component/div/product-config-editor/product-text/ngb-accordion/div[{i + 1}]/div[2]/div/product-text-editor/div/div/";
                     if (!string.IsNullOrEmpty(shirt.Languages[i].BrandName))
                        ChromeHelper.SendKeysElement(By.Id("designCreator-productEditor-brandName"), shirt.Languages[i].BrandName);
                     if (!string.IsNullOrEmpty(shirt.Languages[i].Title))
                        ChromeHelper.SendKeysElement(By.Id("designCreator-productEditor-title"), shirt.Languages[i].Title);
                     if (!string.IsNullOrEmpty(shirt.Languages[i].FeatureBullet1))
                        ChromeHelper.SendKeysElement(By.Id("designCreator-productEditor-featureBullet1"), shirt.Languages[i].FeatureBullet1);
                     if (!string.IsNullOrEmpty(shirt.Languages[i].FeatureBullet2))
                        ChromeHelper.SendKeysElement(By.Id("designCreator-productEditor-featureBullet2"), shirt.Languages[i].FeatureBullet2);
                     if (!string.IsNullOrEmpty(shirt.Languages[i].Description))
                        ChromeHelper.SendKeysElement(By.Id("designCreator-productEditor-description"), shirt.Languages[i].Description);
                  }
               }


               // Select Products
               if (!ChromeHelper.ClickElement(By.Id("select-marketplace-button")))
                  return false;
               Thread.Sleep(1000);
               if (!SelectProduct(shirt))
                  return false;


               // Upload .png files
               if (!UploadFilePNG(shirt))
                  return false;

               //Input detail
               InputDetail(shirt);

               // Submit
               Log.log.Info("---Summit---");
               ChromeHelper.ClickElement(By.Id("submit-button"));
               System.Threading.Thread.Sleep(1000);
               if (!ChromeHelper.ClickElement(By.XPath("/html/body/ngb-modal-window/div/div/ng-component/div[2]/div[2]/button[2]")))
               {
                  return false;
               }
               System.Threading.Thread.Sleep(500);
               Log.log.Info("-----------End Upload-----------");
               return true;
            }
            else
               return false;
         }
         catch (Exception ex)
         {
            string strLog = $"---Fail at shirt {shirt.ImagePath}---\n{ex}";
            Log.log.Fatal(strLog);
            return false;
         }
      }

      private bool InputDetail(Shirt shirt)
      {
         try
         {
            Log.log.Info("---Input Detail---");

            ChromeHelper.ClickElement(By.XPath("//*[@id=\"STANDARD_TSHIRT-card\"]/div[2]/button"));
            ShirtType standardType = shirt.ShirtTypes[0];

            // Shirt type
            if (standardType != null && standardType.FitTypes != null && standardType.FitTypes.Count > 1)
            {
               for (int fitTypeIndex = 0; fitTypeIndex < standardType.FitTypes.Count; fitTypeIndex++)
               {
                  ChromeHelper.ClickCheckBox($"/html/body/div[1]/div/app-root/div/div[2]/ng-component/div/product-config-editor/div[3]/div[{1}]/product-editor/div/div[2]/div/div[2]/div[1]/dimension-editor/fit-type/div/div/label[{fitTypeIndex+1}]/flowcheckbox/span",
                      standardType.FitTypes[fitTypeIndex]);
               }
            }

            // Colors
            var checkedColors = ChromeHelper.Driver.FindElements(By.CssSelector(".sci-check.checkmark"));

            foreach(IWebElement checkedColor in checkedColors)
            {
               IWebElement parentElement = checkedColor.FindElement(By.XPath("./.."));

               if(parentElement != null)
               {
                  parentElement.Click();
                  Thread.Sleep(100);
               }
            }

            var colorCheckBoxElements = ChromeHelper.Driver.FindElements(By.ClassName("color-checkbox"));

            foreach (Color color in standardType.Colors)
            {
               string colorName = Regex.Replace(color.ColorName, "[^a-zA-Z0-9]", "").ToLower();

               if (color.IsActive == false)
                  continue;

               IWebElement colorCheckboxElement = colorCheckBoxElements.FirstOrDefault(p => Regex.Replace(p.GetAttribute("class"), "[^a-zA-Z0-9]", "").ToLower().Contains(colorName));

               colorCheckboxElement?.Click();
               Thread.Sleep(100);
            }



            //for (int j = 0; j < standardType.Colors.Count; j++)
            //{
            //   Color color = standardType.Colors[j];
            //   ChromeHelper.ClickCheckBox($"/html/body/div[1]/div/app-root/div/ng-component/div/product-config-editor/div[2]/div[1]/product-editor/div/div[2]/div/div[2]/div[1]/dimension-editor/color/div/div/div[2]/div[{j + 1}]/colorcheckbox/span", color.IsActive);
               
            
            //}

            //for (int i = 0; i < shirt.ShirtTypes.Count; i++)
            //{
            //   int row = (i) / 4 + 1;
            //   int column = (i) % 4 + 1;
            //   ShirtType s = shirt.ShirtTypes[i];
            //   if (s.IsActive)
            //   {
            //      Log.log.Info($"---i={i}---");
            //      // Edit Detail-Standard                
            //      ChromeHelper.ClickElement(By.XPath($"/html/body/div[1]/div/app-root/div/ng-component/div/product-config-editor/div[2]/div[{row}]/div[{column}]/product-card/div/div[2]/button"));
            //      // Choose Fit type
            //      if (s.FitTypes != null && s.FitTypes.Count > 1)
            //      {
            //         for (int j = 0; j < s.FitTypes.Count; j++)
            //         {
            //            ChromeHelper.ClickCheckBox($"/html/body/div[1]/div/app-root/div/ng-component/div/product-config-editor/div[2]/div[{row}]/product-editor/div/div[2]/div/div[2]/div[1]/dimension-editor/fit-type/div/div/label[{j + 1}]/flowcheckbox/span",
            //                s.FitTypes[j]);
            //         }
            //      }

            //      // Select Color - non japanese types
            //      if (s.TypeName != "StandardTShirt" &&
            //         s.TypeName != "LongSleeveTShirt" &&
            //         s.TypeName != "SweetShirt" &&
            //         s.TypeName != "PullOverHoodie")
            //      {
            //         for (int j = 0; j < s.Colors.Count; j++)
            //         {
            //            Color color = s.Colors[j];
            //            ChromeHelper.ClickCheckBox($"/html/body/div[1]/div/app-root/div/ng-component/div/product-config-editor/div[2]/div[{row}]/product-editor/div/div[2]/div/div[2]/div[1]/dimension-editor/color/div/div/div[2]/div[{j + 1}]/colorcheckbox/span", color.IsActive);
            //         }
            //      }
            //      else
            //      {
            //         List<Color> japaneseColor = new List<Color>();
            //         List<Color> nonJapaneseColor = new List<Color>();

            //         //Process for japanese colors
            //         for (int j = 0; j < s.Colors.Count; j++)
            //         {

            //            Color col = s.Colors[j];
            //            if (col.ColorName == "Brown" ||
            //                col.ColorName == "Dark Heather" ||
            //                col.ColorName == "Grass" ||
            //                col.ColorName == "Heather Blue" ||
            //                col.ColorName == "Silver" ||
            //                col.ColorName == "Slate")
            //            {
            //               japaneseColor.Add(col);
            //            }
            //            else
            //            {
            //               nonJapaneseColor.Add(col);
            //            }
            //         }
            //         for (int j = 0; j < nonJapaneseColor.Count; j++)
            //         {
            //            ChromeHelper.ClickCheckBox($"/html/body/div[1]/div/app-root/div/ng-component/div/product-config-editor/div[2]/div[{row}]/product-editor/div/div[2]/div/div[2]/div[1]/dimension-editor/color/div/div/div[2]/div[{j + 2}]/colorcheckbox/span", nonJapaneseColor[j].IsActive);
            //         }
            //         for (int j = 0; j < japaneseColor.Count; j++)
            //         {
            //            ChromeHelper.ClickCheckBox($"/html/body/div[1]/div/app-root/div/ng-component/div/product-config-editor/div[2]/div[{row}]/product-editor/div/div[2]/div/div[2]/div[1]/dimension-editor/color/div/div/div[3]/div[{j + 2}]/colorcheckbox/span", japaneseColor[j].IsActive);
            //         }
            //      }

            //      // Set Price
            //      for (int j = 0; j < s.MarketPlaces.Count; j++)
            //      {
            //         if (s.MarketPlaces[j])
            //         {
            //            By by = By.XPath($"/html/body/div[1]/div/app-root/div/ng-component/div/product-config-editor/div[2]/div[{row}]/product-editor/div/div[2]/div/div[2]/div[2]/listing-details/div/price-editor[{j + 1}]/div/div/div[2]/div[1]/div[1]/input");
            //            string price = s.Prices[j].ToString();
            //            ChromeHelper.PasteContent(by, price);
            //         }
            //      }
            //      //Click again to close pallete
            //      ChromeHelper.ClickElement(By.XPath($"/html/body/div[1]/div/app-root/div/ng-component/div/product-config-editor/div[2]/div[{row}]/div[{column}]/product-card/div/div[2]/button"));
            //   }
            //}
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
            Log.log.Info("---Select Product---");
            //Check then Uncheck All Checkbox
            ChromeHelper.ClickElement(By.Id("select-none"));

            ChromeHelper.ClickElement(By.XPath("/html/body/ngb-modal-window/div/div/ng-component/div[2]/div/div/table/tbody/tr[3]/td[2]/flowcheckbox"));

            //for (int i = 0; i < shirt.ShirtTypes.Count; i++)
            //{
            //   ShirtType sb = shirt.ShirtTypes[i];
            //   Log.log.Info(sb.TypeName.ToString());
            //   for (int j = 0; j < sb.MarketPlaces.Count; j++)
            //   {
            //      Log.log.Info($"i={i};j={j}");
            //      if (!ChromeHelper.ClickCheckBox($"/html/body/ngb-modal-window/div/div/ng-component/div[2]/div[2]/div/table/tbody/tr[{i + 3}]/td[{j + 2}]/flowcheckbox/span",
            //                              sb.IsActive && sb.MarketPlaces[j]))
            //      {
            //         return false;
            //      }
            //   }
            //}
            // next button
            Log.log.Info("---Next Button---");
            if (!ChromeHelper.ClickElement(By.XPath("/html/body/ngb-modal-window/div/div/ng-component/div[3]/button")))
            {
               return false;
            }
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
            switch (shirt.ImageType)
            {
               case (int)PNGImageType.Standard_Front:
                  {
                     Log.log.Info("-STANDARD_TSHIRT-FRONT-");
                     //ChromeHelper.SendKeysElement(By.Id("STANDARD_TSHIRT-FRONT"), shirt.ImagePath);
                     IWebElement webElement = ChromeHelper.Driver.FindElement(By.ClassName("file-upload-input"));
                     webElement.SendKeys(shirt.ImagePath);
                     break;
                  }
               case (int)PNGImageType.Standard_Back:
                  {
                     ChromeHelper.ClickElement(By.XPath("/html/body/div[1]/div/app-root/div/ng-component/div/ng-component/div[2]/div[1]/div[1]/product-card/div/button"));
                     ChromeHelper.ClickElement(By.XPath("/html/body/div[1]/div/app-root/div/ng-component/div/ng-component/div[2]/div[1]/product-editor/div/div[2]/div/div[1]/product-asset-editor/div/div[2]/div/button[2]"));
                     //ChromeHelper.SendKeysElement(By.Id("STANDARD_TSHIRT-BACK"), shirt.ImagePath);
                     IWebElement webElement = ChromeHelper.Driver.FindElement(By.Id("STANDARD_TSHIRT-BACK"));
                     webElement.SendKeys(shirt.ImagePath);
                     ChromeHelper.ClickElement(By.XPath("/html/body/div[1]/div/app-root/div/ng-component/div/ng-component/div[2]/div[1]/div[1]/product-card/div/button"));
                     break;
                  }
               case (int)PNGImageType.Hoodie_Front:
                  {
                     //ChromeHelper.SendKeysElement(By.Id("STANDARD_PULLOVER_HOODIE-FRONT"), shirt.ImagePath);
                     IWebElement webElement = ChromeHelper.Driver.FindElement(By.Id("STANDARD_PULLOVER_HOODIE-FRONT"));
                     webElement.SendKeys(shirt.ImagePath);
                     break;
                  }
               case (int)PNGImageType.Hoodie_Back:
                  {
                     Log.log.Info("-STANDARD_PULLOVER_HOODIE-BACK-");
                     ChromeHelper.ClickElement(By.XPath("/html/body/div[1]/div/app-root/div/ng-component/div/ng-component/div[2]/div[2]/div[4]/product-card/div/button"));
                     ChromeHelper.ClickElement(By.XPath("/html/body/div[1]/div/app-root/div/ng-component/div/ng-component/div[2]/div[2]/product-editor/div/div[2]/div/div[1]/product-asset-editor/div/div[2]/div/button[2]"));

                     //ChromeHelper.SendKeysElement(By.Id("STANDARD_PULLOVER_HOODIE-BACK"), shirt.ImagePath);

                     IWebElement webElement = ChromeHelper.Driver.FindElement(By.Id("STANDARD_PULLOVER_HOODIE-BACK"));
                     webElement.SendKeys(shirt.ImagePath);
                     ChromeHelper.ClickElement(By.XPath("/html/body/div[1]/div/app-root/div/ng-component/div/ng-component/div[2]/div[2]/div[4]/product-card/div/button"));
                     break;
                  }
               case (int)PNGImageType.Popsockets:
                  {
                     Log.log.Info("-POP_SOCKET-FRONT-");
                     //ChromeHelper.SendKeysElement(By.Id("POP_SOCKET-FRONT"), shirt.ImagePath);

                     IWebElement webElement = ChromeHelper.GetElementWithWait(By.Id("POP_SOCKET-FRONT"));
                     webElement.SendKeys(shirt.ImagePath);
                     break;
                  }

               case (int)PNGImageType.iPhoneCase:
                  {
                     Log.log.Info("-Phone Case-");
                     //ChromeHelper.SendKeysElement(By.Id("PHONE_CASE_APPLE_IPHONE-BACK"), shirt.ImagePath);

                     IWebElement webElement = ChromeHelper.GetElementWithWait(By.Id("PHONE_CASE_APPLE_IPHONE-BACK"));
                     webElement.SendKeys(shirt.ImagePath);
                     break;
                  }
               case (int)PNGImageType.SamsungCase:
                  {
                     Log.log.Info("-Phone Case-");
                     //ChromeHelper.SendKeysElement(By.Id("PHONE_CASE_SAMSUNG_GALAXY-BACK"), shirt.ImagePath);
                     IWebElement webElement = ChromeHelper.GetElementWithWait(By.Id("PHONE_CASE_SAMSUNG_GALAXY-BACK"));
                     webElement.SendKeys(shirt.ImagePath);
                     break;
                  }
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
         ChromeHelper.Driver.Navigate().GoToUrl("https://merch.amazon.com/designs/new");
         Thread.Sleep(1000);
      }

      public void QuitDriver()
      {
         ChromeHelper.QuitDriver();
      }
   }
}
