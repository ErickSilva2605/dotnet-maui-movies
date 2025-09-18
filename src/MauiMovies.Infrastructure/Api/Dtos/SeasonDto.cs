using System.Text.Json.Serialization;

namespace MauiMovies.Infrastructure.Api.Dtos;

public class SeasonDto
{
	[JsonPropertyName("id")]
	public int Id { get; set; }

	[JsonPropertyName("name")]
	public string? Name { get; set; }

	[JsonPropertyName("overview")]
	public string? Overview { get; set; }

	[JsonPropertyName("air_date")]
	public string? AirDate { get; set; }

	[JsonPropertyName("episode_count")]
	public int? EpisodeCount { get; set; }

	[JsonPropertyName("poster_path")]
	public string? PosterPath { get; set; }

	[JsonPropertyName("season_number")]
	public int? SeasonNumber { get; set; }
}