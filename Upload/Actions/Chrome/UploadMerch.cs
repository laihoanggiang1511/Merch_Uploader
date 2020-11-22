﻿using Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Upload.ViewModel;

namespace Upload.Actions.Chrome
{
    public class UploadMerch
    {
        public static ChromeDriver driver;
        public static bool stopUpload;
        private string email = string.Empty;
        private string password = string.Empty;
        public UploadMerch(string password, string email)
        {
            if (!string.IsNullOrEmpty(password))
                this.password = password;
            if (!string.IsNullOrEmpty(email))
                this.email = email;
        }

        public bool Log_In()
        {
            // Log In 
            try
            {
                if (driver != null)
                {
                    if (driver.Url.Contains("www.amazon.com/ap/signin"))
                    {
                        Log.log.Info("------Start Log In----------");
                        if (!string.IsNullOrEmpty(email) &&
                            Helper.GetElementWithWait(driver, By.Id("ap_email"), 10) != null)
                        {
                            Helper.SendKeysElement(driver, By.Id("ap_email"), email);
                        }
                        Helper.SendKeysElement(driver, By.Id("ap_password"), password);
                        Helper.ClickElement(driver, By.Id("signInSubmit"));
                        System.Threading.Thread.Sleep(2000);
                        if (driver.Url.Contains("www.amazon.com/ap/accountfixup"))
                        {
                            Helper.ClickElement(driver, By.Id("ap-account-fixup-phone-skip-link"));
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
                if (driver != null && shirt != null)
                {
                    Helper.LogInCallBack = new LogIn(Log_In);
                    Log.log.Info($"-----------Start Upload-------------");

                    driver.Navigate().GoToUrl("https://merch.amazon.com/designs/new");

                    while (Helper.GetElementWithWait(driver, By.Id("select-marketplace-button"), 20) == null)
                    {
                        driver.Navigate().GoToUrl("https://merch.amazon.com/designs/new");
                    }
                    // Upload .png files
                    if (!UploadFilePNG(shirt))
                        return false;

                    //Set descriptions
                    Log.log.Info("---Descriptions---");
                    Helper.ClickElement(driver, By.Id("acc-control-all"));

                    for (int i = 0; i <= 4; i++)
                    {
                        if (shirt.Languages.Count > i)
                        {
                            string rootXPath = $"/html/body/div[1]/div/app-root/div/ng-component/div/product-config-editor/product-text/ngb-accordion/div[{i + 1}]/div[2]/div/product-text-editor/div/div/";
                            if (!string.IsNullOrEmpty(shirt.Languages[i].BrandName))
                                Helper.SendKeysElement(driver, By.XPath(rootXPath + "div[1]/div[3]/input"), shirt.Languages[i].BrandName);
                            if (!string.IsNullOrEmpty(shirt.Languages[i].Title))
                                Helper.SendKeysElement(driver, By.XPath(rootXPath + "div[1]/div[2]/input"), shirt.Languages[i].Title);
                            if (!string.IsNullOrEmpty(shirt.Languages[i].FeatureBullet1))
                                Helper.SendKeysElement(driver, By.XPath(rootXPath + "div[2]/div[2]/input"), shirt.Languages[i].FeatureBullet1);
                            if (!string.IsNullOrEmpty(shirt.Languages[i].FeatureBullet2))
                                Helper.SendKeysElement(driver, By.XPath(rootXPath + "div[2]/div[3]/input"), shirt.Languages[i].FeatureBullet2);
                            if (!string.IsNullOrEmpty(shirt.Languages[i].Description))
                                Helper.SendKeysElement(driver, By.XPath(rootXPath + "div[2]/div[4]/input"), shirt.Languages[i].Description);
                        }
                    }

                    // Select Products
                    if (!Helper.ClickElement(driver, By.Id("select-marketplace-button")))
                        return false;
                    if (!SelectProduct(shirt))
                        return false;

                    //Input detail
                    InputDetail(shirt);

                    ////Set Other languages Description
                    //if (shirt.ShirtTypes.FirstOrDefault(x => (x.MarketPlaces.Length > 2 && x.MarketPlaces[2] == true)) != null)
                    //{
                    //    string germanXPath = "/html/body/div[1]/div/app-root/div/ng-component/div/div/editor/product-text/ngb-accordion/div[2]/div[2]/div/product-text-editor/div/div/";
                    //    //Helper.ClickElement(driver, By.Id("/html/body/div[1]/div/app-root/div/ng-component/div/ng-component/product-text/div/ul/li[2]/a"));
                    //    if (!string.IsNullOrEmpty(shirt.BrandNameGerman))
                    //        Helper.SendKeysElement(driver, By.XPath(germanXPath + "div[1]/div[3]/input"), shirt.BrandNameGerman);
                    //    if (!string.IsNullOrEmpty(shirt.DesignTitleGerman))
                    //        Helper.SendKeysElement(driver, By.XPath(germanXPath + "div[1]/div[2]/input"), shirt.DesignTitleGerman);
                    //    if (!string.IsNullOrEmpty(shirt.FeatureBullet1German))
                    //        Helper.SendKeysElement(driver, By.XPath(germanXPath + "div[2]/div[2]/input"), shirt.FeatureBullet1German);
                    //    if (!string.IsNullOrEmpty(shirt.FeatureBullet2German))
                    //        Helper.SendKeysElement(driver, By.XPath(germanXPath + "div[2]/div[3]/input"), shirt.FeatureBullet2German);
                    //    if (!string.IsNullOrEmpty(shirt.DescriptionGerman))
                    //        Helper.SendKeysElement(driver, By.XPath(germanXPath + "div[2]/div[4]/textarea"), shirt.DescriptionGerman);
                    //}

                    // Submit
                    Log.log.Info("---Summit---");
                    if (Helper.GetElementWithWait(driver, By.Id("submit-button"), 15) == null)
                        return false;
                    Helper.ClickElement(driver, By.Id("submit-button"));
                    Helper.ClickElement(driver, By.XPath("/html/body/ngb-modal-window/div/div/ng-component/div[3]/button[2]"));
                    System.Threading.Thread.Sleep(3000);
                    Log.log.Info("-----------End Upload-----------");
                    return true;
                }
                else return false;
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
                        Helper.ClickElement(driver, By.XPath($"/html/body/div[1]/div/app-root/div/ng-component/div/product-config-editor/div[2]/div[{row}]/div[{column}]/product-card/div/div[2]/button"));
                        // Choose Fit type
                        if (s.FitTypes != null && s.FitTypes.Count > 1)
                        {
                            for (int j = 0; j < s.FitTypes.Count; j++)
                            {
                                Helper.ClickCheckBox(driver, $"/html/body/div[1]/div/app-root/div/ng-component/div/product-config-editor/div[2]/div[{row}]/product-editor/div/div[2]/div/div[2]/div[1]/dimension-editor/fit-type/div/div/label[{j + 1}]/flowcheckbox/span",
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
                                Helper.ClickCheckBox(driver, $"/html/body/div[1]/div/app-root/div/ng-component/div/product-config-editor/div[2]/div[{row}]/product-editor/div/div[2]/div/div[2]/div[1]/dimension-editor/color/div/div/div[2]/div[{j + 1}]/colorcheckbox/span", color.IsActive);
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
                                Helper.ClickCheckBox(driver, $"/html/body/div[1]/div/app-root/div/ng-component/div/product-config-editor/div[2]/div[{row}]/product-editor/div/div[2]/div/div[2]/div[1]/dimension-editor/color/div/div/div[2]/div[{j + 2}]/colorcheckbox/span", nonJapaneseColor[j].IsActive);
                            }
                            for (int j = 0; j < japaneseColor.Count; j++)
                            {
                                Helper.ClickCheckBox(driver, $"/html/body/div[1]/div/app-root/div/ng-component/div/product-config-editor/div[2]/div[{row}]/product-editor/div/div[2]/div/div[2]/div[1]/dimension-editor/color/div/div/div[3]/div[{j + 2}]/colorcheckbox/span", japaneseColor[j].IsActive);
                            }
                        }

                        // Set Price
                        for (int j = 0; j < s.MarketPlaces.Count; j++)
                        {
                            if (s.MarketPlaces[j])
                            {
                                string[] strPrice = s.Prices[j].ToString().Split('.');
                                if (strPrice.Length > 0)
                                {
                                    //Log.log.Info(strPrice[0]);
                                    //Helper.SendKeysElement(driver, By.XPath($"/html/body/div[1]/div/app-root/div/ng-component/div/product-config-editor/div[2]/div[{row}]/product-editor/div/div[2]/div/div[2]/div[2]/listing-details/div/price-editor[{j + 1}]/div/div/div[2]/div[1]/div[1]/input"),
                                    //                    strPrice[0]);
                                    //if (strPrice.Length > 1)
                                    //{
                                    //    Log.log.Info(strPrice[1]);

                                    //    Helper.SendKeysElement(driver, By.XPath($"/html/body/div[1]/div/app-root/div/ng-component/div/product-config-editor/div[2]/div[{row}]/product-editor/div/div[2]/div/div[2]/div[2]/listing-details/div/price-editor[{j + 1}]/div/div/div[2]/div[1]/div[1]/input"),
                                    //                        ".");
                                    //    Helper.SendKeysElement(driver, By.XPath($"/html/body/div[1]/div/app-root/div/ng-component/div/product-config-editor/div[2]/div[{row}]/product-editor/div/div[2]/div/div[2]/div[2]/listing-details/div/price-editor[{j + 1}]/div/div/div[2]/div[1]/div[1]/input"),
                                    //                        strPrice[1]);
                                    //}
                                }

                            }
                        }
                        //Click again to close pallete
                        Helper.ClickElement(driver, By.XPath($"/html/body/div[1]/div/app-root/div/ng-component/div/product-config-editor/div[2]/div[{row}]/div[{column}]/product-card/div/div[2]/button"));
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
                if (driver != null)
                {
                    //Check then Uncheck All Checkbox
                    Helper.ClickCheckBox(driver, "/html/body/ngb-modal-window/div/div/ng-component/div[2]/div[1]/div/flowcheckbox/span", true);
                    Helper.ClickCheckBox(driver, "/html/body/ngb-modal-window/div/div/ng-component/div[2]/div[1]/div/flowcheckbox/span", false);

                    for (int i = 0; i < shirt.ShirtTypes.Count; i++)
                    {
                        ShirtType sb = shirt.ShirtTypes[i];
                        Log.log.Info(sb.TypeName.ToString());
                        for (int j = 0; j < sb.MarketPlaces.Count; j++)
                        {
                            Log.log.Info($"i={i};j={j}");
                            if (!Helper.ClickCheckBox(driver, $"/html/body/ngb-modal-window/div/div/ng-component/div[2]/div[2]/div/table/tbody/tr[{i + 3}]/td[{j + 2}]/flowcheckbox/span",
                                                    sb.IsActive && sb.MarketPlaces[j]))
                            {
                                return false;
                            }
                        }
                    }
                    // next button
                    Log.log.Info("---Next Button---");
                    if (!Helper.ClickElement(driver, By.XPath("/html/body/ngb-modal-window/div/div/ng-component/div[3]/button")))
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
                if (driver != null)
                {
                    switch (shirt.ImageType)
                    {
                        case (int)PNGImageType.Standard_Front:
                            {
                                Log.log.Info("-STANDARD_TSHIRT-FRONT-");
                                IWebElement webElement = driver.FindElement(By.Id("STANDARD_TSHIRT-FRONT"));
                                webElement.SendKeys(shirt.ImagePath);
                                break;
                            }
                        case (int)PNGImageType.Standard_Back:
                            {
                                Helper.ClickElement(driver, By.XPath("/html/body/div[1]/div/app-root/div/ng-component/div/ng-component/div[2]/div[1]/div[1]/product-card/div/button"));
                                Helper.ClickElement(driver, By.XPath("/html/body/div[1]/div/app-root/div/ng-component/div/ng-component/div[2]/div[1]/product-editor/div/div[2]/div/div[1]/product-asset-editor/div/div[2]/div/button[2]"));
                                IWebElement webElement = driver.FindElement(By.Id("STANDARD_TSHIRT-BACK"));
                                webElement.SendKeys(shirt.ImagePath);
                                Helper.ClickElement(driver, By.XPath("/html/body/div[1]/div/app-root/div/ng-component/div/ng-component/div[2]/div[1]/div[1]/product-card/div/button"));
                                break;
                            }
                        case (int)PNGImageType.Hoodie_Front:
                            {
                                IWebElement webElement = driver.FindElement(By.Id("STANDARD_PULLOVER_HOODIE-FRONT"));
                                webElement.SendKeys(shirt.ImagePath);
                                break;
                            }
                        case (int)PNGImageType.Hoodie_Back:
                            {
                                Log.log.Info("-STANDARD_PULLOVER_HOODIE-BACK-");
                                Helper.ClickElement(driver, By.XPath("/html/body/div[1]/div/app-root/div/ng-component/div/ng-component/div[2]/div[2]/div[4]/product-card/div/button"));
                                Helper.ClickElement(driver, By.XPath("/html/body/div[1]/div/app-root/div/ng-component/div/ng-component/div[2]/div[2]/product-editor/div/div[2]/div/div[1]/product-asset-editor/div/div[2]/div/button[2]"));
                                IWebElement webElement = driver.FindElement(By.Id("STANDARD_PULLOVER_HOODIE-BACK"));
                                webElement.SendKeys(shirt.ImagePath);
                                Helper.ClickElement(driver, By.XPath("/html/body/div[1]/div/app-root/div/ng-component/div/ng-component/div[2]/div[2]/div[4]/product-card/div/button"));
                                break;
                            }
                        case (int)PNGImageType.Popsockets:
                            {
                                Log.log.Info("-POP_SOCKET-FRONT-");
                                IWebElement webElement = driver.FindElement(By.Id("POP_SOCKET-FRONT"));
                                webElement.SendKeys(shirt.ImagePath);
                                break;
                            }

                        case (int)PNGImageType.iPhoneCase:
                            {
                                Log.log.Info("-Phone Case-");
                                IWebElement webElement = driver.FindElement(By.Id("PHONE_CASE_APPLE_IPHONE-BACK"));
                                webElement.SendKeys(shirt.ImagePath);
                                break;
                            }
                        case (int)PNGImageType.SamsungCase:
                            {
                                Log.log.Info("-Phone Case-");
                                IWebElement webElement = driver.FindElement(By.Id("PHONE_CASE_SAMSUNG_GALAXY-BACK"));
                                webElement.SendKeys(shirt.ImagePath);
                                break;
                            }
                    }
                    //if (!string.IsNullOrEmpty(shirt.FrontStdPath))
                    //{
                    //    Log.log.Info("-STANDARD_TSHIRT-FRONT-");
                    //    IWebElement webElement = driver.FindElement(By.Id("STANDARD_TSHIRT-FRONT"));
                    //    webElement.SendKeys(shirt.FrontStdPath);
                    //}
                    //if (!string.IsNullOrEmpty(shirt.FrontHoodiePath))
                    //{
                    //    Log.log.Info("-STANDARD_PULLOVER_HOODIE-FRONT-");
                    //    IWebElement webElement = driver.FindElement(By.Id("STANDARD_PULLOVER_HOODIE-FRONT"));
                    //    webElement.SendKeys(shirt.FrontHoodiePath);
                    //}
                    //if (!string.IsNullOrEmpty(shirt.PopSocketsGripPath))
                    //{
                    //    Log.log.Info("-POP_SOCKET-FRONT-");
                    //    IWebElement webElement = driver.FindElement(By.Id("POP_SOCKET-FRONT"));
                    //    webElement.SendKeys(shirt.PopSocketsGripPath);
                    //}
                    //if (!string.IsNullOrEmpty(shirt.BackStdPath))
                    //{
                    //    Log.log.Info("-STANDARD_TSHIRT-BACK-");
                    //    Helper.ClickElement(driver, By.XPath("/html/body/div[1]/div/app-root/div/ng-component/div/ng-component/div[2]/div[1]/div[1]/product-card/div/button"));
                    //    Helper.ClickElement(driver, By.XPath("/html/body/div[1]/div/app-root/div/ng-component/div/ng-component/div[2]/div[1]/product-editor/div/div[2]/div/div[1]/product-asset-editor/div/div[2]/div/button[2]"));
                    //    IWebElement webElement = driver.FindElement(By.Id("STANDARD_TSHIRT-BACK"));
                    //    webElement.SendKeys(shirt.BackStdPath);
                    //    Helper.ClickElement(driver, By.XPath("/html/body/div[1]/div/app-root/div/ng-component/div/ng-component/div[2]/div[1]/div[1]/product-card/div/button"));

                    //}
                    //if (!string.IsNullOrEmpty(shirt.BackHoodiePath))
                    //{
                    //    Log.log.Info("-STANDARD_PULLOVER_HOODIE-BACK-");
                    //    Helper.ClickElement(driver, By.XPath("/html/body/div[1]/div/app-root/div/ng-component/div/ng-component/div[2]/div[2]/div[4]/product-card/div/button"));
                    //    Helper.ClickElement(driver, By.XPath("/html/body/div[1]/div/app-root/div/ng-component/div/ng-component/div[2]/div[2]/product-editor/div/div[2]/div/div[1]/product-asset-editor/div/div[2]/div/button[2]"));
                    //    IWebElement webElement = driver.FindElement(By.Id("STANDARD_PULLOVER_HOODIE-BACK"));
                    //    webElement.SendKeys(shirt.BackStdPath);
                    //    Helper.ClickElement(driver, By.XPath("/html/body/div[1]/div/app-root/div/ng-component/div/ng-component/div[2]/div[2]/div[4]/product-card/div/button"));
                    //}
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
            try
            {
                Log.log.Info("---Quit Driver---");
                if (driver != null)
                    driver.Close();
                driver = null;
            }
            catch (Exception ex)
            {
                Log.log.Fatal(ex);
            }
            finally
            {
                if (driver != null)
                {
                    driver.Quit();
                }
            }

        }

        public void OpenChrome(string userFolderPath = null)
        {
            try
            {
                Log.log.Info("---Open Chrome---");
                Cursor.Current = Cursors.WaitCursor;
                if (driver == null)
                {
                    driver = StartChromeWithOptions(userFolderPath);
                }
                else
                {
                    QuitDriver();
                    driver = StartChromeWithOptions(userFolderPath);
                }
            }
            catch (Exception ex)
            {
                Log.log.Fatal(ex);
                MessageBox.Show(ex.Message);
            }
        }

        public ChromeDriver StartChromeWithOptions(string userFolderPath)
        {
            ChromeDriver cDriver = null;
            if (!string.IsNullOrEmpty(userFolderPath) && Directory.Exists(userFolderPath))
            {
                ChromeOptions chrOption = new ChromeOptions();
                chrOption.AddArguments("user-data-dir=" + userFolderPath);
                var chromeDriverService = ChromeDriverService.CreateDefaultService();
                chromeDriverService.HideCommandPromptWindow = true;
                cDriver = new ChromeDriver(chromeDriverService, chrOption);
            }
            else
            {
                ChromeOptions chrOption = new ChromeOptions();
                var chromeDriverService = ChromeDriverService.CreateDefaultService();
                chromeDriverService.HideCommandPromptWindow = true;
                cDriver = new ChromeDriver(chromeDriverService, chrOption);
            }
            return cDriver;
        }
    }
}
