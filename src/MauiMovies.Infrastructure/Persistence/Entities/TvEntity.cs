namespace MauiMovies.Infrastructure.Persistence.Entities;

public class TvEntity
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string OriginalName { get; set; } = string.Empty;
	public string Overview { get; set; } = string.Empty;
	public string? PosterPath { get; set; }
	public string? BackdropPath { get; set; }
	public string? FirstAirDate { get; set; }
	public double VoteAverage { get; set; }
	public int VoteCount { get; set; }
	public double Popularity { get; set; }
	public bool Adult { get; set; }
	public string? OriginalLanguage { get; set; }
	public string GenreIds { get; set; } = string.Empty;
	public string OriginCountry { get; set; } = string.Empty;
}
