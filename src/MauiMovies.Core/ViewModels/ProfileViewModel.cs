using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiMovies.Core.Interfaces.Services;

namespace MauiMovies.Core.ViewModels;

public partial class ProfileViewModel : BaseViewModel
{
	readonly IAuthService authService;
	readonly INavigationService navigationService;

	public ProfileViewModel(IAuthService authService, INavigationService navigationService)
	{
		this.authService = authService;
		this.navigationService = navigationService;
		username = authService.Username;
	}

	[ObservableProperty] string? username;

	[RelayCommand]
	Task GoBackAsync() => navigationService.GoBackAsync();

	[RelayCommand]
	async Task SignOutAsync()
	{
		await authService.SignOutAsync();
		await navigationService.GoToRootAsync();
	}
}
