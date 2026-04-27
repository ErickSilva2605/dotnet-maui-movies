using Xunit;

namespace MauiMovies.UITests;

public class LoginFlowTest : BaseTest
{
	const string username = "erick";
	const string password = "test123";

	[Fact]
	public async Task FullLoginAndLogoutFlow()
	{
		await Task.Delay(2000); // Wait for app launch + initial trending load

		// Step 1: Home → tap user icon → PreLogin
		FindUIElement("TopBar_UserButton").Click();
		await Task.Delay(1000);
		App.GetScreenshot().SaveAsFile($"{nameof(FullLoginAndLogoutFlow)}_01_prelogin.png");

		// Step 2: PreLogin → tap Login button → LoginPage
		FindUIElement("PreLogin_LoginButton").Click();
		await Task.Delay(1000);
		App.GetScreenshot().SaveAsFile($"{nameof(FullLoginAndLogoutFlow)}_02_login.png");

		// Step 3: Fill credentials
		FindUIElement("Login_UsernameField").SendKeys(username);
		FindUIElement("Login_PasswordField").SendKeys(password);

		// Step 4: Tap Sign In → Profile (stack replaced — back stack is just Main)
		FindUIElement("Login_SignInButton").Click();
		await Task.Delay(2000);
		App.GetScreenshot().SaveAsFile($"{nameof(FullLoginAndLogoutFlow)}_03_profile.png");

		// Step 5: Verify Profile shows the username
		var usernameLabel = FindUIElement("Profile_UsernameLabel");
		Assert.Contains(username, usernameLabel.Text);

		// Step 6: Logout → back to Home (root)
		FindUIElement("Profile_LogoutButton").Click();
		await Task.Delay(1500);
		App.GetScreenshot().SaveAsFile($"{nameof(FullLoginAndLogoutFlow)}_04_home.png");

		// Step 7: Verify back at Home — TopBar visible
		var userButtonAgain = FindUIElement("TopBar_UserButton");
		Assert.NotNull(userButtonAgain);
	}

	[Fact]
	public async Task PreLoginBackButton_ReturnsToHome()
	{
		await Task.Delay(2000);

		FindUIElement("TopBar_UserButton").Click();
		await Task.Delay(1000);

		FindUIElement("PreLogin_BackButton").Click();
		await Task.Delay(1000);

		// Should be back on Home — TopBar user button visible
		var userButton = FindUIElement("TopBar_UserButton");
		Assert.NotNull(userButton);

		App.GetScreenshot().SaveAsFile($"{nameof(PreLoginBackButton_ReturnsToHome)}.png");
	}
}
