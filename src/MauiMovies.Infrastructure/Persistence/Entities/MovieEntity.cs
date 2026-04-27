namespace MauiMovies.Infrastructure.Persistence.Entities;

public class MovieEntity
{
	public int Id { get; set; }
	public string Title { get; set; } = string.Empty;
	public string OriginalTitle { get; set; } = string.Empty;
	public string Overview { get; set; } = string.Empty;
	public string? PosterPath { get; set; }
	public string? BackdropPath { get; set; }
	public string? ReleaseDate { get; set; }
	public double VoteAverage { get; set; }
	public int VoteCount { get; set; }
	public double Popularity { get; set; }
	public bool Adult { get; set; }
	public string? OriginalLanguage { get; set; }
	public string GenreIds { get; set; } = string.Empty;
	public bool Video { get; set; }
}
