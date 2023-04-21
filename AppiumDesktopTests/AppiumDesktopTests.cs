using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;

namespace AppiumDesktopTests
{
    public class AppiumDesktopTests
    {
        private WindowsDriver<WindowsElement> driver;
        private AppiumOptions options;

        private const string appLocation = @"C:\AppsDemo\shortUrl\ShortURL-DesktopClient.exe";
        private const string appiumServer = "http://127.0.0.1:4723/wd/hub";
       
        private const string appServer = "https://shorturl.Cappricornia.repl.co/api";

        [SetUp]
        public void SetUpApp()
        {
            this.options = new AppiumOptions();
            options.AddAdditionalCapability("app", appLocation);
            driver = new WindowsDriver<WindowsElement>(new Uri(appiumServer), options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        [TearDown]
        public void CloseApp()
        {
            driver.Quit();
        }

        [Test]
        public void Test_Create_A_New_URl()
        {
            var newUrl = "https://url" + DateTime.Now.Ticks + ".com";

            var inputUrl = driver.FindElementByAccessibilityId("textBoxApiUrl");
            inputUrl.Clear();
            inputUrl.SendKeys(appServer);

            var connectButton = driver.FindElementByAccessibilityId("buttonConnect");
            connectButton.Click();

            var addButton = driver.FindElementByAccessibilityId("buttonAdd");
            addButton.Click();  

            var inputNewUrl = driver.FindElementByAccessibilityId("textBoxURL");
            inputNewUrl.SendKeys(newUrl);

            Thread.Sleep(2000);

            var createButton = driver.FindElementByAccessibilityId("buttonCreate");
            createButton.Click();

            var addedUrl = driver.FindElementByName(newUrl);
            
            Assert.IsNotEmpty(addedUrl.Text);
            Assert.That(addedUrl.Text, Is.EqualTo(newUrl)); 
        }
        
        [Test]
        public void Test_Add_Invalid_Data_Assert_Err_Msg()
        {
            var apiUrl = driver.FindElementByAccessibilityId("textBoxApiUrl");
            apiUrl.Clear();

            apiUrl.SendKeys(appServer);

            var connectButton = driver.FindElementByAccessibilityId("buttonConnect");
            connectButton.Click();

            var addButton = driver.FindElementByAccessibilityId("buttonAdd");
            addButton.Click();

            var invalidData = "invalid data";

            var urlField = driver.FindElementByAccessibilityId("textBoxURL");
            urlField.SendKeys(invalidData);

            var createButton = driver.FindElementByAccessibilityId("buttonCreate");
            createButton.Click();

             Assert.That(HttpStatusCode.BadRequest.ToString, Is.EqualTo("BadRequest"));
        }

    }
}
