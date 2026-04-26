namespace MauiMovies.Infrastructure.Api.Dtos;

public class ProductionCountryDto
{
	[JsonPropertyName("iso_3166_1")]
	public string? IsoCode { get; set; }

	[JsonPropertyName("name")]
	public string? Name { get; set; }
}
