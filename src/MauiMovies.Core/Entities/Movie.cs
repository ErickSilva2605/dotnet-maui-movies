
namespace MauiMovies.Core.Entities;

public class Movie : MediaItem
{
	public override MediaType MediaType => MediaType.Movie;

	public required string Title { get; init; }
	public string OriginalTitle { get; init; } = string.Empty;
	public string Overview { get; init; } = string.Empty;
	public string? PosterPath { get; init; }
	public string? BackdropPath { get; init; }
	public string? ReleaseDate { get; init; }
	public double VoteAverage { get; init; }
	public int VoteCount { get; init; }
	public string? OriginalLanguage { get; init; }
	public IReadOnlyList<int> GenreIds { get; init; } = [];
	public bool Video { get; init; }
}
