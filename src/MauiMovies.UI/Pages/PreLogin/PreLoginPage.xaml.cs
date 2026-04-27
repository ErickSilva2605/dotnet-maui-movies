using MauiMovies.Core.ViewModels;

namespace MauiMovies.UI.Pages.PreLogin;

public partial class PreLoginPage : ContentPage
{
	public PreLoginPage(PreLoginViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
}
