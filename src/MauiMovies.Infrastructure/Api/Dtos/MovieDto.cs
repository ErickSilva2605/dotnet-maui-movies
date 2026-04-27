using MauiMovies.Infrastructure.Api.Dtos.Enums;

namespace MauiMovies.Infrastructure.Api.Dtos;

public class MovieDto : BaseDto
{
	[JsonPropertyName("title")]
	public string? Title { get; set; }

	[JsonPropertyName("original_title")]
	public string? OriginalTitle { get; set; }

	[JsonPropertyName("release_date")]
	public string? ReleaseDate { get; set; }

	[JsonPropertyName("video")]
	public bool? Video { get; set; }

	[JsonPropertyName("original_language")]
	public string? OriginalLanguage { get; set; }

	[JsonPropertyName("genre_ids")]
	public List<int>? GenreIds { get; set; }

	[JsonPropertyName("genres")]
	public List<GenreDto>? Genres { get; set; }

	[JsonPropertyName("production_companies")]
	public List<ProductionCompanyDto>? ProductionCompanies { get; set; }

	[JsonPropertyName("production_countries")]
	public List<ProductionCountryDto>? ProductionCountries { get; set; }

	[JsonPropertyName("spoken_languages")]
	public List<SpokenLanguageDto>? SpokenLanguages { get; set; }

	[JsonPropertyName("runtime")]
	public int? Runtime { get; set; }

	[JsonPropertyName("status")]
	public MovieStatus? Status { get; set; }

	[JsonPropertyName("tagline")]
	public string? Tagline { get; set; }

	[JsonPropertyName("budget")]
	public long? Budget { get; set; }

	[JsonPropertyName("revenue")]
	public long? Revenue { get; set; }

	[JsonPropertyName("homepage")]
	public string? Homepage { get; set; }

	[JsonPropertyName("imdb_id")]
	public string? ImdbId { get; set; }
}
