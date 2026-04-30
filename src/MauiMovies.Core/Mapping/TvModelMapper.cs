
namespace MauiMovies.Core.Mapping;

public static class TvModelMapper
{
	public static TvModel ToModel(this Tv tv) => new()
	{
		Id = tv.Id,
		Name = tv.Name,
		OriginalName = tv.OriginalName,
		Overview = tv.Overview,
		PosterPath = tv.PosterPath,
		BackdropPath = tv.BackdropPath,
		FirstAirDate = tv.FirstAirDate,
		VoteAverage = tv.VoteAverage,
		VoteCount = tv.VoteCount,
		Popularity = tv.Popularity,
		Adult = tv.Adult,
		OriginalLanguage = tv.OriginalLanguage,
		GenreIds = tv.GenreIds,
		OriginCountry = tv.OriginCountry,
	};
}
