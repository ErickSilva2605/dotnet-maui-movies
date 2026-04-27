using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiMovies.Core.Interfaces.Services;

namespace MauiMovies.Core.ViewModels;

public partial class PersonDetailsViewModel : BaseViewModel
{
	const string idKey = "id";

	readonly INavigationService navigationService;

	public PersonDetailsViewModel(INavigationService navigationService)
	{
		this.navigationService = navigationService;
	}

	[ObservableProperty] int personId;

	public override void ApplyParameters(IDictionary<string, object> parameters)
	{
		if (parameters.TryGetValue(idKey, out var raw) && int.TryParse(raw?.ToString(), out var id))
			PersonId = id;
	}

	[RelayCommand]
	Task GoBackAsync() => navigationService.GoBackAsync();
}
