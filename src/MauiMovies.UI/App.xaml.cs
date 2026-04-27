using MauiMovies.Core.Interfaces.Services;
using MauiMovies.Infrastructure.Persistence;
#if ANDROID || IOS
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Platform;
#endif

namespace MauiMovies.UI;

public partial class App : Application
{
	readonly IServiceProvider services;

	public App(IServiceProvider services)
	{
		this.services = services;
		InitializeComponent();
		RequestedThemeChanged += (_, e) => ApplyStatusBar(e.RequestedTheme);
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new AppShell());
	}

	protected override async void OnStart()
	{
		base.OnStart();
		ApplyStatusBar(RequestedTheme);

		try
		{
			var initializer = services.GetRequiredService<DatabaseInitializer>();
			await initializer.InitializeAsync();
		}
		catch (Exception ex)
		{
			System.Diagnostics.Debug.WriteLine($"[App.OnStart] Database initialization failed: {ex}");
		}

		try
		{
			var authService = services.GetRequiredService<IAuthService>();
			await authService.InitializeAsync();
		}
		catch (Exception ex)
		{
			System.Diagnostics.Debug.WriteLine($"[App.OnStart] Auth initialization failed: {ex}");
		}
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
