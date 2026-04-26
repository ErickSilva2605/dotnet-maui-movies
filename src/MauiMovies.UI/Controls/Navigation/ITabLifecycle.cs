namespace MauiMovies.UI.Controls.Navigation;

interface ITabLifecycle
{
	Task OnTabActivatedAsync();
	void OnTabDeactivated();
}
