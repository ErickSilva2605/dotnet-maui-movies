
namespace MauiMovies.Core.Entities;

public abstract class MediaItem
{
	public required int Id { get; init; }
	public abstract MediaType MediaType { get; }
	public double Popularity { get; init; }
	public bool Adult { get; init; }
}
