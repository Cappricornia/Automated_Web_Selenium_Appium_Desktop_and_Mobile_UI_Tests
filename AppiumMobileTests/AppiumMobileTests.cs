using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium;

namespace AppiumMobileTests
{
    public class AppiumMobileTests
    {
        private AndroidDriver<AndroidElement> driver;
        private AppiumOptions options;

        private const string appLocation = @"C:\AppsDemo\com.android.example.github.apk";
        private const string appiumServer = "http://127.0.0.1:4723/wd/hub";

        [SetUp]
        public void PrepareApp()
        {
            this.options = new AppiumOptions() { PlatformName = "Android" };
            options.AddAdditionalCapability("app", appLocation);
            driver = new AndroidDriver<AndroidElement>(new Uri(appiumServer), options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        [TearDown]
        public void CloseApp()
        {
            driver.Quit();
        }

        [Test]  
        public void Test_Assert_Developer_Barancev_In_The_List()
        {
            var searchField = driver.FindElementById("com.android.example.github:id/input");
            searchField.Click();
            searchField.SendKeys("Selenium");

            driver.PressKeyCode(AndroidKeyCode.Enter);

            var seleniumElement = driver.FindElementByXPath("//android.widget.FrameLayout[1]/android.view.ViewGroup/android.widget.TextView[2]");
           
            Assert.That(seleniumElement.Text, Is.EqualTo("SeleniumHQ/selenium"));

            seleniumElement.Click();

            var barancevProfile = driver.FindElementByXPath("//android.widget.FrameLayout[2]/android.view.ViewGroup/android.widget.TextView");

            Assert.That(barancevProfile.Text, Is.EqualTo("barancev"));

            barancevProfile.Click();

            var barancevName = driver.FindElementByXPath("//android.widget.TextView[@content-desc=\"user name\"]");

            Assert.That(barancevName.Text, Is.EqualTo("Alexei Barantsev"));

        }



    }
}