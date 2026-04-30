
namespace MauiMovies.Core.Mapping;

public static class MovieModelMapper
{
	public static MovieModel ToModel(this Movie movie) => new()
	{
		Id = movie.Id,
		Title = movie.Title,
		OriginalTitle = movie.OriginalTitle,
		Overview = movie.Overview,
		PosterPath = movie.PosterPath,
		BackdropPath = movie.BackdropPath,
		ReleaseDate = movie.ReleaseDate,
		VoteAverage = movie.VoteAverage,
		VoteCount = movie.VoteCount,
		Popularity = movie.Popularity,
		Adult = movie.Adult,
		OriginalLanguage = movie.OriginalLanguage,
		GenreIds = movie.GenreIds,
		Video = movie.Video,
	};
}
