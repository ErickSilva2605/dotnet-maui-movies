using MauiMovies.Core.Entities;

namespace MauiMovies.Infrastructure.Api.Mapping;

public static class MovieDtoMapper
{
	public static Movie ToDomain(this MovieDto dto) => new()
	{
		Id = dto.Id,
		Title = dto.Title ?? string.Empty,
		OriginalTitle = dto.OriginalTitle ?? string.Empty,
		Overview = dto.Overview ?? string.Empty,
		PosterPath = dto.PosterPath,
		BackdropPath = dto.BackdropPath,
		ReleaseDate = dto.ReleaseDate,
		VoteAverage = dto.VoteAverage,
		VoteCount = dto.VoteCount,
		Popularity = dto.Popularity,
		Adult = dto.Adult ?? false,
		OriginalLanguage = dto.OriginalLanguage,
		GenreIds = dto.GenreIds is { } ids ? ids.AsReadOnly() : [],
		Video = dto.Video ?? false,
	};
}
