import org.junit.*;
import org.openqa.selenium.Point;
import org.openqa.selenium.TakesScreenshot;
import org.openqa.selenium.Dimension;
import org.openqa.selenium.Keys;
import org.openqa.selenium.OutputType;
import org.openqa.selenium.WebElement;
import org.openqa.selenium.io.FileHandler;
import org.openqa.selenium.remote.DesiredCapabilities;
import io.appium.java_client.windows.WindowsDriver;
import org.openqa.selenium.remote.RemoteWebElement;

import java.util.List;
import java.util.Set;
import java.util.concurrent.TimeUnit;
import java.io.File;
import java.net.URL;

public class UnitTest {

	// winappdriver url
	private String WindowsApplicationDriverUrl = "http://127.0.0.1:4723/";
	// app path
	private String NexacroAppDr = "C:\\Program Files (x86)\\nexacro\\17\\app0201";
	// nexacro.exe path
	private String NexacroAppId = "C:\\Program Files (x86)\\nexacro\\17\\app0201\\nexacro.exe";
	// nexacro.exe argument
	private String NexacroAppAg = "-k 'UnitTest' -s 'C:\\Program Files (x86)\\nexacro\\17\\app0201\\app0201\\start.json'";
	// search root
	// private String FindNexacro = "Root";

	// OS/Device
	private String TestOs = "Windows";
	private String TestDevice = "WindowsPC";

	// Session Object : WindowsDriver
	private static WindowsDriver<RemoteWebElement> session = null;
	

	@Before
	public void setUp() throws Exception {
		// Launch a new instance of nexacro application or find nexacro application
		try {
			// Create a new session to launch nexacro application
			if(session == null)
			{
				DesiredCapabilities appCapabilities = new DesiredCapabilities();
	
				appCapabilities.setCapability("app", NexacroAppId);
				appCapabilities.setCapability("appArguments", NexacroAppAg);
				appCapabilities.setCapability("appWorkingDir", NexacroAppDr);
				appCapabilities.setCapability("platformName", TestOs);
				appCapabilities.setCapability("deviceName", TestDevice);
	
				// Request Remote WinAppDriver Service
				session = new WindowsDriver<RemoteWebElement>(new URL(WindowsApplicationDriverUrl), appCapabilities);
				session.manage().timeouts().implicitlyWait(1, TimeUnit.SECONDS);
			}

		} catch (Exception e) {
			e.printStackTrace();
		} finally {

		}
	}

	@After
	public void tearDown() throws Exception {
		if (session != null) {
			session.quit();
		}
	}

	@Test
    public void Test_01_Session() throws Exception {
        System.out.println("session : " + session);
		System.out.println("session.SessionId : " + session.getSessionId());     
    }
	
	@Test
    public void Test_02_MainFrame() throws Exception {
		System.out.println("session.Title:" + session.getTitle());
    }	
    
	@Test    
    public void Test_03_WindowHandle() throws Exception {
		System.out.println("session.CurrentWindowHandle:" + session.getWindowHandle());
		System.out.println("session.WindowHandles:" + session.getWindowHandles());
		Set<String> handles = session.getWindowHandles();
		System.out.println("session.WindowHandles[0]:" + handles.iterator().next());
		System.out.println("session.Manage().Window:" + session.manage().window());
		System.out.println("session.Manage().Window.Size:" + session.manage().window().getSize());
    }    

	@Test
    public void Test_04_WindowSwitch() throws Exception {

		String temp_WindowHandle = null;
		WebElement nexaButton00 = session.findElementByAccessibilityId("mainframe.ChildFrame00.form.Button00");
    	Thread.sleep(1000);
        nexaButton00.click();
        Thread.sleep(1000);
        System.out.println("session.Count:" + session.getWindowHandles().size());
        System.out.println("session.WindowHandles:" + session.getWindowHandle());
        temp_WindowHandle = session.getWindowHandle();
        Set<String> handles = session.getWindowHandles();
        handles.remove(session.getWindowHandle());
        session.switchTo().window(handles.iterator().next());
        System.out.println("session.WindowHandles[0]:" + handles.iterator().next());
        System.out.println("session.WindowHandles:" + session.getWindowHandle());
        Thread.sleep(1000);
        WebElement closeButton = session.findElementByAccessibilityId("test01.form.Button00");
        Assert.assertNotNull(closeButton);
        
        
        session.switchTo().window(temp_WindowHandle);
        Thread.sleep(1000);
        closeButton.click();
        
        System.out.println("session.Count:" + session.getWindowHandles().size());
        System.out.println("session.WindowHandles:" + session.getWindowHandle());
    }    

	@Test
    public void Test_05_WindowMaximize() throws Exception {
        session.manage().window().maximize();
    }	
	
	@Test	
    public void Test_06_WindowSizeChange() throws Exception {
		Dimension preSize = session.manage().window().getSize();
        session.manage().window().setPosition(new Point(50, 50));
        session.manage().window().setSize(new Dimension(800, 800));
        System.out.println("session.Manage().Window.Position:" + session.manage().window().getPosition());
        System.out.println("session.Manage().Window.Size:" + session.manage().window().getSize());
        Thread.sleep(1000);
        session.manage().window().setSize(preSize);
        System.out.println("session.Manage().Window.Size:" + session.manage().window().getSize());
    }
	
	@Test	
    public void Test_07_FindElement() throws Exception {
    	WebElement element = session.findElementByAccessibilityId("mainframe.ChildFrame00.form.Button00");
        Assert.assertNotNull(element);

        List<RemoteWebElement> elements = session.findElementsByAccessibilityId("mainframe.ChildFrame00.form");
        Assert.assertNotNull(elements);
        System.out.println("List: "+elements);

        WebElement nexaComboDrop00 = session.findElementByAccessibilityId("mainframe.ChildFrame00.form.Div00.form.Combo00.dropbutton");
        Assert.assertNotNull(nexaComboDrop00);
        WebElement nexaRadioItem1 = session.findElementByAccessibilityId("mainframe.ChildFrame00.form.Radio00.radioitem1");
        Assert.assertNotNull(nexaRadioItem1);
    }

	@Test
    public void Test_08_ElementInfo() throws Exception {
		WebElement element = session.findElementByAccessibilityId("mainframe.ChildFrame00.form.TextArea00");
        Assert.assertNotNull(element);

        System.out.println("element.GetAttribute:" + element.getAttribute("AutomationId"));
        System.out.println("element.Text:" + element.getText());
        System.out.println("element.Name:" + element.getTagName());
        System.out.println("element.TagName:" + element.getTagName());
    }   

	@Test
    public void Test_09_EditInput() throws Exception {
    	WebElement nexaEdit00 = session.findElementByAccessibilityId("mainframe.ChildFrame00.form.Edit00:input");
        Assert.assertNotNull(nexaEdit00);
        Thread.sleep(1000);

        nexaEdit00.clear();
        nexaEdit00.click();
        nexaEdit00.sendKeys("abcdeABCDE");
        Assert.assertEquals("abcdeABCDE", nexaEdit00.getText());
    }

	@Test
    public void Test_10_MaskEditInput() throws Exception {
    	WebElement nexaMaskEdit00 = session.findElementByAccessibilityId("mainframe.ChildFrame00.form.MaskEdit00");
        Assert.assertNotNull(nexaMaskEdit00);
        Thread.sleep(1000);

        nexaMaskEdit00.clear();
        nexaMaskEdit00.click();
        nexaMaskEdit00.sendKeys(Keys.CONTROL+"a");
        Thread.sleep(1000);
        nexaMaskEdit00.sendKeys("abcde1234"+Keys.ENTER);
        Assert.assertEquals("abcde-1234", nexaMaskEdit00.getText());
    }	
	
	@Test
    public void Test_11_TextAreaInput() throws Exception {
		WebElement nexaTextArea00 = session.findElementByAccessibilityId("mainframe.ChildFrame00.form.TextArea00:textarea");
        Assert.assertNotNull(nexaTextArea00);
        Thread.sleep(1000);

        nexaTextArea00.clear();
        nexaTextArea00.click();
        nexaTextArea00.sendKeys("abcdeABCDE 12345&*()_!@#$%");
        Assert.assertEquals("abcdeABCDE 12345&*()_!@#$%", nexaTextArea00.getText());
    }	
	
	@Test
    public void Test_12_CheckBoxToggle() throws Exception {
		WebElement nexaCheckBox00 = session.findElementByAccessibilityId("mainframe.ChildFrame00.form.Div00.form.CheckBox00");
        Assert.assertNotNull(nexaCheckBox00);
        Thread.sleep(1000);
        System.out.println("nexaCheckBox00.Selected:" + nexaCheckBox00.isSelected());

        // Clear CheckBox
        nexaCheckBox00.clear();
        Thread.sleep(1000);
        System.out.println("nexaCheckBox00.Selected:" + nexaCheckBox00.isSelected());

        // Toggle by Click CheckBox
        nexaCheckBox00.click();
        Thread.sleep(1000);
        System.out.println("nexaCheckBox00.Selected:" + nexaCheckBox00.isSelected());

        // Toggle by Space CheckBox
        nexaCheckBox00.sendKeys(Keys.SPACE);
        Thread.sleep(1000);
        System.out.println("nexaCheckBox00.Selected:" + nexaCheckBox00.isSelected());
    }	
	
	@Test
    public void Test_13_SpinValue() throws Exception {
		WebElement nexaSpin00 = session.findElementByAccessibilityId("mainframe.ChildFrame00.form.Div00.form.Spin00");
        Assert.assertNotNull(nexaSpin00);
        Thread.sleep(1000);

        // Spin Up by Key Up
        nexaSpin00.click();
        nexaSpin00.sendKeys(Keys.UP);
        nexaSpin00.sendKeys(Keys.UP);
        Assert.assertEquals("2", nexaSpin00.getText());

        // Search Spin Edit
        WebElement nexaSpinEd = session.findElementByAccessibilityId("mainframe.ChildFrame00.form.Div00.form.Spin00.spinedit");
        Assert.assertNotNull(nexaSpinEd);

        // Set Spin Edit Value
        nexaSpinEd.clear();
        nexaSpinEd.click();
        nexaSpinEd.sendKeys("10");
        Assert.assertEquals("10", nexaSpin00.getText());

        // Search Spin Up/Down Button
        WebElement nexaSpinUp = session.findElementByAccessibilityId("mainframe.ChildFrame00.form.Div00.form.Spin00.spinupbutton");
        WebElement nexaSpinDn = session.findElementByAccessibilityId("mainframe.ChildFrame00.form.Div00.form.Spin00.spindownbutton");
        Assert.assertNotNull(nexaSpinUp);
        Assert.assertNotNull(nexaSpinDn);

        // Spin Up by Button Click
        nexaSpinUp.click();
        nexaSpinUp.click();
        Assert.assertEquals("12", nexaSpin00.getText());

        // Spin Down by Button Click
        nexaSpinDn.click();
        nexaSpinDn.click();
        Assert.assertEquals("10", nexaSpin00.getText());
    }

	@Test
    public void Test_14_CompStatus() throws Exception {
		WebElement nexaButton00 = session.findElementByAccessibilityId("mainframe.ChildFrame00.form.Button00");
		System.out.println("nexaButton00.Displayed:" + nexaButton00.isDisplayed());
		System.out.println("nexaButton00.Enabled:" + nexaButton00.isEnabled());
		System.out.println("nexaButton00.Selected:" + nexaButton00.isSelected());
		System.out.println("nexaButton00.Location:" + nexaButton00.getLocation());
		// http://appium.io/docs/en/commands/element/attributes/location-in-view/ 
		// Not supported
		// System.out.println("nexaButton00.LocationOnScreenOnceScrolledIntoView:" + nexaButton00.LocationOnScreenOnceScrolledIntoView);
		System.out.println("nexaButton00.Size:" + nexaButton00.getSize());
		System.out.println("nexaButton00.GetAttribute:" + nexaButton00.getAttribute("AutomationId"));
    }

	@Test
    public void Test_15_Screenshot() throws Exception {
        File sessionshot = ((TakesScreenshot)session).getScreenshotAs(OutputType.FILE);
        Assert.assertNotNull(sessionshot);

        // Save Session ScreenShot to ImageFile
        //sessionshot.SaveAsFile("/SessionScreenShot.png", ImageFormat.Png);
        // 권한이 있는 저장소만 접근할 수 있음
        FileHandler.copy(sessionshot, new File(System.getProperty("user.home")+"\\Downloads\\SessionScreenShot.png"));
        System.out.println(System.getProperty("user.home"));
        // Get Session ScreenShot Base64EncodingString
        String base64str = ((TakesScreenshot)session).getScreenshotAs(OutputType.BASE64);

        WebElement element = session.findElementByAccessibilityId("mainframe.ChildFrame00.form.TextArea00");
        File elementshot = ((TakesScreenshot)element).getScreenshotAs(OutputType.FILE);
        Assert.assertNotNull(elementshot);

        // Save Element ScreenShot to ImageFile
        //elementshot.SaveAsFile("/ElementScreenShot.png", ImageFormat.Png);
        FileHandler.copy(elementshot, new File(System.getProperty("user.home")+"\\Downloads\\ElementScreenShot.png"));
        // Get Element ScreenShot Base64EncodingString
        String base64strelement = ((TakesScreenshot)element).getScreenshotAs(OutputType.BASE64);
    }
	
	@Test	
    public void Test_16_Combodropbutton() throws Exception {
        WebElement nexaCombo = session.findElementByAccessibilityId("mainframe.ChildFrame00.form.Div00.form.Combo00");
        Assert.assertNotNull(nexaCombo);
        Thread.sleep(1000);
        nexaCombo.click();
        nexaCombo.sendKeys(Keys.ESCAPE);
        Thread.sleep(1000);
        nexaCombo.sendKeys(Keys.chord(Keys.ALT, Keys.ARROW_DOWN));
        nexaCombo.sendKeys(Keys.ESCAPE);
	}
}
