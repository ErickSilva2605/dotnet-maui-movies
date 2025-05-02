using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.Mac;

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
		var macOptions = new AppiumOptions
		{
			// Specify mac2 as the driver, typically don't need to change this
			AutomationName = "mac2",
			// Always Mac for Mac
			PlatformName = "Mac",
			// The full path to the .app file to test
			App = "/path/to/MauiApp/bin/Debug/net8.0-maccatalyst/maccatalyst-x64/BasicAppiumSample.app",
		};

		// Setting the Bundle ID is required, else the automation will run on Finder
		macOptions.AddAdditionalAppiumOption(IOSMobileCapabilityType.BundleId, "com.companyname.applicationname");

		// Note there are many more options that you can use to influence the app under test according to your needs

		driver = new MacDriver(macOptions);
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