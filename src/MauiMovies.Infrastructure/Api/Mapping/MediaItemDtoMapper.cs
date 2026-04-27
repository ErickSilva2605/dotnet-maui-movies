using MauiMovies.Core.Entities;

namespace MauiMovies.Infrastructure.Api.Mapping;

public static class MediaItemDtoMapper
{
	public static MediaItem? ToMediaItem(this BaseDto dto) => dto switch
	{
		MovieDto movie => movie.ToDomain(),
		TvDto tv => tv.ToDomain(),
		PersonDto person => person.ToDomain(),
		_ => null,
	};
}
