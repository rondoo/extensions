﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Internal;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support;
using OpenQA.Selenium.Support.UI;
using Signum.Utilities;


namespace Signum.Web.Selenium
{
    public static class SeleniumExtensions
    {
        //public static string PageLoadTimeout = "20000";
        public static TimeSpan DefaultTimeout = TimeSpan.FromMilliseconds(20 * 1000);
        public static TimeSpan DefaultPoolingInterval = TimeSpan.FromMilliseconds(200);

        //public static void WaitForPageToLoad(this RemoteWebDriver selenium)
        //{
        //    selenium.WaitForPageToLoad(PageLoadTimeout);
        //}

        public static T Wait<T>(this RemoteWebDriver selenium, Func<T> condition, Func<string> actionDescription = null, TimeSpan? timeout = null)
        {
            try
            {
                var wait = new DefaultWait<string>("")
                {
                    Timeout = timeout ?? DefaultTimeout,
                    PollingInterval = DefaultPoolingInterval
                };

                wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(NoAlertPresentException));
                
                return wait.Until(_ => condition());
            }
            catch (WebDriverTimeoutException ex)
            {
                throw new WebDriverTimeoutException(ex.Message + ": waiting for {0} in page {1}({2})".FormatWith(
                    actionDescription == null ? "visual condition" : actionDescription(),
                    selenium.Title,
                    selenium.Url));
            }
        }

        public static IWebElement TryFindElement(this RemoteWebDriver selenium, By locator)
        {
            return selenium.FindElements(locator).FirstOrDefault();
        }

        public static IWebElement WaitElementPresent(this RemoteWebDriver selenium, By locator, Func<string> actionDescription = null, TimeSpan? timeout = null)
        {
            return selenium.Wait(() => selenium.FindElements(locator).FirstOrDefault(),
                actionDescription ?? (Func<string>)(() => "{0} to be present".FormatWith(locator)), timeout);
        }

        public static void AssertElementPresent(this RemoteWebDriver selenium, By locator)
        {
            if (!selenium.IsElementPresent(locator))
                throw new InvalidOperationException("{0} not found".FormatWith(locator));
        }

        public static bool IsElementPresent(this RemoteWebDriver selenium, By locator)
        {
            return selenium.FindElements(locator).Any();
        }

        public static void WaitElementNotPresent(this RemoteWebDriver selenium, By locator, Func<string> actionDescription = null, TimeSpan? timeout = null)
        {
            selenium.Wait(() => !selenium.IsElementPresent(locator),
                actionDescription ?? (Func<string>)(() => "{0} to be not present".FormatWith(locator)), timeout);
        }

        public static void AssertElementNotPresent(this RemoteWebDriver selenium, By locator)
        {
            if (selenium.IsElementPresent(locator))
                throw new InvalidOperationException("{0} is found".FormatWith(locator));
        }

        public static IWebElement WaitElementVisible(this RemoteWebDriver selenium, By locator, Func<string> actionDescription = null, TimeSpan? timeout = null)
        {
            return selenium.Wait(() => selenium.FindElements(locator).FirstOrDefault(a => a.Displayed),
                actionDescription ?? (Func<string>)(() => "{0} to be visible".FormatWith(locator)), timeout);
        }

        public static void AssertElementVisible(this RemoteWebDriver selenium, By locator)
        {
            var elements = selenium.FindElements(locator);

            if (!elements.Any())
                throw new InvalidOperationException("{0} not found".FormatWith(locator));

            if (!elements.First().Displayed)
                throw new InvalidOperationException("{0} found but not visible".FormatWith(locator));
        }

        public static bool IsElementVisible(this RemoteWebDriver selenium, By locator)
        {
            var elements = selenium.FindElements(locator);
            try
            {
                return elements.Any() && elements.First().Displayed;
            }
            catch (StaleElementReferenceException)
            {
                return false;
            }
        }

        public static void WaitElementNotVisible(this RemoteWebDriver selenium, By locator, Func<string> actionDescription = null, TimeSpan? timeout = null)
        {
            selenium.Wait(() => !selenium.IsElementVisible(locator),
                actionDescription ?? (Func<string>)(() => "{0} to be not visible".FormatWith(locator)), timeout);
        }

        public static void AssertElementNotVisible(this RemoteWebDriver selenium, By locator)
        {
            if (selenium.IsElementVisible(locator))
                throw new InvalidOperationException("{0} is visible".FormatWith(locator));
        }

        public static RemoteWebDriver GetDriver(this IWebElement element)
        {
            return (RemoteWebDriver)((IWrapsDriver)element).WrappedDriver;
        }

        public static void SetChecked(this IWebElement element, bool isChecked)
        {
            if (element.Selected == isChecked)
                return;

            element.Click();

            if (element.Selected != isChecked)
                throw new InvalidOperationException();
        }

        [DebuggerStepThrough]
        public static bool IsAlertPresent(this RemoteWebDriver selenium)
        {
            try
            {
                selenium.SwitchTo().Alert();
                return true;
            }
            catch (NoAlertPresentException e)
            {
                return false;
            }
        }

        public static void ConsumeAlert(this RemoteWebDriver selenium)
        {
            var alert = selenium.Wait(() => selenium.SwitchTo().Alert());
            
            alert.Accept();
        }

        public static string CssSelector(this By by)
        {
            string str = by.ToString();

            var after = str.After(": ");
            switch (str.Before(":"))
            {
                case "By.CssSelector": return after;
                case "By.Id": return "#" + after;
                case "By.Name": return "[name=" + after + "]";
                default: throw new InvalidOperationException("Impossible to combine: " + str);
            }
        }

        public static By CombineCss(this By by, string cssSelectorSuffix)
        {
            return By.CssSelector(by.CssSelector() + cssSelectorSuffix);
        }

        public static bool ContainsText(this IWebElement element, string text)
        {
            return element.Text.Contains(text) || element.FindElements(By.XPath("descendant::*[contains(text(), '" + text + "')]")).Any();
        }

        public static SelectElement SelectElement(this IWebElement element)
        {
            return new SelectElement(element);
        }

        public static string GetID(this IWebElement element)
        {
            return element.GetAttribute("id");
        }

        public static IEnumerable<string> GetClasses(this IWebElement element)
        {
            return element.GetAttribute("class").Split(' ');
        }

        public static bool HasClass(this IWebElement element, string className)
        {
            return element.GetClasses().Contains(className);
        }

        public static bool HasClass(this IWebElement element, params string[] classNames)
        {
            var classes = element.GetClasses();
            return classNames.All(classes.Contains);
        }

        public static IWebElement GetParent(this IWebElement e)
        {
            return e.FindElement(By.XPath(".."));
        }

        public static void SelectByPredicate(this SelectElement element, Func<IWebElement, bool> predicate)
        {
            element.Options.SingleEx(predicate).Click();
        }

        public static void ContextClick(this IWebElement element)
        {
            Actions builder = new Actions(element.GetDriver());
            builder.MoveToElement(element, 2, 2).ContextClick().Build().Perform();
        }

        public static void DoubleClick(this IWebElement element)
        {
            Actions builder = new Actions(element.GetDriver());
            builder.MoveToElement(element, 2, 2).DoubleClick().Build().Perform();
        }

        public static void SafeSendKeys(this IWebElement element, string text)
        {
            while(element.GetAttribute("value").Length > 0)
                element.SendKeys(Keys.Backspace);
            element.SendKeys(text);
            Thread.Sleep(0);
            element.GetDriver().Wait(() => element.GetAttribute("value") == text);
        }

        public static void ButtonClick(this IWebElement button)
        {
            if (!button.Enabled)
                throw new InvalidOperationException("Button is not enabled");

            if (!button.Displayed)
            {
                var menu = button.FindElement(By.XPath("ancestor::*[contains(@class,'dropdown-menu')]"));
                var superButton = menu.GetParent().FindElement(By.CssSelector("a[data-toggle='dropdown']"));
                superButton.Click();
            }

            button.Click();
        }
    }
}
