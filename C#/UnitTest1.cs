using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Threading;

namespace UnitTestProject1
{
    [TestClass]
	public class NexaTestSession
	{
		// winappdriver url
		protected const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723/";
		// app path
		private const string NexacroAppDr = "C:\\Program Files (x86)\\nexacro\\17\\app0201";
		// nexacro.exe path
		private const string NexacroAppId = "C:\\Program Files (x86)\\nexacro\\17\\app0201\\nexacro.exe";
		// nexacro.exe argument
		private const string NexacroAppAg = "-k 'UnitTest' -s 'C:\\Program Files (x86)\\nexacro\\17\\app0201\\app0201\\start.json'";
		// search root
		private const string FindNexacro = "Root";
		// OS/Device
		private const string TestOs = "Windows";
		private const string TestDevice = "WindowsPC";

		// Session Object : WindowsDriver 
		protected static WindowsDriver<WindowsElement> session;

		// WinAppDriver Session Create
		[ClassInitialize]
		public static void Setup(TestContext context)
		{
			// Launch a new instance of nexacro application or find nexacro application
			if (session == null)
			{
				// Create a new session to launch nexacro application
				DesiredCapabilities appCapabilities = new DesiredCapabilities();
				TimeSpan time = new TimeSpan(0, 0, 10);

				// Launch Nexacro
				appCapabilities.SetCapability("app", NexacroAppId);
				appCapabilities.SetCapability("appArguments", NexacroAppAg);
				appCapabilities.SetCapability("appWorkingDir", NexacroAppDr);
				appCapabilities.SetCapability("platformName", TestOs);
				appCapabilities.SetCapability("deviceName", TestDevice);

				// Request Remote WinAppDriver Service
				session = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appCapabilities, time);

				// Set implicit timeout to 1.5 seconds to make element search to retry every 500 ms for at most three times
				session.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(0.5));
				Thread.Sleep(TimeSpan.FromSeconds(1.0));
			}

		}

        [TestMethod]
        public void GetStatus()
        {
            var request = WebRequest.Create(WindowsApplicationDriverUrl + "status/");
            request.Method = "GET";
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                JObject responseObject = JObject.Parse(responseString);

                JToken buildToken = responseObject["build"];
                Assert.IsNotNull(buildToken);
                Assert.IsNotNull(buildToken["version"]);
                Assert.IsNotNull(buildToken["revision"]);
                Assert.IsNotNull(buildToken["time"]);

                JToken osToken = responseObject["os"];
                Assert.IsNotNull(osToken);
                Assert.IsNotNull(osToken["arch"]);
                Assert.AreEqual(osToken["name"], "windows");
                Assert.IsNotNull(osToken["version"]);
            }
        }

        [TestMethod]
        public void Test_01_Session()
        {
            Debug.WriteLine("session:" + session);
            Debug.WriteLine("session.SessionId:" + session.SessionId);
        }

        [TestMethod]
        public void Test_02_MainFrame()
        {
            Debug.WriteLine("session.Title:" + session.Title);
        }

        [TestMethod]
        public void Test_03_WindowHandle()
        {
            Debug.WriteLine("session.CurrentWindowHandle:" + session.CurrentWindowHandle);
            Debug.WriteLine("session.WindowHandles:" + session.WindowHandles);
            Debug.WriteLine("session.WindowHandles[0]:" + session.WindowHandles[0]);
            Debug.WriteLine("session.Manage().Window:" + session.Manage().Window);
            Debug.WriteLine("session.Manage().Window.Size:" + session.Manage().Window.Size);
        }

        [TestMethod]
        public void Test_04_WindowSwitch()
        {
            string temp_WindowHandle = null;
            WindowsElement nexaButton00 = session.FindElementByAccessibilityId("mainframe.ChildFrame00.form.Button00");
            Thread.Sleep(TimeSpan.FromSeconds(0.1));
            nexaButton00.Click();
            Thread.Sleep(TimeSpan.FromSeconds(1.0));
            Debug.WriteLine("session.Count:" + session.WindowHandles.Count); //2
            Debug.WriteLine("session.WindowHandles:" + session.CurrentWindowHandle);
            temp_WindowHandle = session.CurrentWindowHandle;
            session.SwitchTo().Window(session.WindowHandles[0]);
            Debug.WriteLine("session.WindowHandles[0]:" + session.WindowHandles[0]);
            Debug.WriteLine("session.WindowHandles:" + session.CurrentWindowHandle);
            WindowsElement closeButton = session.FindElementByAccessibilityId("test01.form.Button00");
            session.SwitchTo().Window(temp_WindowHandle);
            Thread.Sleep(TimeSpan.FromSeconds(0.1));
            closeButton.Click();
            Debug.WriteLine("session.Count:" + session.WindowHandles.Count); //1

            WindowsElement nexaButton01 = session.FindElementByAccessibilityId("mainframe.ChildFrame00.form.Button00");
            Debug.WriteLine("session.WindowHandles:" + session.CurrentWindowHandle);
        }

        [TestMethod]
        public void Test_05_WindowMaximize()
        {
            session.Manage().Window.Maximize();
        }

        [TestMethod]
        public void Test_06_WindowSizeChange()
        {
            Size preSize = session.Manage().Window.Size;
            session.Manage().Window.Position = new Point(50, 50);
            session.Manage().Window.Size = new Size(800, 800);
            Debug.WriteLine("session.Manage().Window.Position:" + session.Manage().Window.Position);
            Debug.WriteLine("session.Manage().Window.Size:" + session.Manage().Window.Size);
            Thread.Sleep(TimeSpan.FromSeconds(1.0));
            session.Manage().Window.Size = preSize;
            Debug.WriteLine("session.Manage().Window.Size:" + session.Manage().Window.Size);
        }

        [TestMethod]
        public void Test_07_FindElement()
        {
            WindowsElement element = session.FindElementByAccessibilityId("mainframe.ChildFrame00.form.Button00");
            Assert.IsNotNull(element);

            ReadOnlyCollection<WindowsElement> elements = session.FindElementsByAccessibilityId("mainframe.ChildFrame00.form.Button00");
            Assert.IsNotNull(elements);

            WindowsElement nexaComboDrop00 = session.FindElementByAccessibilityId("mainframe.ChildFrame00.form.Div00.form.Combo00.dropbutton");
            Assert.IsNotNull(nexaComboDrop00);
            WindowsElement nexaRadioItem1 = session.FindElementByAccessibilityId("mainframe.ChildFrame00.form.Radio00.radioitem1");
            Assert.IsNotNull(nexaRadioItem1);
        }

        [TestMethod]
        public void Test_08_ElementInfo()
        {
            WindowsElement element = session.FindElementByAccessibilityId("mainframe.ChildFrame00.form.TextArea00");
            Assert.IsNotNull(element);

            Debug.WriteLine("element.GetAttribute:" + element.GetAttribute("AutomationId"));
            Debug.WriteLine("element.Text:" + element.Text);
            Debug.WriteLine("element.Name:" + element.Id);
            Debug.WriteLine("element.TagName:" + element.TagName);
        }

        [TestMethod]
        public void Test_09_EditInput()
        {
            WindowsElement nexaEdit00 = session.FindElementByAccessibilityId("mainframe.ChildFrame00.form.Edit00:input");
            Assert.IsNotNull(nexaEdit00);
            Thread.Sleep(TimeSpan.FromSeconds(0.1));

            nexaEdit00.Clear();
            nexaEdit00.Click();
            nexaEdit00.SendKeys("abcdeABCDE");
            Assert.AreEqual("abcdeABCDE", nexaEdit00.Text);
        }

        [TestMethod]
        public void Test_10_MaskEditInput()
        {
            WindowsElement nexaMaskEdit00 = session.FindElementByAccessibilityId("mainframe.ChildFrame00.form.MaskEdit00");
            Assert.IsNotNull(nexaMaskEdit00);
            Thread.Sleep(TimeSpan.FromSeconds(0.1));

            nexaMaskEdit00.Clear();
            nexaMaskEdit00.Click();
            nexaMaskEdit00.SendKeys(Keys.Control + "a");
            Thread.Sleep(TimeSpan.FromSeconds(0.1));
            nexaMaskEdit00.SendKeys("abcde1234"+Keys.Enter);
            Assert.AreEqual("abcde-1234", nexaMaskEdit00.Text);
        }

        [TestMethod]
        public void Test_11_TextAreaInput()
        {
            WindowsElement nexaTextArea00 = session.FindElementByAccessibilityId("mainframe.ChildFrame00.form.TextArea00:textarea");
            Assert.IsNotNull(nexaTextArea00);
            Thread.Sleep(TimeSpan.FromSeconds(0.1));

            nexaTextArea00.Clear();
            nexaTextArea00.Click();
            nexaTextArea00.SendKeys("abcdeABCDE 12345&*()_!@#$%");
            Assert.AreEqual("abcdeABCDE 12345&*()_!@#$%", nexaTextArea00.Text);
        }

        [TestMethod]
        public void Test_12_CheckBoxToggle()
        {
            WindowsElement nexaCheckBox00 = session.FindElementByAccessibilityId("mainframe.ChildFrame00.form.Div00.form.CheckBox00");
            Assert.IsNotNull(nexaCheckBox00);
            Thread.Sleep(TimeSpan.FromSeconds(0.1));
            Debug.WriteLine("nexaCheckBox00.Selected:" + nexaCheckBox00.Selected);

            // Clear CheckBox
            nexaCheckBox00.Clear();
            //Thread.Sleep(TimeSpan.FromSeconds(0.1));
            Debug.WriteLine("nexaCheckBox00.Selected:" + nexaCheckBox00.Selected);

            // Toggle by Click CheckBox
            nexaCheckBox00.Click();
            //Thread.Sleep(TimeSpan.FromSeconds(0.1));
            Debug.WriteLine("nexaCheckBox00.Selected:" + nexaCheckBox00.Selected);

            // Toggle by Space CheckBox
            nexaCheckBox00.SendKeys(Keys.Space);
            //Thread.Sleep(TimeSpan.FromSeconds(0.1));
            Debug.WriteLine("nexaCheckBox00.Selected:" + nexaCheckBox00.Selected);
        }

        [TestMethod]
        public void Test_13_SpinValue()
        {
            WindowsElement nexaSpin00 = session.FindElementByAccessibilityId("mainframe.ChildFrame00.form.Div00.form.Spin00");
            Assert.IsNotNull(nexaSpin00);
            Thread.Sleep(TimeSpan.FromSeconds(0.1));

            // Spin Up by Key Up
            nexaSpin00.Click();
            nexaSpin00.SendKeys(Keys.Up);
            nexaSpin00.SendKeys(Keys.Up);
            Assert.AreEqual("2", nexaSpin00.Text);

            // Search Spin Edit
            WindowsElement nexaSpinEd = session.FindElementByAccessibilityId("mainframe.ChildFrame00.form.Div00.form.Spin00.spinedit");
            Assert.IsNotNull(nexaSpinEd);

            // Set Spin Edit Value
            nexaSpinEd.Clear();
            nexaSpinEd.Click();
            nexaSpinEd.SendKeys("10");
            Assert.AreEqual("10", nexaSpin00.Text);

            // Search Spin Up/Down Button
            WindowsElement nexaSpinUp = session.FindElementByAccessibilityId("mainframe.ChildFrame00.form.Div00.form.Spin00.spinupbutton");
            WindowsElement nexaSpinDn = session.FindElementByAccessibilityId("mainframe.ChildFrame00.form.Div00.form.Spin00.spindownbutton");
            Assert.IsNotNull(nexaSpinUp);
            Assert.IsNotNull(nexaSpinDn);

            // Spin Up by Button Click
            nexaSpinUp.Click();
            nexaSpinUp.Click();
            Assert.AreEqual("12", nexaSpin00.Text);

            // Spin Down by Button Click
            nexaSpinDn.Click();
            nexaSpinDn.Click();
            Assert.AreEqual("10", nexaSpin00.Text);
        }

        [TestMethod]
        public void Test_14_CompStatus()
        {
            WindowsElement nexaButton00 = session.FindElementByAccessibilityId("mainframe.ChildFrame00.form.Button00");
            Debug.WriteLine("nexaButton00.Displayed:" + nexaButton00.Displayed);
            Debug.WriteLine("nexaButton00.Enabled:" + nexaButton00.Enabled);
            Debug.WriteLine("nexaButton00.Selected:" + nexaButton00.Selected);
            Debug.WriteLine("nexaButton00.Location:" + nexaButton00.Location);
            Debug.WriteLine("nexaButton00.LocationOnScreenOnceScrolledIntoView:" + nexaButton00.LocationOnScreenOnceScrolledIntoView);
            Debug.WriteLine("nexaButton00.Size:" + nexaButton00.Size);
            Debug.WriteLine("nexaButton00.GetAttribute:" + nexaButton00.GetAttribute("AutomationId"));
        }

        [TestMethod]
        public void Test_15_Screenshot()
        {
            Screenshot sessionshot = session.GetScreenshot();
            Assert.IsNotNull(sessionshot);

            // Save Session ScreenShot to ImageFile
            sessionshot.SaveAsFile("/SessionScreenShot.png", ImageFormat.Png);
            // Get Session ScreenShot Base64EncodingString
            String base64str = sessionshot.AsBase64EncodedString;

            WindowsElement element = session.FindElementByAccessibilityId("mainframe.ChildFrame00.form.TextArea00");
            Screenshot elementshot = element.GetScreenshot();
            Assert.IsNotNull(elementshot);

            // Save Element ScreenShot to ImageFile
            elementshot.SaveAsFile("/ElementScreenShot.png", ImageFormat.Png);
            // Get Element ScreenShot Base64EncodingString
            String base64strelement = elementshot.AsBase64EncodedString;
        }

        // WinAppDriver Session Delete
        [ClassCleanup]
        public static void TearDown()
        {
            // Close the application and delete the session
            if (session != null)
            {
                Debug.WriteLine("TearDown");
                // Close Session – Close Application
                session.Close();
                // Quit Session
                session.Quit();
                // Delete Session
                session = null;
            }
        }
    }
}
