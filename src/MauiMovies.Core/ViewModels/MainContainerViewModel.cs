namespace MauiMovies.Core.ViewModels;

public partial class MainContainerViewModel : BaseViewModel
{
	[RelayCommand]
	Task NavigateToUserAsync()
	{
		return Task.CompletedTask;
	}

	[RelayCommand]
	Task SearchAsync()
	{
		return Task.CompletedTask;
	}
}
