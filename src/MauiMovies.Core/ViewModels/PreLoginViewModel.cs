using CommunityToolkit.Mvvm.Input;
using MauiMovies.Core.Interfaces.Services;

namespace MauiMovies.Core.ViewModels;

public partial class PreLoginViewModel : BaseViewModel
{
	readonly INavigationService navigationService;

	public PreLoginViewModel(INavigationService navigationService)
	{
		this.navigationService = navigationService;
	}

	[RelayCommand]
	Task GoBackAsync() => navigationService.GoBackAsync();

	[RelayCommand]
	Task NavigateToLoginAsync() => navigationService.NavigateToLoginAsync();
}
