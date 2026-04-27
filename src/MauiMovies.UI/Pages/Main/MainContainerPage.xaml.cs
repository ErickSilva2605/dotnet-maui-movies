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
	readonly IServiceProvider services;
	readonly ContentView?[] tabViews = new ContentView?[5];
	int currentTab;

	public MainContainerPage(IServiceProvider services, MainContainerViewModel viewModel)
	{
		this.services = services;
		BindingContext = viewModel;
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

	ContentView CreateTabView(int index) =>
		index switch
		{
			0 => services.GetRequiredService<HomeTabView>(),
			1 => services.GetRequiredService<MoviesTabView>(),
			2 => services.GetRequiredService<TvTabView>(),
			3 => services.GetRequiredService<PeopleTabView>(),
			4 => services.GetRequiredService<AwardsTabView>(),
			_ => throw new ArgumentOutOfRangeException(nameof(index), index, "Tab index out of range"),
		};
}
