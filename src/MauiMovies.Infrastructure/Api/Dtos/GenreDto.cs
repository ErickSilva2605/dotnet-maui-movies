using System.Text.Json.Serialization;

namespace MauiMovies.Infrastructure.Api.Dtos;

public class GenreDto
{
	[JsonPropertyName("id")]
	public int Id { get; set; }

	[JsonPropertyName("name")]
	public string? Name { get; set; }
}