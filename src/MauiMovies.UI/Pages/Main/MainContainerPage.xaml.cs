using MauiMovies.Core.Interfaces;
using MauiMovies.Core.ViewModels;
using MauiMovies.UI.Navigation;
using MauiMovies.UI.Pages.Awards;
using MauiMovies.UI.Pages.Home;
using MauiMovies.UI.Pages.Movies;
using MauiMovies.UI.Pages.People;
using MauiMovies.UI.Pages.Tv;

namespace MauiMovies.UI.Pages.Main;

public partial class MainContainerPage : ContentPage
{
	readonly ContentView?[] tabViews = new ContentView?[5];
	int currentTab;

	public MainContainerPage()
	{
		BindingContext = new MainContainerViewModel();
		InitializeComponent();
		bottomBar.TabSelected += OnTabSelected;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await ActivateTabAsync(currentTab, animate: false);
	}

	async void OnTabSelected(object? sender, AppTab tab) =>
		await ActivateTabAsync((int)tab, animate: true);

	async Task ActivateTabAsync(int index, bool animate)
	{
		var previous = tabViews[currentTab];

		if (previous?.BindingContext is IPageLifecycle previousLifecycle)
			await previousLifecycle.OnDisappearingAsync();

		if (tabViews[index] is null)
		{
			var view = CreateTabView(index);
			tabViews[index] = view;
			tabContainer.Children.Add(view);
		}

		foreach (var view in tabViews)
			if (view is not null)
				view.IsVisible = false;

		tabViews[index]!.IsVisible = true;
		currentTab = index;

		if (tabViews[index]?.BindingContext is IPageLifecycle lifecycle)
			await lifecycle.OnAppearingAsync();
	}

	static ContentView CreateTabView(int index) =>
		index switch
		{
			0 => new HomeTabView(),
			1 => new MoviesTabView(),
			2 => new TvTabView(),
			3 => new PeopleTabView(),
			4 => new AwardsTabView(),
			_ => new HomeTabView()
		};
}
