using OpenQA.Selenium;
using XYZ_shop.Tests.E2E.Selectors;

namespace XYZ_shop.Tests.E2E.Helper
{
    public static class LoginHelper
    {
        public static void Login(this IWebDriver webDriver, string login, string password)
        {
            webDriver.Navigate().GoToUrl($"{GlobalConstants.BASE_URL}/Auth/Login");

            var loginInput = webDriver.FindElement(AuthLoginPage.LoginInput);
            loginInput.SendKeys(login);

            var passwordInput = webDriver.FindElement(AuthLoginPage.PasswordInput);
            passwordInput.SendKeys(password);

            webDriver
                .FindElement(AuthLoginPage.SubmitButton)
                .Click();
        }

        public static void LoginAsAdmin(this IWebDriver webDriver)
        {
            Login(webDriver, "adm", "adm");
        }

        public static void Logout(this IWebDriver webDriver)
        {
            webDriver.Navigate().GoToUrl($"{GlobalConstants.BASE_URL}/Auth/Logout");
        }

        public static void LoginAsUser(this IWebDriver webDriver)
        {
            Login(webDriver, "user", "user");
        }
    }
}
