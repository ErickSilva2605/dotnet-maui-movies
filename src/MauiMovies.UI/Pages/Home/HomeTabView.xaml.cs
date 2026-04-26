using MauiMovies.UI.Controls.Navigation;

namespace MauiMovies.UI.Pages.Home;

public partial class HomeTabView : ContentView, ITabLifecycle
{
	public HomeTabView()
	{
		InitializeComponent();
	}

	public Task OnTabActivatedAsync() => Task.CompletedTask;

	public void OnTabDeactivated() { }
}
