using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiMovies.Core.Interfaces.Services;

namespace MauiMovies.Core.ViewModels;

public partial class LoginViewModel : BaseViewModel
{
	readonly IAuthService authService;
	readonly INavigationService navigationService;

	public LoginViewModel(IAuthService authService, INavigationService navigationService)
	{
		this.authService = authService;
		this.navigationService = navigationService;
	}

	[ObservableProperty] string username = string.Empty;
	[ObservableProperty] string password = string.Empty;
	[ObservableProperty] bool isBusy;
	[ObservableProperty] string? errorMessage;

	[RelayCommand]
	Task GoBackAsync() => navigationService.GoBackAsync();

	[RelayCommand]
	async Task SignInAsync()
	{
		if (IsBusy)
			return;

		IsBusy = true;
		ErrorMessage = null;

		try
		{
			var success = await authService.SignInAsync(Username, Password);

			if (success)
				await navigationService.NavigateToProfileAsync();
			else
				ErrorMessage = "Invalid credentials";
		}
		catch (Exception ex)
		{
			ErrorMessage = ex.Message;
		}
		finally
		{
			IsBusy = false;
		}
	}
}
