using MauiMovies.Core.Models;
using MauiMovies.Core.ViewModels;

namespace MauiMovies.UI.Pages.Home;

public partial class HomeTabView : ContentView
{
	public HomeTabView(HomeViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}

	async void OnTrendingSelectionChanged(object? sender, SelectionChangedEventArgs e)
	{
		if (sender is not CollectionView collectionView || BindingContext is not HomeViewModel viewModel)
			return;

		if (e.CurrentSelection.FirstOrDefault() is MediaItemModel item)
			await viewModel.NavigateToMediaItemCommand.ExecuteAsync(item);

		collectionView.SelectedItem = null;
	}
}
