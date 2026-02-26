using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentGradebookApi.Tests.UITests
{
    public class LoadStudentListTests
    {
        public void Login(ChromeDriver driver)
        {
            var loginForm = driver.FindElement(By.CssSelector(".MuiContainer-root"));
            var emailInput = loginForm.FindElement(By.Id("email"));
            emailInput.SendKeys("pass123");

            var pswInput = loginForm.FindElement(By.Id("password"));
            pswInput.SendKeys("pass123");

            var submitBtn = loginForm.FindElement(By.CssSelector(".MuiButton-root"));
            submitBtn.Click();
        }

        [Fact]
        public void LoginFormPage_UserSuccessfullyLogsIn()
        {
            using var driver = new ChromeDriver(); //Starts Chrome
            driver.Navigate().GoToUrl("http://localhost:5173");

            Login(driver);

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10)); //wait timer
            wait.Until(d => d.Url == "http://localhost:5173/dashboard");
            //var table = wait.Until(d => d.FindElement(By.CssSelector("[data-testid='student-table']")));
            //var rows = table.FindElements(By.CssSelector(".MuiDataGrid-row"));
            //Assert.True(rows.Count > 0);
        }

        [Fact]
        public void StudentListPage_ShouldLoadUsers()
        {
            using var driver = new ChromeDriver(); //Starts Chrome
            driver.Navigate().GoToUrl("http://localhost:5173");

            Login(driver);

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10)); //wait timer
            wait.Until(d => d.Url == "http://localhost:5173/dashboard");

            driver.Navigate().GoToUrl("http://localhost:5173/students");

            var table = wait.Until(d => d.FindElement(By.CssSelector(".MuiTable-root")));

            var rows = wait.Until(d => d.FindElements(By.CssSelector(".MuiTableRow-root")));
            Assert.True(rows.Count() > 0);
        }
    }
}
