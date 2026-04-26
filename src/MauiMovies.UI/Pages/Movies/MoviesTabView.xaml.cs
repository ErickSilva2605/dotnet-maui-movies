using MauiMovies.UI.Controls.Navigation;

namespace MauiMovies.UI.Pages.Movies;

public partial class MoviesTabView : ContentView, ITabLifecycle
{
	public MoviesTabView()
	{
		InitializeComponent();
	}

	public Task OnTabActivatedAsync() => Task.CompletedTask;

	public void OnTabDeactivated() { }
}
