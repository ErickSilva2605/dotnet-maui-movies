using MauiMovies.UI.Controls.Navigation;

namespace MauiMovies.UI.Pages.Awards;

public partial class AwardsTabView : ContentView, ITabLifecycle
{
	public AwardsTabView()
	{
		InitializeComponent();
	}

	public Task OnTabActivatedAsync() => Task.CompletedTask;

	public void OnTabDeactivated() { }
}
