using System.Configuration;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace SimplyHealthTest.Utilities
{
    public abstract class DriverClassBase
    {
        //private static IWebDriver driver;
        public ThreadLocal<IWebDriver> driver = new ThreadLocal<IWebDriver>();
        public  IWebDriver Driver { get => driver.Value; set => driver.Value = value; }

        [SetUp]
        public void StartBrowser()
        {
            
            InitialiseWebDriver("Chrome");
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            Driver.Manage().Window.Maximize();
            Driver.Url = "https://www.simplyhealth.co.uk";
            
            //wait for the Accept Cookies to be present and click Accept
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.
                ElementExists(By.XPath("//*[@id=\"onetrust-accept-btn-handler\"]")));
            Driver.FindElement(By.XPath("//*[@id=\"onetrust-accept-btn-handler\"]")).Click();

        }
        private void InitialiseWebDriver(string Browsername)
        {

            switch (Browsername)
            {
                case "Edge":
                    new DriverManager().SetUpDriver(new EdgeConfig());
                    Driver = new EdgeDriver();
                    break;
                case "Chrome":
                    new DriverManager().SetUpDriver(new ChromeConfig());
                    Driver = new ChromeDriver();
                    break;
                case "Firefox":
                    new DriverManager().SetUpDriver(new FirefoxConfig());
                    Driver = new FirefoxDriver();
                    break;
                default: throw new ArgumentException(Browsername + "Instance driver is not initiated");
            }


        }

        [TearDown]
        public void CloseBowser()
        {
            Driver.Quit();
        }
    }
}