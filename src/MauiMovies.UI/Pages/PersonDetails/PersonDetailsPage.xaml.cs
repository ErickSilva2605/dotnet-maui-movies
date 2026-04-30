
namespace MauiMovies.UI.Pages.PersonDetails;

public partial class PersonDetailsPage : ContentPage, IQueryAttributable
{
	readonly PersonDetailsViewModel viewModel;

	public PersonDetailsPage(PersonDetailsViewModel viewModel)
	{
		this.viewModel = viewModel;
		BindingContext = viewModel;
		InitializeComponent();
	}

	public void ApplyQueryAttributes(IDictionary<string, object> query) =>
		viewModel.ApplyParameters(query);
}
