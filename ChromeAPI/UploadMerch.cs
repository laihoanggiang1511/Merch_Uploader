using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Upload.Definitions;
using Upload.Model;

namespace ChromeAPI
{
    public class UploadMerch
    {
        public bool Log_In(ChromeDriver driver, string password, string email = null)
        {
            // Log In 
            try
            {
                if (driver.Url.Contains("www.amazon.com/ap/signin"))
                {
                    if (!string.IsNullOrEmpty(email) && 
                        Utils.GetElementWithWait(driver, By.Id("ap_email"),10) != null)
                    {
                        Utils.SendKeysElement(driver, By.Id("ap_email"), email);
                    }
                    Utils.SendKeysElement(driver, By.Id("ap_password"), password);
                    Utils.ClickElement(driver, By.Id("signInSubmit"));
                    System.Threading.Thread.Sleep(3000);
                    return true;
                }
                else return true;
            }
            catch
            {
                return false;
            }
        }
        public bool Upload(ChromeDriver driver, Shirt shirt)
        {
            try
            {
                if (driver != null && shirt != null)
                {
                    driver.Navigate().GoToUrl("https://merch.amazon.com/designs/new");

                    while (Utils.GetElementWithWait(driver, By.Id("select-marketplace-button"),20) == null)
                    {
                        driver.Navigate().GoToUrl("https://merch.amazon.com/designs/new");
                    }

                    // Upload .png files
                    if (!UploadFilePNG(driver, shirt))
                        return false;
                    // Select Products
                    if (!Utils.ClickElement(driver, By.Id("select-marketplace-button")))
                        return false;
                    if (!SelectProduct(driver, shirt))
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
                            Utils.ClickElement(driver, By.XPath($"/html/body/div[1]/div/app-root/div/ng-component/div/ng-component/div[2]/div[{row}]/div[{column}]/product-card/div/button"));
                            // Choose Fit type
                            if (s.FitTypes != null && s.FitTypes.Length > 1)
                            {
                                for (int j = 0; j < s.FitTypes.Length; j++)
                                {
                                    Utils.ClickCheckBox(driver, $"/html/body/div[1]/div/app-root/div/ng-component/div/ng-component/div[2]/div[{row}]/product-editor/div/div[2]/div/div[2]/div[1]/dimension-editor" +
                                        $"/fit-type/div/div/label[{j + 1}]/flowcheckbox/span",
                                        s.FitTypes[j]);
                                }
                            }
                            // Select Color
                            for (int j = 0; j < s.Colors.Length; j++)
                            {
                                Color color = s.Colors[j];
                                Utils.ClickCheckBox(driver, $"/html/body/div[1]/div/app-root/div/ng-component/div/ng-component/div[2]/div[{row}]/product-editor/div/div[2]/div/div[2]/div[1]/dimension-editor/color/div/div" +
                                                                        $"/div[{j + 1}]/colorcheckbox/span", color.IsActive);
                            }
                            // Set Price
                            for (int j = 0; j < s.MarketPlaces.Length; j++)
                            {
                                if (s.MarketPlaces[j])
                                {
                                    //Utils.ClickElement(driver, By.XPath($"/html/body/div[1]/div/app-root/div/ng-component/div/ng-component/div[2]/div[{row}]/product-editor/div/div[2]/div/div[2]/div[2]/listing-details/div/price-editor[{j+1}]/div/div/div[2]/div[1]/div[1]/input"));
                                    Utils.SendKeysElement(driver, By.XPath($"/html/body/div[1]/div/app-root/div/ng-component/div/ng-component/div[2]/div[{row}]/product-editor/div/div[2]/div/div[2]/div[2]/listing-details/div/price-editor[{j + 1}]/div/div/div[2]/div[1]/div[1]/input"),
                                                            s.Prices[j].ToString());
                                }
                            }
                        }
                    }

                    // Set English Descriptions
                    Utils.SendKeysElement(driver, By.Id("designCreator-productEditor-brandName"), shirt.BrandName);
                    Utils.SendKeysElement(driver, By.Id("designCreator-productEditor-title"), shirt.DesignTitle);
                    Utils.SendKeysElement(driver, By.Id("designCreator-productEditor-featureBullet1"), shirt.FeatureBullet1);
                    Utils.SendKeysElement(driver, By.Id("designCreator-productEditor-featureBullet2"), shirt.FeatureBullet2);
                    Utils.SendKeysElement(driver, By.Id("designCreator-productEditor-description"), shirt.Description);

                    //Set German Description
                    Utils.ClickElement(driver, By.Id("/html/body/div[1]/div/app-root/div/ng-component/div/ng-component/product-text/div/ul/li[2]/a"));
                    Utils.SendKeysElement(driver, By.Id("designCreator-productEditor-brandName"), shirt.BrandNameGerman);
                    Utils.SendKeysElement(driver, By.Id("designCreator-productEditor-title"), shirt.DesignTitleGerman);
                    Utils.SendKeysElement(driver, By.Id("designCreator-productEditor-featureBullet1"), shirt.FeatureBullet1German);
                    Utils.SendKeysElement(driver, By.Id("designCreator-productEditor-featureBullet2"), shirt.FeatureBullet2German);
                    Utils.SendKeysElement(driver, By.Id("designCreator-productEditor-description"), shirt.DescriptionGerman);

                    // Submit
                    if (Utils.GetElementWithWait(driver, By.Id("submit-button"), 15) == null)
                        return false;
                    Utils.ClickElement(driver, By.Id("submit-button"));
                    Utils.ClickElement(driver, By.XPath("/html/body/ngb-modal-window/div/div/ng-component/div[3]/button[2]"));
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
        private bool SelectProduct(ChromeDriver driver, Shirt shirt)
        {
            try
            {
                for (int i = 0; i < shirt.ShirtTypes.Length; i++)
                {
                    ShirtBase sb = shirt.ShirtTypes[i];
                    for (int j = 0; j < sb.MarketPlaces.Length; j++)
                    {
                        
                        if(!Utils.ClickCheckBox(driver, $"/html/body/ngb-modal-window/div/div/ng-component/div[2]/div[2]/div/table/tbody/tr[{i + 3}]/td[{j + 2}]/flowcheckbox/span",
                                                sb.IsActive && sb.MarketPlaces[j]))
                        {
                            return false;
                        }
                    }
                }
                // next button
                if(!Utils.ClickElement(driver, By.XPath("/html/body/ngb-modal-window/div/div/ng-component/div[3]/button")))
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool UploadFilePNG(ChromeDriver driver, Shirt shirt)
        {
            try
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
                    Utils.ClickElement(driver, By.XPath("/html/body/div[1]/div/app-root/div/ng-component/div/ng-component/div[2]/div[1]/div[1]/product-card/div/button"));
                    Utils.ClickElement(driver, By.XPath("/html/body/div[1]/div/app-root/div/ng-component/div/ng-component/div[2]/div[1]/product-editor/div/div[2]/div/div[1]/product-asset-editor/div/div[2]/div/button[2]"));
                    IWebElement webElement = driver.FindElement(By.Id("STANDARD_TSHIRT-BACK"));
                    webElement.SendKeys(shirt.BackStdPath);
                    Utils.ClickElement(driver, By.XPath("/html/body/div[1]/div/app-root/div/ng-component/div/ng-component/div[2]/div[1]/div[1]/product-card/div/button"));

                }
                if (!string.IsNullOrEmpty(shirt.BackHoodiePath))
                {
                    Utils.ClickElement(driver, By.XPath("/html/body/div[1]/div/app-root/div/ng-component/div/ng-component/div[2]/div[2]/div[4]/product-card/div/button"));
                    Utils.ClickElement(driver, By.XPath("/html/body/div[1]/div/app-root/div/ng-component/div/ng-component/div[2]/div[2]/product-editor/div/div[2]/div/div[1]/product-asset-editor/div/div[2]/div/button[2]"));
                    IWebElement webElement = driver.FindElement(By.Id("STANDARD_PULLOVER_HOODIE-BACK"));
                    webElement.SendKeys(shirt.BackStdPath);
                    Utils.ClickElement(driver, By.XPath("/html/body/div[1]/div/app-root/div/ng-component/div/ng-component/div[2]/div[2]/div[4]/product-card/div/button"));
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
