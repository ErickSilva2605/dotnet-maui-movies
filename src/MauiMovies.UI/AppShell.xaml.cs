using MauiMovies.UI.Navigation;
using MauiMovies.UI.Pages.Login;
using MauiMovies.UI.Pages.PreLogin;
using MauiMovies.UI.Pages.Profile;

namespace MauiMovies.UI;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		RegisterRoutes();
	}

	static void RegisterRoutes()
	{
		Routing.RegisterRoute(Routes.PreLogin, typeof(PreLoginPage));
		Routing.RegisterRoute(Routes.Login, typeof(LoginPage));
		Routing.RegisterRoute(Routes.Profile, typeof(ProfilePage));
	}
}
