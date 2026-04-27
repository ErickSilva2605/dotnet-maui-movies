using MauiMovies.Core.ViewModels;

namespace MauiMovies.UI.Pages.MovieDetails;

public partial class MovieDetailsPage : ContentPage, IQueryAttributable
{
	readonly MovieDetailsViewModel viewModel;

	public MovieDetailsPage(MovieDetailsViewModel viewModel)
	{
		this.viewModel = viewModel;
		BindingContext = viewModel;
		InitializeComponent();
	}

	public void ApplyQueryAttributes(IDictionary<string, object> query) =>
		viewModel.ApplyParameters(query);
}
