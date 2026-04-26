using MauiMovies.UI.Controls.Navigation;

namespace MauiMovies.UI.Pages.Tv;

public partial class TvTabView : ContentView, ITabLifecycle
{
	public TvTabView()
	{
		InitializeComponent();
	}

	public Task OnTabActivatedAsync() => Task.CompletedTask;

	public void OnTabDeactivated() { }
}
