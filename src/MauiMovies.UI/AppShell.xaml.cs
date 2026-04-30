using MauiMovies.UI.Pages.Login;
using MauiMovies.UI.Pages.MovieDetails;
using MauiMovies.UI.Pages.PersonDetails;
using MauiMovies.UI.Pages.PreLogin;
using MauiMovies.UI.Pages.Profile;
using MauiMovies.UI.Pages.TvDetails;

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
		Routing.RegisterRoute(Routes.MovieDetails, typeof(MovieDetailsPage));
		Routing.RegisterRoute(Routes.TvDetails, typeof(TvDetailsPage));
		Routing.RegisterRoute(Routes.PersonDetails, typeof(PersonDetailsPage));
	}
}
