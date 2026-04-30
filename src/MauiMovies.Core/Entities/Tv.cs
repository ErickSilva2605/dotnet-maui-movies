
namespace MauiMovies.Core.Entities;

public class Tv : MediaItem
{
	public override MediaType MediaType => MediaType.Tv;

	public required string Name { get; init; }
	public string OriginalName { get; init; } = string.Empty;
	public string Overview { get; init; } = string.Empty;
	public string? PosterPath { get; init; }
	public string? BackdropPath { get; init; }
	public string? FirstAirDate { get; init; }
	public double VoteAverage { get; init; }
	public int VoteCount { get; init; }
	public string? OriginalLanguage { get; init; }
	public IReadOnlyList<int> GenreIds { get; init; } = [];
	public IReadOnlyList<string> OriginCountry { get; init; } = [];
}
