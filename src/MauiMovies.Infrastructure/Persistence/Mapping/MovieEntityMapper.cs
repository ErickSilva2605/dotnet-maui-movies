
namespace MauiMovies.Infrastructure.Persistence.Mapping;

public static class MovieEntityMapper
{
	public static Movie ToDomain(this MovieEntity entity) => new()
	{
		Id = entity.Id,
		Title = entity.Title,
		OriginalTitle = entity.OriginalTitle,
		Overview = entity.Overview,
		PosterPath = entity.PosterPath,
		BackdropPath = entity.BackdropPath,
		ReleaseDate = entity.ReleaseDate,
		VoteAverage = entity.VoteAverage,
		VoteCount = entity.VoteCount,
		Popularity = entity.Popularity,
		Adult = entity.Adult,
		OriginalLanguage = entity.OriginalLanguage,
		GenreIds = ParseIntCsv(entity.GenreIds),
		Video = entity.Video,
	};

	public static MovieEntity ToEntity(this Movie domain) => new()
	{
		Id = domain.Id,
		Title = domain.Title,
		OriginalTitle = domain.OriginalTitle,
		Overview = domain.Overview,
		PosterPath = domain.PosterPath,
		BackdropPath = domain.BackdropPath,
		ReleaseDate = domain.ReleaseDate,
		VoteAverage = domain.VoteAverage,
		VoteCount = domain.VoteCount,
		Popularity = domain.Popularity,
		Adult = domain.Adult,
		OriginalLanguage = domain.OriginalLanguage,
		GenreIds = string.Join(',', domain.GenreIds),
		Video = domain.Video,
	};

	static IReadOnlyList<int> ParseIntCsv(string csv) =>
		string.IsNullOrEmpty(csv)
			? []
			: csv.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
}
