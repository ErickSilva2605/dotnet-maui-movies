using MauiMovies.Core.ViewModels;

namespace MauiMovies.UI.Pages.Login;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
}
