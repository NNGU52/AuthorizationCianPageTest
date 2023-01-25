using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using OpenQA.Selenium.Interactions;
using System;

namespace AuthorizationCianPageTest
{
    public class Tests
    {
        // объявить драйвер
        private IWebDriver driver;

        private readonly By _signInButton = By.XPath("//span[text()='Войти']");
        private readonly By _phoneInputButton = By.XPath("//input[@autocomplete='tel']");
        private readonly By _acceptCheckBox = By.XPath("(//input[@type='checkbox'])[1]");
        private readonly By _continueButton = By.XPath("//span[text()='Продолжить']");
        private readonly By _errorText = By.XPath("//span[text()='Произошла непредвиденная ошибка']");
        private readonly By _codeButton = By.XPath("//span[text()='Получить код']");
        private readonly By _oneLastNumber = By.XPath("//input[@name='pin_input_0']");
        private readonly By _twoLastNumber = By.XPath("//input[@name='pin_input_1']");
        private readonly By _threeLastNumber = By.XPath("//input[@name='pin_input_2']");
        private readonly By _fourLastNumber = By.XPath("//input[@name='pin_input_3']");
        private readonly By _errorTextTwo = By.XPath("//div[text()='Введён неверный код']");
        private readonly By _restartButton = By.XPath("//span[text()='Ввести заново']");

        private const string _phone = "89834183377";
        private const string _fourNumber = "6812";

        // метод для проверки на наличие элемента
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
            // открыть окно в браузере
            driver = new OpenQA.Selenium.Chrome.ChromeDriver();
            driver.Navigate().GoToUrl("https://cian.ru");

            // развеврнуть на весь экран
            driver.Manage().Window.Maximize();

            // удалить куки
            driver.Manage().Cookies.DeleteAllCookies();
        }

        [Test]
        public void Test1()
        {
            // нажать на кнопку Войти
            var signIn = driver.FindElement(_signInButton);
            signIn.Click();

            // ввести телефон для идентификации
            var phone = driver.FindElement(_phoneInputButton);
            phone.SendKeys(_phone);

            // ожидание загрузки элемента
            Thread.Sleep(15000);

            // проверка на наличие элемента
            bool prov1 = isElementPresent(_errorText);

            while (prov1 != false)
            {
                var code = driver.FindElement(_codeButton);
                code.Click();

                // ожидание загрузки элемента
                Thread.Sleep(15000);

                prov1 = isElementPresent(_errorText);
            }

            var check = driver.FindElement(_acceptCheckBox);

            // перемещаем указатель мыши на элемент и кликаем
            Actions builder = new Actions(driver);
            builder.MoveToElement(check).Click().Build().Perform();

            // нажать на кнопку Продолжить
            var continueLogin = driver.FindElement(_continueButton);
            continueLogin.Click();

            // ожидание загрузки элемента
            Thread.Sleep(5000);

            // ввести последние 4 цифры кода
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
            // закрываем браузер
            driver.Quit();
        }
    }
}