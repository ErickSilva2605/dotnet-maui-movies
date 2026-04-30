using MauiMovies.Core.Interfaces.Services;
using MauiMovies.Core.UseCases;
using MauiMovies.UI.Pages.Awards;
using MauiMovies.UI.Pages.Home;
using MauiMovies.UI.Pages.Login;
using MauiMovies.UI.Pages.Main;
using MauiMovies.UI.Pages.MovieDetails;
using MauiMovies.UI.Pages.Movies;
using MauiMovies.UI.Pages.People;
using MauiMovies.UI.Pages.PersonDetails;
using MauiMovies.UI.Pages.PreLogin;
using MauiMovies.UI.Pages.Profile;
using MauiMovies.UI.Pages.Tv;
using MauiMovies.UI.Pages.TvDetails;
using MauiMovies.UI.Services;

namespace MauiMovies.UI.DI;

public static class UIExtensions
{
	public static IServiceCollection AddUI(this IServiceCollection services)
	{
		AddServices(services);
		AddUseCases(services);
		AddViewModels(services);
		AddPages(services);

		return services;
	}

	static void AddServices(IServiceCollection services)
	{
		services.AddSingleton<IAuthService, MauiAuthService>();
		services.AddSingleton<INavigationService, MauiNavigationService>();
	}

	static void AddUseCases(IServiceCollection services)
	{
		services.AddTransient<GetTrendingAllUseCase>();
	}

	static void AddViewModels(IServiceCollection services)
	{
		services.AddTransient<MainContainerViewModel>();
		services.AddTransient<HomeViewModel>();
		services.AddTransient<PreLoginViewModel>();
		services.AddTransient<LoginViewModel>();
		services.AddTransient<ProfileViewModel>();
		services.AddTransient<MovieDetailsViewModel>();
		services.AddTransient<TvDetailsViewModel>();
		services.AddTransient<PersonDetailsViewModel>();
	}

	static void AddPages(IServiceCollection services)
	{
		services.AddTransient<MainContainerPage>();
		services.AddTransient<HomeTabView>();
		services.AddTransient<MoviesTabView>();
		services.AddTransient<TvTabView>();
		services.AddTransient<PeopleTabView>();
		services.AddTransient<AwardsTabView>();
		services.AddTransient<PreLoginPage>();
		services.AddTransient<LoginPage>();
		services.AddTransient<ProfilePage>();
		services.AddTransient<MovieDetailsPage>();
		services.AddTransient<TvDetailsPage>();
		services.AddTransient<PersonDetailsPage>();
	}
}
