using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace FetchTest
{
    internal class GoldBars
    {
        public int NumberOfBars { get; set; }
        public int NumberOfBarsLeft { get; set; }
        public List<int> LeftBowl { get; set; }
        public List<int> RightBowl { get; set; }
        public string Result { get; set; }
        public string AlertText { get; set; }
        public int NumberOfWeighings { get; set; }
        public int LightBrick { get; set; }
        public IWebDriver Driver { get; set; }

        //IPage _page;

        public GoldBars(IWebDriver driver)
        {
            Driver = driver;
            LeftBowl = new List<int>();
            RightBowl = new List<int>();
            Result = string.Empty;
            AlertText = string.Empty;
        }

        public void DivideList()
        {
            var remainderOfSplit = NumberOfBars % 2;
            var NumberOfBarsMinusRemainder = NumberOfBars - remainderOfSplit;
            NumberOfBarsLeft = NumberOfBarsMinusRemainder;

            for (int i = 0; i < (NumberOfBarsMinusRemainder) / 2; i++)
            {
                LeftBowl.Add(i);
            }

            for (int i = (NumberOfBarsMinusRemainder) / 2; i < NumberOfBarsMinusRemainder; i++)
            {
                RightBowl.Add(i);
            }
        }

        public void FillBoxes()
        {
            FillLeftBowl();
            FillRightBowl();
        }

        public void ClickWeighButton()
        {
            var weighButton = Driver.FindElement(By.Id("weigh"));
            weighButton.Click();
            Thread.Sleep(2200);
            NumberOfWeighings++;
        }

        public void ClickResetButton()
        {
            var resetButton = Driver.FindElement(By.XPath("//button[contains(text(), 'Reset')]"));
            resetButton.Click();
        }

        public void CheckResult()
        {
            var resultButton = Driver.FindElement(By.XPath("//div[@class='result']/button[@id='reset']"));
            Result = resultButton.Text;
        }

        public void ProcessResult()
        {
            if (Result == ">")
            {
                SplitList("right");
            }
            else if (Result == "<")
            {
                SplitList("left");
            }
        }

        private void SplitList(string side)
        {
            var startingValue = 0;
            var endValue = 0;
            var barsPerBowl = 0;
            List<int> newLeftBowl = new List<int>();
            List<int> newRightBowl = new List<int>();
            if (side == "left")
            {
                NumberOfBarsLeft = LeftBowl.Count();
                startingValue = LeftBowl.First();
                barsPerBowl = NumberOfBarsLeft / 2;
                endValue = LeftBowl.First() + barsPerBowl;
            }
            else
            {
                NumberOfBarsLeft = RightBowl.Count();
                startingValue = RightBowl.First();
                barsPerBowl = NumberOfBarsLeft / 2;
                endValue = RightBowl.First() + barsPerBowl;
            }

            for (int i = startingValue; i < endValue; i++)
            {
                newLeftBowl.Add(i);
            }

            for (int i = endValue; i < endValue + barsPerBowl; i++)
            {
                newRightBowl.Add(i);
            }

            LeftBowl = newLeftBowl;
            RightBowl = newRightBowl;
        }

        public void ChooseLightBrick()
        {
            var barButton = Driver.FindElement(By.Id($"coin_{LightBrick}"));
            barButton.Click();
            Console.WriteLine($"The light brick is: {LightBrick}");
        }

        private void FillLeftBowl()
        {
            var boxCounter = 0;
            foreach (int bar in LeftBowl)
            {
                var textBox = Driver.FindElement(By.Id($"left_{boxCounter}"));
                textBox.SendKeys(bar.ToString());
                boxCounter++;
            }
        }

        private void FillRightBowl()
        {
            var boxCounter = 0;
            foreach (int bar in RightBowl)
            {
                var textBox = Driver.FindElement(By.Id($"right_{boxCounter}"));
                textBox.SendKeys(bar.ToString());
                boxCounter++;
            }
        }

        private void PrintWeighings()
        {
            Console.WriteLine("List Of Weighings");
            var list = Driver.FindElement(By.TagName("ol"));
            IList<IWebElement> weighings = list.FindElements(By.TagName("li"));
            foreach (IWebElement weigh in weighings)
            {
                Console.WriteLine(weigh.Text);
            }
        }

        public void ProcessFinalResult()
        {
            if (Result == ">")
            {
                LightBrick = RightBowl[0];
            }
            else if (Result == "<")
            {
                LightBrick = LeftBowl[0];
            }            
            ChooseLightBrick();
        }

        public void WriteResults()
        {
            PrintWeighings();
            Console.WriteLine($"Number of weighings: {NumberOfWeighings}");
            Console.WriteLine($"Alert Text : {AlertText}");
        }

        public void VerifyMessage()
        {            
            AlertText = Driver.SwitchTo().Alert().Text;
            Assert.AreEqual("Yay! You find it!", AlertText);
            Driver.SwitchTo().Alert().Accept();
        }
    }
}
