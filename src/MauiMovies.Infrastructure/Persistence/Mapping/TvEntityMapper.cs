using MauiMovies.Core.Entities;
using MauiMovies.Infrastructure.Persistence.Entities;

namespace MauiMovies.Infrastructure.Persistence.Mapping;

public static class TvEntityMapper
{
	public static Tv ToDomain(this TvEntity entity) => new()
	{
		Id = entity.Id,
		Name = entity.Name,
		OriginalName = entity.OriginalName,
		Overview = entity.Overview,
		PosterPath = entity.PosterPath,
		BackdropPath = entity.BackdropPath,
		FirstAirDate = entity.FirstAirDate,
		VoteAverage = entity.VoteAverage,
		VoteCount = entity.VoteCount,
		Popularity = entity.Popularity,
		Adult = entity.Adult,
		OriginalLanguage = entity.OriginalLanguage,
		GenreIds = ParseIntCsv(entity.GenreIds),
		OriginCountry = ParseStringCsv(entity.OriginCountry),
	};

	public static TvEntity ToEntity(this Tv domain) => new()
	{
		Id = domain.Id,
		Name = domain.Name,
		OriginalName = domain.OriginalName,
		Overview = domain.Overview,
		PosterPath = domain.PosterPath,
		BackdropPath = domain.BackdropPath,
		FirstAirDate = domain.FirstAirDate,
		VoteAverage = domain.VoteAverage,
		VoteCount = domain.VoteCount,
		Popularity = domain.Popularity,
		Adult = domain.Adult,
		OriginalLanguage = domain.OriginalLanguage,
		GenreIds = string.Join(',', domain.GenreIds),
		OriginCountry = string.Join(',', domain.OriginCountry),
	};

	static IReadOnlyList<int> ParseIntCsv(string csv) =>
		string.IsNullOrEmpty(csv)
			? []
			: csv.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

	static IReadOnlyList<string> ParseStringCsv(string csv) =>
		string.IsNullOrEmpty(csv)
			? []
			: csv.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
}
