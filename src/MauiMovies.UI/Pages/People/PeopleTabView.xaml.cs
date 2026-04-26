using MauiMovies.UI.Controls.Navigation;

namespace MauiMovies.UI.Pages.People;

public partial class PeopleTabView : ContentView, ITabLifecycle
{
	public PeopleTabView()
	{
		InitializeComponent();
	}

	public Task OnTabActivatedAsync() => Task.CompletedTask;

	public void OnTabDeactivated() { }
}
