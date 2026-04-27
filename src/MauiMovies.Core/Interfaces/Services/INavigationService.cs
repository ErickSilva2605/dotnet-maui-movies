namespace MauiMovies.Core.Interfaces.Services;

public interface INavigationService
{
	Task GoBackAsync();
	Task GoToRootAsync();
	Task NavigateToPreLoginAsync();
	Task NavigateToLoginAsync();
	Task NavigateToProfileAsync();
	Task ReplaceStackWithProfileAsync();
	Task NavigateToMovieDetailsAsync(int movieId);
	Task NavigateToTvDetailsAsync(int tvId);
	Task NavigateToPersonDetailsAsync(int personId);
}
