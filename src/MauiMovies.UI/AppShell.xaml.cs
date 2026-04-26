using MauiMovies.UI.Controls.Navigation;
using MauiMovies.UI.Pages.Awards;
using MauiMovies.UI.Pages.Home;
using MauiMovies.UI.Pages.Movies;
using MauiMovies.UI.Pages.People;
using MauiMovies.UI.Pages.Tv;

namespace MauiMovies.UI;

public partial class AppShell : Shell
{
	CustomTabBarView? tabBarView;

	public AppShell()
	{
		InitializeComponent();
		Loaded += OnLoaded;
		Navigated += OnNavigated;
	}

	void OnLoaded(object? sender, EventArgs e)
	{
		if (tabBarView is not null)
			return;

		if (Handler?.MauiContext is not { } mauiContext)
			return;

		tabBarView = new CustomTabBarView();
		tabBarView.TabSelected += OnTabSelected;
		TabBarInjector.Inject(tabBarView, mauiContext);
	}

	void OnNavigated(object? sender, ShellNavigatedEventArgs e)
	{
		if (tabBarView is null)
			return;

		var tab = CurrentPage switch
		{
			HomePage   => (AppTab?)AppTab.Home,
			MoviesPage => AppTab.Movies,
			TvPage     => AppTab.Tv,
			PeoplePage => AppTab.People,
			AwardsPage => AppTab.Awards,
			_          => null
		};

		tabBarView.IsVisible = tab.HasValue;

		if (tab is { } value)
			tabBarView.SelectTab(value, animate: false);
	}

	void OnTabSelected(object? sender, AppTab tab)
	{
		var route = tab switch
		{
			AppTab.Home   => Routes.Home,
			AppTab.Movies => Routes.Movies,
			AppTab.Tv     => Routes.Tv,
			AppTab.People => Routes.People,
			AppTab.Awards => Routes.Awards,
			_             => Routes.Home
		};

		var target = FindShellSectionByRoute(route);
		if (target is not null)
			CurrentItem.CurrentItem = target;
	}

	ShellSection? FindShellSectionByRoute(string route) =>
		Items.SelectMany(item => item.Items)
		     .FirstOrDefault(section => section.Items.Any(content => content.Route == route));
}
