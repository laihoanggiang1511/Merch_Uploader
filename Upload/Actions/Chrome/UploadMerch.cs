﻿using Common;
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
        }
        public bool LogIn()
        {
            // Log In 
            try
            {
                if (Driver != null)
                {
                    if (Driver.Url.Contains("www.amazon.com/ap/signin"))
                    {
                        Log.log.Info("------Start Log In----------");
                        if (!string.IsNullOrEmpty(_email) &&
                            ChromeHelper.GetElementWithWait(Driver, By.Id("ap_email"), 10) != null)
                        {
                            ChromeHelper.SendKeysElement(Driver, By.Id("ap_email"), _email);
                        }
                        ChromeHelper.SendKeysElement(Driver, By.Id("ap_password"), _password);
                        ChromeHelper.ClickElement(Driver, By.Id("signInSubmit"));
                        System.Threading.Thread.Sleep(2000);
                        if (Driver.Url.Contains("www.amazon.com/ap/accountfixup"))
                        {
                            ChromeHelper.ClickElement(Driver, By.Id("ap-account-fixup-phone-skip-link"));
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
                if (Driver != null && shirt != null)
                {
                    ChromeHelper.LogInCallBack = new LogIn(LogIn);
                    Log.log.Info($"-----------Start Upload-------------");

                    Driver.Navigate().GoToUrl("https://merch.amazon.com/designs/new");

                    //Set descriptions
                    Log.log.Info("---Descriptions---");
                    ChromeHelper.ClickElement(Driver, By.Id("acc-control-all"));

                    for (int i = 0; i <= 4; i++)
                    {
                        if (shirt.Languages.Count > i)
                        {
                            string rootXPath = $"/html/body/div[1]/div/app-root/div/ng-component/div/product-config-editor/product-text/ngb-accordion/div[{i + 1}]/div[2]/div/product-text-editor/div/div/";
                            if (!string.IsNullOrEmpty(shirt.Languages[i].BrandName))
                                ChromeHelper.SendKeysElement(Driver, By.XPath(rootXPath + "div[1]/div[3]/input"), shirt.Languages[i].BrandName);
                            if (!string.IsNullOrEmpty(shirt.Languages[i].Title))
                                ChromeHelper.SendKeysElement(Driver, By.XPath(rootXPath + "div[1]/div[2]/input"), shirt.Languages[i].Title);
                            if (!string.IsNullOrEmpty(shirt.Languages[i].FeatureBullet1))
                                ChromeHelper.SendKeysElement(Driver, By.XPath(rootXPath + "div[2]/div[2]/input"), shirt.Languages[i].FeatureBullet1);
                            if (!string.IsNullOrEmpty(shirt.Languages[i].FeatureBullet2))
                                ChromeHelper.SendKeysElement(Driver, By.XPath(rootXPath + "div[2]/div[3]/input"), shirt.Languages[i].FeatureBullet2);
                            if (!string.IsNullOrEmpty(shirt.Languages[i].Description))
                                ChromeHelper.SendKeysElement(Driver, By.XPath(rootXPath + "div[2]/div[4]/input"), shirt.Languages[i].Description);
                        }
                    }

                    // Select Products
                    if (!ChromeHelper.ClickElement(Driver, By.Id("select-marketplace-button")))
                        return false;
                    if (!SelectProduct(shirt))
                        return false;

                    while (ChromeHelper.GetElementWithWait(Driver, By.Id("select-marketplace-button"), 5) == null)
                    {
                        Driver.Navigate().GoToUrl("https://merch.amazon.com/designs/new");
                    }
                    // Upload .png files
                    if (!UploadFilePNG(shirt))
                        return false;

                    //Input detail
                    InputDetail(shirt);

                    // Submit
                    Log.log.Info("---Summit---");
                    ChromeHelper.ClickElement(Driver, By.Id("submit-button"));
                    System.Threading.Thread.Sleep(1000);
                    if (!ChromeHelper.ClickElement(Driver, By.XPath("/html/body/ngb-modal-window/div/div/ng-component/div[2]/div[2]/button[2]")))
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
                for (int i = 0; i < shirt.ShirtTypes.Count; i++)
                {
                    int row = (i) / 4 + 1;
                    int column = (i) % 4 + 1;
                    ShirtType s = shirt.ShirtTypes[i];
                    if (s.IsActive)
                    {
                        Log.log.Info($"---i={i}---");
                        // Edit Detail-Standard                
                        ChromeHelper.ClickElement(Driver, By.XPath($"/html/body/div[1]/div/app-root/div/ng-component/div/product-config-editor/div[2]/div[{row}]/div[{column}]/product-card/div/div[2]/button"));
                        // Choose Fit type
                        if (s.FitTypes != null && s.FitTypes.Count > 1)
                        {
                            for (int j = 0; j < s.FitTypes.Count; j++)
                            {
                                ChromeHelper.ClickCheckBox(Driver, $"/html/body/div[1]/div/app-root/div/ng-component/div/product-config-editor/div[2]/div[{row}]/product-editor/div/div[2]/div/div[2]/div[1]/dimension-editor/fit-type/div/div/label[{j + 1}]/flowcheckbox/span",
                                    s.FitTypes[j]);
                            }
                        }

                        // Select Color - non japanese types
                        if (s.TypeName != "StandardTShirt" &&
                           s.TypeName != "LongSleeveTShirt" &&
                           s.TypeName != "SweetShirt" &&
                           s.TypeName != "PullOverHoodie")
                        {
                            for (int j = 0; j < s.Colors.Count; j++)
                            {
                                Color color = s.Colors[j];
                                ChromeHelper.ClickCheckBox(Driver, $"/html/body/div[1]/div/app-root/div/ng-component/div/product-config-editor/div[2]/div[{row}]/product-editor/div/div[2]/div/div[2]/div[1]/dimension-editor/color/div/div/div[2]/div[{j + 1}]/colorcheckbox/span", color.IsActive);
                            }
                        }
                        else
                        {
                            List<Color> japaneseColor = new List<Color>();
                            List<Color> nonJapaneseColor = new List<Color>();

                            //Process for japanese colors
                            for (int j = 0; j < s.Colors.Count; j++)
                            {

                                Color col = s.Colors[j];
                                if (col.ColorName == "Brown" ||
                                    col.ColorName == "Dark Heather" ||
                                    col.ColorName == "Grass" ||
                                    col.ColorName == "Heather Blue" ||
                                    col.ColorName == "Silver" ||
                                    col.ColorName == "Slate")
                                {
                                    japaneseColor.Add(col);
                                }
                                else
                                {
                                    nonJapaneseColor.Add(col);
                                }
                            }
                            for (int j = 0; j < nonJapaneseColor.Count; j++)
                            {
                                ChromeHelper.ClickCheckBox(Driver, $"/html/body/div[1]/div/app-root/div/ng-component/div/product-config-editor/div[2]/div[{row}]/product-editor/div/div[2]/div/div[2]/div[1]/dimension-editor/color/div/div/div[2]/div[{j + 2}]/colorcheckbox/span", nonJapaneseColor[j].IsActive);
                            }
                            for (int j = 0; j < japaneseColor.Count; j++)
                            {
                                ChromeHelper.ClickCheckBox(Driver, $"/html/body/div[1]/div/app-root/div/ng-component/div/product-config-editor/div[2]/div[{row}]/product-editor/div/div[2]/div/div[2]/div[1]/dimension-editor/color/div/div/div[3]/div[{j + 2}]/colorcheckbox/span", japaneseColor[j].IsActive);
                            }
                        }

                        // Set Price
                        for (int j = 0; j < s.MarketPlaces.Count; j++)
                        {
                            if (s.MarketPlaces[j])
                            {
                                By by = By.XPath($"/html/body/div[1]/div/app-root/div/ng-component/div/product-config-editor/div[2]/div[{row}]/product-editor/div/div[2]/div/div[2]/div[2]/listing-details/div/price-editor[{j + 1}]/div/div/div[2]/div[1]/div[1]/input");
                                string price = s.Prices[j].ToString();
                                IWebElement element = ChromeHelper.GetElementWithWait(Driver, by);
                                element.SendKeys("1");
                                Clipboard.SetText(s.Prices[j].ToString());
                                OpenQA.Selenium.Interactions.Actions action = new OpenQA.Selenium.Interactions.Actions(Driver);
                                action.MoveToElement(element).Perform();
                                action.DoubleClick().Perform();
                                element.SendKeys(OpenQA.Selenium.Keys.Control + 'v');
                                //Helper.Paste();
                                //Helper.SendKeysElement(Driver, By.XPath($"/html/body/div[1]/div/app-root/div/ng-component/div/product-config-editor/div[2]/div[{row}]/product-editor/div/div[2]/div/div[2]/div[2]/listing-details/div/price-editor[{j + 1}]/div/div/div[2]/div[1]/div[1]/input"),
                                //                        s.Prices[j].ToString());

                            }
                        }
                        //Click again to close pallete
                        ChromeHelper.ClickElement(Driver, By.XPath($"/html/body/div[1]/div/app-root/div/ng-component/div/product-config-editor/div[2]/div[{row}]/div[{column}]/product-card/div/div[2]/button"));
                    }
                }
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
                if (Driver != null)
                {
                    //Check then Uncheck All Checkbox
                    ChromeHelper.ClickCheckBox(Driver, "/html/body/ngb-modal-window/div/div/ng-component/div[2]/div[1]/div/flowcheckbox/span", true);
                    ChromeHelper.ClickCheckBox(Driver, "/html/body/ngb-modal-window/div/div/ng-component/div[2]/div[1]/div/flowcheckbox/span", false);

                    for (int i = 0; i < shirt.ShirtTypes.Count; i++)
                    {
                        ShirtType sb = shirt.ShirtTypes[i];
                        Log.log.Info(sb.TypeName.ToString());
                        for (int j = 0; j < sb.MarketPlaces.Count; j++)
                        {
                            Log.log.Info($"i={i};j={j}");
                            if (!ChromeHelper.ClickCheckBox(Driver, $"/html/body/ngb-modal-window/div/div/ng-component/div[2]/div[2]/div/table/tbody/tr[{i + 3}]/td[{j + 2}]/flowcheckbox/span",
                                                    sb.IsActive && sb.MarketPlaces[j]))
                            {
                                return false;
                            }
                        }
                    }
                    // next button
                    Log.log.Info("---Next Button---");
                    if (!ChromeHelper.ClickElement(Driver, By.XPath("/html/body/ngb-modal-window/div/div/ng-component/div[3]/button")))
                    {
                        return false;
                    }
                    return true;
                }
                else return false;
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
                if (Driver != null)
                {
                    switch (shirt.ImageType)
                    {
                        case (int)PNGImageType.Standard_Front:
                            {
                                Log.log.Info("-STANDARD_TSHIRT-FRONT-");
                                IWebElement webElement = Driver.FindElement(By.Id("STANDARD_TSHIRT-FRONT"));
                                webElement.SendKeys(shirt.ImagePath);
                                break;
                            }
                        case (int)PNGImageType.Standard_Back:
                            {
                                ChromeHelper.ClickElement(Driver, By.XPath("/html/body/div[1]/div/app-root/div/ng-component/div/ng-component/div[2]/div[1]/div[1]/product-card/div/button"));
                                ChromeHelper.ClickElement(Driver, By.XPath("/html/body/div[1]/div/app-root/div/ng-component/div/ng-component/div[2]/div[1]/product-editor/div/div[2]/div/div[1]/product-asset-editor/div/div[2]/div/button[2]"));
                                IWebElement webElement = Driver.FindElement(By.Id("STANDARD_TSHIRT-BACK"));
                                webElement.SendKeys(shirt.ImagePath);
                                ChromeHelper.ClickElement(Driver, By.XPath("/html/body/div[1]/div/app-root/div/ng-component/div/ng-component/div[2]/div[1]/div[1]/product-card/div/button"));
                                break;
                            }
                        case (int)PNGImageType.Hoodie_Front:
                            {
                                IWebElement webElement = Driver.FindElement(By.Id("STANDARD_PULLOVER_HOODIE-FRONT"));
                                webElement.SendKeys(shirt.ImagePath);
                                break;
                            }
                        case (int)PNGImageType.Hoodie_Back:
                            {
                                Log.log.Info("-STANDARD_PULLOVER_HOODIE-BACK-");
                                ChromeHelper.ClickElement(Driver, By.XPath("/html/body/div[1]/div/app-root/div/ng-component/div/ng-component/div[2]/div[2]/div[4]/product-card/div/button"));
                                ChromeHelper.ClickElement(Driver, By.XPath("/html/body/div[1]/div/app-root/div/ng-component/div/ng-component/div[2]/div[2]/product-editor/div/div[2]/div/div[1]/product-asset-editor/div/div[2]/div/button[2]"));
                                IWebElement webElement = Driver.FindElement(By.Id("STANDARD_PULLOVER_HOODIE-BACK"));
                                webElement.SendKeys(shirt.ImagePath);
                                ChromeHelper.ClickElement(Driver, By.XPath("/html/body/div[1]/div/app-root/div/ng-component/div/ng-component/div[2]/div[2]/div[4]/product-card/div/button"));
                                break;
                            }
                        case (int)PNGImageType.Popsockets:
                            {
                                Log.log.Info("-POP_SOCKET-FRONT-");
                                IWebElement webElement = Driver.FindElement(By.Id("POP_SOCKET-FRONT"));
                                webElement.SendKeys(shirt.ImagePath);
                                break;
                            }

                        case (int)PNGImageType.iPhoneCase:
                            {
                                Log.log.Info("-Phone Case-");
                                IWebElement webElement = Driver.FindElement(By.Id("PHONE_CASE_APPLE_IPHONE-BACK"));
                                webElement.SendKeys(shirt.ImagePath);
                                break;
                            }
                        case (int)PNGImageType.SamsungCase:
                            {
                                Log.log.Info("-Phone Case-");
                                IWebElement webElement = Driver.FindElement(By.Id("PHONE_CASE_SAMSUNG_GALAXY-BACK"));
                                webElement.SendKeys(shirt.ImagePath);
                                break;
                            }
                    }

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
        

        public void OpenChrome(string userFolderPath = null)
        {
            try
            {

                Log.log.Info("---Open Chrome---");
                Cursor.Current = Cursors.WaitCursor;
                if (Driver != null)
                {
                    QuitDriver();
                }
                Driver = ChromeHelper.StartChromeWithOptions(userFolderPath);
            }
            catch (Exception ex)
            {
                Log.log.Fatal(ex);
                MessageBox.Show(ex.Message);
            }
        }


    }
}
