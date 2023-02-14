using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using SimplyHealthTest.Utilities;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace SimplyHealthTest 
{
    //Run all Tests Parallel
    [Parallelizable(ParallelScope.Children)]
    public class E2ETests : DriverClassBase
    { 

        [Test]
        
        public void RegisterUser() 
        {

            Driver.FindElement(By.LinkText("My account")).Click();

            //wait for Sign up to be present and click Accept
            WebDriverWait waitLink = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            waitLink.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.
                ElementExists(By.LinkText("Sign up")));
            Driver.FindElement(By.LinkText("Sign up")).Click();

            //Fill the Registration Form and submit
            RegistrationForm("ven@vetcha.co.uk", "ven@vetcha.co.uk", "Str0ngpassword1!", "Str0ngpassword1!");
            
            //Assert Title
            string expectedTitle = Driver.Title;
            Assert.That(Driver.Title.Contains("Simplyhealth - Consumer Portal"), Is.EqualTo(true));

        }

        private void RegistrationForm(string email, string confirmEmail, string password, string confirmPassword)
        {
            Driver.FindElement(By.Id("email")).SendKeys(email);
            Driver.FindElement(By.Id("confirmEmail")).SendKeys(confirmEmail);
            Driver.FindElement(By.Id("password")).SendKeys(password);
            Driver.FindElement(By.CssSelector(".FieldElement_form_password_field_container__qDKRE:nth-child(3) .FieldElement_password_toggle_icon__RpMrY")).Click();
            Driver.FindElement(By.Id("confirmPassword")).SendKeys(confirmPassword);
            Driver.FindElement(By.CssSelector("button[type='submit']")).Click();
        }

        [Test]

        public void FeedbackPage()
        {
            //wait for the Feedback to be available to click
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.
                ElementExists(By.XPath("//a[@class='edr_go']")));
            Driver.FindElement(By.XPath("//a[@class='edr_go']")).Click();

            //Switch to child Window
            Assert.That(Driver.WindowHandles, Has.Count.EqualTo(2));
            String childWindowName = Driver.WindowHandles[1];
            Driver.SwitchTo().Window(childWindowName);

            //Fill the Feedback for and submit
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.
                ElementExists(By.XPath("(//span[normalize-space()='Positive feedback'])[1]")));
            
            //Select Positive Feedback
            Driver.FindElement(By.XPath("(//span[normalize-space()='Positive feedback'])[1]")).Click();
            
            //Enter Comments
            Driver.FindElement(By.CssSelector("textarea[name='question_322644781']")).SendKeys("RandomText");
            
            //Select Overall experience
            Driver.FindElement(By.CssSelector("tbody tr[role='radiogroup'] td:nth-child(1) span:nth-child(1)")).Click();

            //Click on Submit
            Driver.FindElement(By.CssSelector("button[class='btn continue']")).Click();

            //Assert Title
            string expectedTitle = Driver.Title;
            Assert.That(Driver.Title.Contains("Your Feedback"), Is.EqualTo(true));
            
           
        }

        
    }
}