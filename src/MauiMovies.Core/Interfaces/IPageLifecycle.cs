namespace MauiMovies.Core.Interfaces;

public interface IPageLifecycle
{
	Task OnAppearingAsync();
	Task OnDisappearingAsync();
}
