#if ANDROID || IOS
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Platform;
#endif

namespace MauiMovies.UI;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
		RequestedThemeChanged += (_, e) => ApplyStatusBar(e.RequestedTheme);
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new AppShell());
	}

	protected override void OnStart()
	{
		base.OnStart();
		ApplyStatusBar(RequestedTheme);
	}

	static void ApplyStatusBar(AppTheme theme)
	{
#if ANDROID
		if (!OperatingSystem.IsAndroidVersionAtLeast(23))
			return;
#endif
#if ANDROID || IOS
		var isDark = theme == AppTheme.Dark;
		StatusBar.SetColor(isDark ? Color.FromArgb("#031D33") : Colors.White);
		StatusBar.SetStyle(isDark ? StatusBarStyle.LightContent : StatusBarStyle.DarkContent);
#endif
	}
}