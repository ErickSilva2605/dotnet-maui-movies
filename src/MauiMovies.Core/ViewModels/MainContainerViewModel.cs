using CommunityToolkit.Mvvm.Input;
using MauiMovies.Core.Interfaces.Services;

namespace MauiMovies.Core.ViewModels;

public partial class MainContainerViewModel : BaseViewModel
{
	readonly INavigationService navigationService;
	readonly IAuthService authService;

	public MainContainerViewModel(INavigationService navigationService, IAuthService authService)
	{
		this.navigationService = navigationService;
		this.authService = authService;
	}

	[RelayCommand]
	Task NavigateToUserAsync() =>
		authService.IsAuthenticated
			? navigationService.NavigateToProfileAsync()
			: navigationService.NavigateToPreLoginAsync();

	[RelayCommand]
	Task SearchAsync() => Task.CompletedTask;
}
