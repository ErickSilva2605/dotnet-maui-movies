using System.Text.Json.Serialization;

namespace MauiMovies.Infrastructure.Api.Dtos;

public abstract class BaseDto
{
	[JsonPropertyName("id")]
	public int Id { get; set; }

	[JsonPropertyName("poster_path")]
	public string? PosterPath { get; set; }

	[JsonPropertyName("backdrop_path")]
	public string? BackdropPath { get; set; }

	[JsonPropertyName("overview")]
	public string? Overview { get; set; }

	[JsonPropertyName("popularity")]
	public double Popularity { get; set; }

	[JsonPropertyName("vote_average")]
	public double VoteAverage { get; set; }

	[JsonPropertyName("vote_count")]
	public int VoteCount { get; set; }

	[JsonPropertyName("media_type")]
	public string? MediaType { get; set; }
}