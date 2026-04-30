
namespace MauiMovies.Core.ViewModels;

public partial class TvDetailsViewModel : BaseViewModel
{
	const string idKey = "id";

	readonly INavigationService navigationService;

	public TvDetailsViewModel(INavigationService navigationService)
	{
		this.navigationService = navigationService;
	}

	[ObservableProperty] int tvId;

	public override void ApplyParameters(IDictionary<string, object> parameters)
	{
		if (parameters.TryGetValue(idKey, out var raw) && int.TryParse(raw?.ToString(), out var id))
			TvId = id;
	}

	[RelayCommand]
	Task GoBackAsync() => navigationService.GoBackAsync();
}
