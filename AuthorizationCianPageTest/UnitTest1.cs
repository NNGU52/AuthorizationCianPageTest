using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using OpenQA.Selenium.Interactions;
using System;

namespace AuthorizationCianPageTest
{
    public class Tests
    {
        // �������� �������
        private IWebDriver driver;

        private readonly By _signInButton = By.XPath("//span[text()='�����']");
        private readonly By _phoneInputButton = By.XPath("//input[@autocomplete='tel']");
        private readonly By _acceptCheckBox = By.XPath("(//input[@type='checkbox'])[1]");
        private readonly By _continueButton = By.XPath("//span[text()='����������']");
        private readonly By _errorText = By.XPath("//span[text()='��������� �������������� ������']");
        private readonly By _codeButton = By.XPath("//span[text()='�������� ���']");
        private readonly By _oneLastNumber = By.XPath("//input[@name='pin_input_0']");
        private readonly By _twoLastNumber = By.XPath("//input[@name='pin_input_1']");
        private readonly By _threeLastNumber = By.XPath("//input[@name='pin_input_2']");
        private readonly By _fourLastNumber = By.XPath("//input[@name='pin_input_3']");
        private readonly By _errorTextTwo = By.XPath("//div[text()='����� �������� ���']");
        private readonly By _restartButton = By.XPath("//span[text()='������ ������']");

        private const string _phone = "89834183377";
        private const string _fourNumber = "6812";

        // ����� ��� �������� �� ������� ��������
        public Boolean isElementPresent(By locatorKey)
        {
            try
            {
                driver.FindElement(locatorKey);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        [SetUp]
        public void Setup()
        {
            // ������� ���� � ��������
            driver = new OpenQA.Selenium.Chrome.ChromeDriver();
            driver.Navigate().GoToUrl("https://cian.ru");

            // ����������� �� ���� �����
            driver.Manage().Window.Maximize();

            // ������� ����
            driver.Manage().Cookies.DeleteAllCookies();
        }

        [Test]
        public void Test1()
        {
            // ������ �� ������ �����
            var signIn = driver.FindElement(_signInButton);
            signIn.Click();

            // ������ ������� ��� �������������
            var phone = driver.FindElement(_phoneInputButton);
            phone.SendKeys(_phone);

            // �������� �������� ��������
            Thread.Sleep(15000);

            // �������� �� ������� ��������
            bool prov1 = isElementPresent(_errorText);

            while (prov1 != false)
            {
                var code = driver.FindElement(_codeButton);
                code.Click();

                // �������� �������� ��������
                Thread.Sleep(15000);

                prov1 = isElementPresent(_errorText);
            }

            var check = driver.FindElement(_acceptCheckBox);

            // ���������� ��������� ���� �� ������� � �������
            Actions builder = new Actions(driver);
            builder.MoveToElement(check).Click().Build().Perform();

            // ������ �� ������ ����������
            var continueLogin = driver.FindElement(_continueButton);
            continueLogin.Click();

            // �������� �������� ��������
            Thread.Sleep(5000);

            // ������ ��������� 4 ����� ����
            var oneLast = driver.FindElement(_oneLastNumber);
            oneLast.SendKeys(_fourNumber.Substring(0,1));
            var twoLast = driver.FindElement(_twoLastNumber);
            twoLast.SendKeys(_fourNumber.Substring(1, 1));
            var threeLast = driver.FindElement(_threeLastNumber);
            threeLast.SendKeys(_fourNumber.Substring(2, 1));
            var fourLast = driver.FindElement(_fourLastNumber);
            fourLast.SendKeys(_fourNumber.Substring(3, 1));

            Thread.Sleep(1000);

            bool prov2 = isElementPresent(_errorTextTwo);
            if (prov2 == true)
            {
                var restart = driver.FindElement(_restartButton);
                restart.Click();
                Thread.Sleep(600);
            }
        }

        [TearDown]
        public void TearDown()
        {
            // ��������� �������
            driver.Quit();
        }
    }
}