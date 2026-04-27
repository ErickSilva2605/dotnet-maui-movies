using Xunit;

namespace MauiMovies.UITests;

public class SmokeTest : BaseTest
{
	[Fact]
	public void AppLaunches()
	{
		App.GetScreenshot().SaveAsFile($"{nameof(AppLaunches)}.png");
	}
}
