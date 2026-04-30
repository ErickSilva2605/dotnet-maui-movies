using MauiMovies.Core.ViewModels;

namespace MauiMovies.UI.Pages;

public abstract class BasePage<TViewModel> : ContentPage, IQueryAttributable
	where TViewModel : BaseViewModel
{
	protected TViewModel ViewModel => (TViewModel)BindingContext;

	void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query) =>
		ViewModel?.ApplyParameters(query);

	protected override void OnAppearing()
	{
		base.OnAppearing();
		_ = ViewModel?.OnAppearingAsync();
	}

	protected override void OnDisappearing()
	{
		base.OnDisappearing();
		_ = ViewModel?.OnDisappearingAsync();
	}
}
