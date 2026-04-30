
namespace MauiMovies.Core.ViewModels;

public partial class MovieDetailsViewModel : BaseViewModel
{
	const string idKey = "id";

	readonly INavigationService navigationService;

	public MovieDetailsViewModel(INavigationService navigationService)
	{
		this.navigationService = navigationService;
	}

	[ObservableProperty] int movieId;

	public override void ApplyParameters(IDictionary<string, object> parameters)
	{
		if (parameters.TryGetValue(idKey, out var raw) && int.TryParse(raw?.ToString(), out var id))
			MovieId = id;
	}

	[RelayCommand]
	Task GoBackAsync() => navigationService.GoBackAsync();
}
