using MauiMovies.Core.Entities;
using MauiMovies.Core.Models;

namespace MauiMovies.Core.Mapping;

public static class MediaItemModelMapper
{
	public static MediaItemModel? ToModel(this MediaItem item) => item switch
	{
		Movie movie => movie.ToModel(),
		Tv tv => tv.ToModel(),
		Person person => person.ToModel(),
		_ => null,
	};
}
