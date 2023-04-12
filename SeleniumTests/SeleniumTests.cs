using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumTests
{
    public class SeleniumTests
    {

        private WebDriver driver;
        private const string baseUrl = "https://shorturl.cappricornia.repl.co/";

        [SetUp]
        public void OpenBrowser()
        {
            this.driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.Url = baseUrl;
        }

        [TearDown]
        public void CloseBrowser()
        {
            driver.Quit();
        }

        [Test]
        public void Test_ShortUrl_Holds_leftCell_header_OriginalUrl()
        {
            var shortUrls = driver.FindElement(By.LinkText("Short URLs"));
            shortUrls.Click();

            var originalUrl = driver.FindElement(By.CssSelector("body > main > table > thead > tr > th:nth-child(1)"));

            Assert.That(originalUrl.Text, Is.EqualTo("Original URL"), "Table Upper Left Element");

        }

        [Test]
        public void Test_AddUrl_Add_Valid_Url_Assert_New_Email_In_The_List()
        {

            var newUrl = "http://newurl" + DateTime.Now.Ticks + ".com";

            var addUrl = driver.FindElement(By.LinkText("Add URL"));
            addUrl.Click();

            var inputUrl = driver.FindElement(By.Id("url"));
            inputUrl.SendKeys(newUrl);

            var createButton = driver.FindElement(By.XPath("//button[@type='submit']"));
            createButton.Click();


            Assert.That(driver.PageSource.Contains(newUrl));

            var lastRow = driver.FindElements(By.CssSelector("table > tbody > tr")).Last();
            
            var lastRowFirstCell = lastRow.FindElements(By.CssSelector("td")).First();

            Assert.That(lastRowFirstCell.Text, Is.EqualTo(newUrl), "URL");

        }

        [Test]
        public void Test_AddUrl_Add_Invalid_Url_Assert_Error_Msg()
        {

            var invalidUrl = "www.invalidurl.com";

            var addUrl = driver.FindElement(By.LinkText("Add URL"));
            addUrl.Click();

            var inputUrl = driver.FindElement(By.Id("url"));
            inputUrl.SendKeys(invalidUrl);

            var createButton = driver.FindElement(By.XPath("//button[@type='submit']"));
            createButton.Click();

            var errMsg = driver.FindElement(By.XPath("//div[contains(@class,'err')]"));

            Assert.True(errMsg.Displayed, "Err Msg not displayed");

            Assert.That(errMsg.Text, Is.EqualTo("Invalid URL!"));

        }


        [Test]
        public void Test_Visit_Non_Existing_Short_Url()
        {

            driver.Url = "https://shorturl.cappricornia.repl.co/go/invalid349393";

            var errMsg = driver.FindElement(By.XPath("//div[contains(.,'Cannot navigate to given short URL')]"));

            Assert.True(errMsg.Displayed, "Err Msg not displayed");

            Assert.That(errMsg.Text, Is.EqualTo("Cannot navigate to given short URL"));
        }

        [Test]
        public void Test_Visit_Existing_Short_Url_Assert_Visitors_Counter_Increases()
        {
            var shortUrls = driver.FindElement(By.LinkText("Short URLs"));
            shortUrls.Click();

            var firstRow = driver.FindElements(By.CssSelector("table > tbody > tr")).First();

           
            var oldCount = int.Parse(firstRow.FindElements(By.CssSelector("td")).Last().Text);

           
            var cellClickShortLink = firstRow.FindElements(By.CssSelector("td"))[1];

            
            var clickShortLink = cellClickShortLink.FindElement(By.TagName("a"));
            clickShortLink.Click();

           
            driver.SwitchTo().Window(driver.WindowHandles[0]);

            driver.Navigate().Refresh();

            firstRow = driver.FindElements(By.CssSelector("table > tbody > tr")).First();
           
            var newCount = int.Parse(firstRow.FindElements(By.CssSelector("td")).Last().Text);
            
            Assert.That(newCount, Is.EqualTo(oldCount + 1));
        }

    }
}