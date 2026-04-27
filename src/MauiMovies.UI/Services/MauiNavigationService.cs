using MauiMovies.Core.Interfaces.Services;
using MauiMovies.UI.Navigation;

namespace MauiMovies.UI.Services;

public class MauiNavigationService : INavigationService
{
	public Task GoBackAsync() =>
		Shell.Current.GoToAsync(Routes.Up);

	public Task GoToRootAsync() =>
		Shell.Current.GoToAsync(Routes.Root);

	public Task NavigateToPreLoginAsync() =>
		Shell.Current.GoToAsync(Routes.PreLogin);

	public Task NavigateToLoginAsync() =>
		Shell.Current.GoToAsync(Routes.Login);

	public Task NavigateToProfileAsync() =>
		Shell.Current.GoToAsync(Routes.Profile);

	public Task ReplaceStackWithProfileAsync() =>
		Shell.Current.GoToAsync($"{Routes.Root}/{Routes.Profile}");

	public Task NavigateToMovieDetailsAsync(int movieId) =>
		Shell.Current.GoToAsync($"{Routes.MovieDetails}?{Routes.IdParameter}={movieId}");

	public Task NavigateToTvDetailsAsync(int tvId) =>
		Shell.Current.GoToAsync($"{Routes.TvDetails}?{Routes.IdParameter}={tvId}");

	public Task NavigateToPersonDetailsAsync(int personId) =>
		Shell.Current.GoToAsync($"{Routes.PersonDetails}?{Routes.IdParameter}={personId}");
}
