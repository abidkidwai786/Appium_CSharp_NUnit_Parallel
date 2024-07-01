using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using NUnit.Framework;
using System.Threading;
using System.Collections.Generic;

namespace NUnitSelenium
{

    [TestFixture("Pixel 3", "9", "android")]
    [TestFixture("Galaxy S21 5G", "11", "android")]
    [TestFixture("OnePlus 11", "13", "android")]
    [Parallelizable(ParallelScope.Children)]
    public class NUnitSeleniumSample
    {
        public static string LT_USERNAME = "";
        public static string LT_ACCESS_KEY = "";
        public static bool tunnel = Boolean.Parse(Environment.GetEnvironmentVariable("LT_TUNNEL")== null ? "false" : Environment.GetEnvironmentVariable("LT_TUNNEL"));       
        public static string build = Environment.GetEnvironmentVariable("LT_BUILD") == null ? "your build name" : Environment.GetEnvironmentVariable("LT_BUILD");
        public static string seleniumUri = "https://mobile-hub.lambdatest.com:443/wd/hub";


        ThreadLocal<IWebDriver> driver = new ThreadLocal<IWebDriver>();
        private String device;
        private String version;
        private String platform;

        public NUnitSeleniumSample(String device, String version, String platform)
        {
            this.device = device;
            this.version = version;
            this.platform = platform;
        }

        [SetUp]
        public void Init()
        {
            
            DesiredCapabilities capabilities = new DesiredCapabilities();
            capabilities.SetCapability("deviceName", device);
            capabilities.SetCapability("platformVersion", version);
            capabilities.SetCapability("platformName", platform);
            capabilities.SetCapability("isRealMobile", true);
            capabilities.SetCapability("app", "lt://proverbial-android");

            //Requires a named tunnel.
            if (tunnel)
            {
                capabilities.SetCapability("tunnel", tunnel);
            }
            if (build != null)
            {
                capabilities.SetCapability("build", "C# NUNIT APPIUM PARALLEL");
            }
          
            capabilities.SetCapability("user", LT_USERNAME);
            capabilities.SetCapability("accessKey", LT_ACCESS_KEY);

            capabilities.SetCapability("name",
            String.Format("{0}:{1}",
            TestContext.CurrentContext.Test.ClassName,
            TestContext.CurrentContext.Test.MethodName));
            driver.Value = new RemoteWebDriver(new Uri(seleniumUri), capabilities, TimeSpan.FromSeconds(600));
            Console.Out.WriteLine(driver);
        }

        [Test]
       public void Todotest()
        {
            {

                driver.Value.FindElement(By.Id("color")).Click();
                Thread.Sleep(3000);
                driver.Value.FindElement(By.Id("color")).Click();
                Thread.Sleep(3000);
                Console.WriteLine("Clicking color");
                driver.Value.FindElement(By.Id("Text")).Click();
                Thread.Sleep(3000);

                driver.Value.FindElement(By.Id("toast")).Click();
                Thread.Sleep(3000);
                driver.Value.FindElement(By.Id("notification")).Click();
                Thread.Sleep(3000);


            }
        }

        [TearDown]
        public void Cleanup()
        {
            bool passed = TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Passed;
            try
            {
                // Logs the result to LambdaTest
                ((IJavaScriptExecutor)driver.Value).ExecuteScript("lambda-status=" + (passed ? "passed" : "failed"));
            }
            finally
            {
                
                // Terminates the remote webdriver session
                driver.Value.Quit();
            }
        }
    }
}
