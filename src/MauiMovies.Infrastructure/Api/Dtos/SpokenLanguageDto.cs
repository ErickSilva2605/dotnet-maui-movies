using System.Text.Json.Serialization;

namespace MauiMovies.Infrastructure.Api.Dtos;

public class SpokenLanguageDto
{
	[JsonPropertyName("english_name")]
	public string? EnglishName { get; set; }

	[JsonPropertyName("iso_639_1")]
	public string? IsoCode { get; set; }

	[JsonPropertyName("name")]
	public string? Name { get; set; }
}