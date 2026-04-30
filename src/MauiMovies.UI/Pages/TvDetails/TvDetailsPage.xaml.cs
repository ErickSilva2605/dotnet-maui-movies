
namespace MauiMovies.UI.Pages.TvDetails;

public partial class TvDetailsPage : ContentPage, IQueryAttributable
{
	readonly TvDetailsViewModel viewModel;

	public TvDetailsPage(TvDetailsViewModel viewModel)
	{
		this.viewModel = viewModel;
		BindingContext = viewModel;
		InitializeComponent();
	}

	public void ApplyQueryAttributes(IDictionary<string, object> query) =>
		viewModel.ApplyParameters(query);
}
