
namespace MauiMovies.Infrastructure.Api.Mapping;

public static class TvDtoMapper
{
	public static Tv ToDomain(this TvDto dto) => new()
	{
		Id = dto.Id,
		Name = dto.Name ?? string.Empty,
		OriginalName = dto.OriginalName ?? string.Empty,
		Overview = dto.Overview ?? string.Empty,
		PosterPath = dto.PosterPath,
		BackdropPath = dto.BackdropPath,
		FirstAirDate = dto.FirstAirDate,
		VoteAverage = dto.VoteAverage,
		VoteCount = dto.VoteCount,
		Popularity = dto.Popularity,
		Adult = dto.Adult ?? false,
		OriginalLanguage = dto.OriginalLanguage,
		GenreIds = dto.GenreIds is { } ids ? ids.AsReadOnly() : [],
		OriginCountry = dto.OriginCountry is { } countries ? countries.AsReadOnly() : [],
	};
}
