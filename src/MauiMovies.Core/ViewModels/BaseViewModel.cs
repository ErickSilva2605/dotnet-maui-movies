namespace MauiMovies.Core.ViewModels;

public abstract class BaseViewModel : ObservableObject, IPageLifecycle, INavigationParametersAware
{
	public virtual Task OnAppearingAsync() => Task.CompletedTask;

	public virtual Task OnDisappearingAsync() => Task.CompletedTask;

	public virtual void ApplyParameters(IDictionary<string, object> parameters) { }
}
