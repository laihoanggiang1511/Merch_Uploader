using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Upload.Definitions;
using Upload.Model;

namespace ChromeAPI
{
    public class UploadMerch
    {
        public static ChromeDriver driver;
        public bool Log_In(string password, string email = null)
        {
            // Log In 
            try
            {
                if (driver != null)
                {
                    if (driver.Url.Contains("www.amazon.com/ap/signin"))
                    {
                        if (!string.IsNullOrEmpty(email) &&
                            Helper.GetElementWithWait(driver, By.Id("ap_email"), 10) != null)
                        {
                            Helper.SendKeysElement(driver, By.Id("ap_email"), email);
                        }
                        Helper.SendKeysElement(driver, By.Id("ap_password"), password);
                        Helper.ClickElement(driver, By.Id("signInSubmit"));
                        System.Threading.Thread.Sleep(3000);
                        return true;
                    }
                    else return true;
                }
                else return false;
            }
            catch
            {
                return false;
            }
        }
        public bool Upload(Shirt shirt)
        {
            try
            {
                if (driver != null && shirt != null)
                {
                    driver.Navigate().GoToUrl("https://merch.amazon.com/designs/new");

                    while (Helper.GetElementWithWait(driver, By.Id("select-marketplace-button"), 20) == null)
                    {
                        driver.Navigate().GoToUrl("https://merch.amazon.com/designs/new");
                    }

                    // Upload .png files
                    if (!UploadFilePNG(shirt))
                        return false;
                    // Select Products
                    if (!Helper.ClickElement(driver, By.Id("select-marketplace-button")))
                        return false;
                    if (!SelectProduct(shirt))
                        return false;

                    //Input detail
                    for (int i = 0; i < shirt.ShirtTypes.Length; i++)
                    {
                        int column = (i) / 4 + 1;
                        int row = (i) % 4 + 1;
                        ShirtBase s = shirt.ShirtTypes[i];
                        if (s.IsActive)
                        {
                            // Edit Detail-Standard
                            Helper.ClickElement(driver, By.XPath($"/html/body/div[1]/div/app-root/div/ng-component/div/ng-component/div[2]/div[{row}]/div[{column}]/product-card/div/button"));
                            // Choose Fit type
                            if (s.FitTypes != null && s.FitTypes.Length > 1)
                            {
                                for (int j = 0; j < s.FitTypes.Length; j++)
                                {
                                    Helper.ClickCheckBox(driver, $"/html/body/div[1]/div/app-root/div/ng-component/div/ng-component/div[2]/div[{row}]/product-editor/div/div[2]/div/div[2]/div[1]/dimension-editor" +
                                        $"/fit-type/div/div/label[{j + 1}]/flowcheckbox/span",
                                        s.FitTypes[j]);
                                }
                            }
                            // Select Color
                            for (int j = 0; j < s.Colors.Length; j++)
                            {
                                Color color = s.Colors[j];
                                Helper.ClickCheckBox(driver, $"/html/body/div[1]/div/app-root/div/ng-component/div/ng-component/div[2]/div[{row}]/product-editor/div/div[2]/div/div[2]/div[1]/dimension-editor/color/div/div" +
                                                                        $"/div[{j + 1}]/colorcheckbox/span", color.IsActive);
                            }
                            // Set Price
                            for (int j = 0; j < s.MarketPlaces.Length; j++)
                            {
                                if (s.MarketPlaces[j])
                                {
                                    //Utils.ClickElement(driver, By.XPath($"/html/body/div[1]/div/app-root/div/ng-component/div/ng-component/div[2]/div[{row}]/product-editor/div/div[2]/div/div[2]/div[2]/listing-details/div/price-editor[{j+1}]/div/div/div[2]/div[1]/div[1]/input"));
                                    Helper.SendKeysElement(driver, By.XPath($"/html/body/div[1]/div/app-root/div/ng-component/div/ng-component/div[2]/div[{row}]/product-editor/div/div[2]/div/div[2]/div[2]/listing-details/div/price-editor[{j + 1}]/div/div/div[2]/div[1]/div[1]/input"),
                                                            s.Prices[j].ToString());
                                }
                            }
                        }
                    }

                    // Set English Descriptions
                    Helper.SendKeysElement(driver, By.Id("designCreator-productEditor-brandName"), shirt.BrandName);
                    Helper.SendKeysElement(driver, By.Id("designCreator-productEditor-title"), shirt.DesignTitle);
                    Helper.SendKeysElement(driver, By.Id("designCreator-productEditor-featureBullet1"), shirt.FeatureBullet1);
                    Helper.SendKeysElement(driver, By.Id("designCreator-productEditor-featureBullet2"), shirt.FeatureBullet2);
                    Helper.SendKeysElement(driver, By.Id("designCreator-productEditor-description"), shirt.Description);

                    //Set German Description
                    Helper.ClickElement(driver, By.Id("/html/body/div[1]/div/app-root/div/ng-component/div/ng-component/product-text/div/ul/li[2]/a"));
                    Helper.SendKeysElement(driver, By.Id("designCreator-productEditor-brandName"), shirt.BrandNameGerman);
                    Helper.SendKeysElement(driver, By.Id("designCreator-productEditor-title"), shirt.DesignTitleGerman);
                    Helper.SendKeysElement(driver, By.Id("designCreator-productEditor-featureBullet1"), shirt.FeatureBullet1German);
                    Helper.SendKeysElement(driver, By.Id("designCreator-productEditor-featureBullet2"), shirt.FeatureBullet2German);
                    Helper.SendKeysElement(driver, By.Id("designCreator-productEditor-description"), shirt.DescriptionGerman);

                    // Submit
                    if (Helper.GetElementWithWait(driver, By.Id("submit-button"), 15) == null)
                        return false;
                    Helper.ClickElement(driver, By.Id("submit-button"));
                    Helper.ClickElement(driver, By.XPath("/html/body/ngb-modal-window/div/div/ng-component/div[3]/button[2]"));
                    System.Threading.Thread.Sleep(3000);
                    return true;
                }
                else return false;
            }
            catch
            {
                return false;
            }
        }
        private bool SelectProduct(Shirt shirt)
        {
            try
            {
                if (driver != null)
                {
                    for (int i = 0; i < shirt.ShirtTypes.Length; i++)
                    {
                        ShirtBase sb = shirt.ShirtTypes[i];
                        for (int j = 0; j < sb.MarketPlaces.Length; j++)
                        {
                            if (!Helper.ClickCheckBox(driver, $"/html/body/ngb-modal-window/div/div/ng-component/div[2]/div[2]/div/table/tbody/tr[{i + 3}]/td[{j + 2}]/flowcheckbox/span",
                                                    sb.IsActive && sb.MarketPlaces[j]))
                            {
                                return false;
                            }
                        }
                    }
                    // next button
                    if (!Helper.ClickElement(driver, By.XPath("/html/body/ngb-modal-window/div/div/ng-component/div[3]/button")))
                    {
                        return false;
                    }
                    return true;
                }
                else return false;
            }
            catch
            {
                return false;
            }
        }

        private bool UploadFilePNG(Shirt shirt)
        {
            try
            {
                if (driver != null)
                {
                    if (!string.IsNullOrEmpty(shirt.FrontStdPath))
                    {
                        IWebElement webElement = driver.FindElement(By.Id("STANDARD_TSHIRT-FRONT"));
                        webElement.SendKeys(shirt.FrontStdPath);
                    }
                    if (!string.IsNullOrEmpty(shirt.FrontHoodiePath))
                    {
                        IWebElement webElement = driver.FindElement(By.Id("STANDARD_PULLOVER_HOODIE-FRONT"));
                        webElement.SendKeys(shirt.FrontHoodiePath);
                    }
                    if (!string.IsNullOrEmpty(shirt.PopSocketsGripPath))
                    {
                        IWebElement webElement = driver.FindElement(By.Id("POP_SOCKET-FRONT"));
                        webElement.SendKeys(shirt.PopSocketsGripPath);
                    }
                    if (!string.IsNullOrEmpty(shirt.BackStdPath))
                    {
                        Helper.ClickElement(driver, By.XPath("/html/body/div[1]/div/app-root/div/ng-component/div/ng-component/div[2]/div[1]/div[1]/product-card/div/button"));
                        Helper.ClickElement(driver, By.XPath("/html/body/div[1]/div/app-root/div/ng-component/div/ng-component/div[2]/div[1]/product-editor/div/div[2]/div/div[1]/product-asset-editor/div/div[2]/div/button[2]"));
                        IWebElement webElement = driver.FindElement(By.Id("STANDARD_TSHIRT-BACK"));
                        webElement.SendKeys(shirt.BackStdPath);
                        Helper.ClickElement(driver, By.XPath("/html/body/div[1]/div/app-root/div/ng-component/div/ng-component/div[2]/div[1]/div[1]/product-card/div/button"));

                    }
                    if (!string.IsNullOrEmpty(shirt.BackHoodiePath))
                    {
                        Helper.ClickElement(driver, By.XPath("/html/body/div[1]/div/app-root/div/ng-component/div/ng-component/div[2]/div[2]/div[4]/product-card/div/button"));
                        Helper.ClickElement(driver, By.XPath("/html/body/div[1]/div/app-root/div/ng-component/div/ng-component/div[2]/div[2]/product-editor/div/div[2]/div/div[1]/product-asset-editor/div/div[2]/div/button[2]"));
                        IWebElement webElement = driver.FindElement(By.Id("STANDARD_PULLOVER_HOODIE-BACK"));
                        webElement.SendKeys(shirt.BackStdPath);
                        Helper.ClickElement(driver, By.XPath("/html/body/div[1]/div/app-root/div/ng-component/div/ng-component/div[2]/div[2]/div[4]/product-card/div/button"));
                    }
                    return true;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        public static void QuitDriver()
        {
            try
            {
                if (driver != null)
                {
                    driver.Close();
                    driver.Quit();
                    driver = null;
                }
                //Process[] procs = Process.GetProcessesByName("chromedriver.exe");
                //foreach (Process proc in procs)
                //{
                //    proc.Kill();
                //}
            }
            catch
            {
            }
        }

        public static void OpenChrome(string userFolderPath = null, bool next = true)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (driver == null)
                {
                    if (userFolderPath != null && userFolderPath != string.Empty)
                    {
                        List<string> lst = new List<string>()
                        {
                            "enable-automation",
                        };

                        ChromeOptions chrOption = new ChromeOptions();
                        chrOption.AddArguments("user-data-dir=" + userFolderPath);
                        var chromeDriverService = ChromeDriverService.CreateDefaultService();
                        chromeDriverService.HideCommandPromptWindow = true;
                        driver = new ChromeDriver(chromeDriverService, chrOption);
                    }
                    else
                    {
                        ChromeOptions chrOption = new ChromeOptions();
                        //chrOption.AddAdditionalCapability("useAutomationExtension", false);
                        //chrOption.AddAdditionalCapability("excludeSwitches", "enable-automation");
                        var chromeDriverService = ChromeDriverService.CreateDefaultService();
                        chromeDriverService.HideCommandPromptWindow = true;
                        driver = new ChromeDriver(chromeDriverService, chrOption);
                    }
                }
                else
                {
                    driver.Navigate().GoToUrl("https://google.com/");
                }
            }
            catch (Exception ex)
            {
                if (driver != null)
                {
                    QuitDriver();
                    if (next == true)
                    {
                        OpenChrome(userFolderPath, false);
                    }
                }
                else
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

    }
}
