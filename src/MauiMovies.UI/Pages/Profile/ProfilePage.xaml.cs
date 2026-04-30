
namespace MauiMovies.UI.Pages.Profile;

public partial class ProfilePage : ContentPage
{
	public ProfilePage(ProfileViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
}
