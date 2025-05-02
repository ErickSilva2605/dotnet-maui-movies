using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.iOS;

namespace MauiMovies.UITests;

public class AppiumSetup : IDisposable
{
	static AppiumDriver? driver;
	bool disposed = false;

	public static AppiumDriver App => driver ?? throw new NullReferenceException("AppiumDriver is null");

	public AppiumSetup()
	{
		// If you started an Appium server manually, make sure to comment out the next line
		// This line starts a local Appium server for you as part of the test run
		AppiumServerHelper.StartAppiumLocalServer();
		var iOSOptions = new AppiumOptions
		{
			// Specify XCUITest as the driver, typically don't need to change this
			AutomationName = "XCUITest",
			// Always iOS for iOS
			PlatformName = "iOS",
			// iOS Version
			PlatformVersion = "17.0",
			// Don't specify if you don't want a specific device
			DeviceName = "iPhone 15 Pro",
			// The full path to the .app file to test or the bundle id if the app is already installed on the device
			App = "com.companyname.applicationname",
		};

		// Note there are many more options that you can use to influence the app under test according to your needs

		driver = new IOSDriver(iOSOptions);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposed || !disposing)
			return;

		driver?.Quit();
		AppiumServerHelper.DisposeAppiumLocalServer();
		disposed = true;
	}

	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}
}