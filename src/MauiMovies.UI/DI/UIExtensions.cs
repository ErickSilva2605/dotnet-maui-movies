using MauiMovies.Core.UseCases;
using MauiMovies.Core.ViewModels;
using MauiMovies.UI.Pages.Awards;
using MauiMovies.UI.Pages.Home;
using MauiMovies.UI.Pages.Main;
using MauiMovies.UI.Pages.Movies;
using MauiMovies.UI.Pages.People;
using MauiMovies.UI.Pages.Tv;

namespace MauiMovies.UI.DI;

public static class UIExtensions
{
	public static IServiceCollection AddUI(this IServiceCollection services)
	{
		AddUseCases(services);
		AddViewModels(services);
		AddPages(services);

		return services;
	}

	static void AddUseCases(IServiceCollection services)
	{
		services.AddTransient<GetTrendingAllUseCase>();
	}

	static void AddViewModels(IServiceCollection services)
	{
		services.AddTransient<MainContainerViewModel>();
		services.AddTransient<HomeViewModel>();
	}

	static void AddPages(IServiceCollection services)
	{
		services.AddTransient<MainContainerPage>();
		services.AddTransient<HomeTabView>();
		services.AddTransient<MoviesTabView>();
		services.AddTransient<TvTabView>();
		services.AddTransient<PeopleTabView>();
		services.AddTransient<AwardsTabView>();
	}
}
