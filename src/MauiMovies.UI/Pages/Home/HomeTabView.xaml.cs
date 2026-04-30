
namespace MauiMovies.UI.Pages.Home;

public partial class HomeTabView : ContentView
{
	public HomeTabView(HomeViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
}
