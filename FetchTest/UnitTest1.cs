using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;

namespace FetchTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://sdetchallenge.fetch.com/");

            ValidatePage();

            GoldBars bars = new GoldBars(driver);
            bars.NumberOfBars = 9;
            bars.DivideList();

            //Divide and Conquer Algorithm
            bars.FillBoxes();
            bars.ClickWeighButton();
            bars.CheckResult();
            if (bars.Result == "=")
            {
                bars.LightBrick = 8;
                //bars.ProcessFinalResult();
            }
            else
            {
                while (bars.NumberOfBarsLeft > 2)
                {
                    bars.ProcessResult();
                    bars.ClickResetButton();
                    bars.FillBoxes();
                    bars.ClickWeighButton();
                    bars.CheckResult();
                }
                
                //bars.ProcessFinalResult();
                //bars.VerifyMessage();
                //bars.WriteResults();
            }
            bars.ProcessFinalResult();
            bars.VerifyMessage();
            bars.WriteResults();
            driver.Quit();

        }

        private void ValidatePage()
        {
            //throw new NotImplementedException();
        }
    }
}