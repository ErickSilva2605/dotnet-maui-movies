using System.Text.Json.Serialization;
using MauiMovies.Infrastructure.Api.Dtos.Enums;

namespace MauiMovies.Infrastructure.Api.Dtos;

public class TvDto : BaseDto
{
	[JsonPropertyName("name")]
	public string? Name { get; set; }

	[JsonPropertyName("original_name")]
	public string? OriginalName { get; set; }

	[JsonPropertyName("first_air_date")]
	public string? FirstAirDate { get; set; }

	[JsonPropertyName("last_air_date")]
	public string? LastAirDate { get; set; }

	[JsonPropertyName("in_production")]
	public bool? InProduction { get; set; }

	[JsonPropertyName("number_of_episodes")]
	public int? NumberOfEpisodes { get; set; }

	[JsonPropertyName("number_of_seasons")]
	public int? NumberOfSeasons { get; set; }

	[JsonPropertyName("episode_run_time")]
	public List<int>? EpisodeRunTime { get; set; }

	[JsonPropertyName("original_language")]
	public string? OriginalLanguage { get; set; }

	[JsonPropertyName("origin_country")]
	public List<string>? OriginCountry { get; set; }

	[JsonPropertyName("genres")]
	public List<GenreDto>? Genres { get; set; }

	[JsonPropertyName("production_companies")]
	public List<ProductionCompanyDto>? ProductionCompanies { get; set; }

	[JsonPropertyName("production_countries")]
	public List<ProductionCountryDto>? ProductionCountries { get; set; }

	[JsonPropertyName("spoken_languages")]
	public List<SpokenLanguageDto>? SpokenLanguages { get; set; }

	[JsonPropertyName("networks")]
	public List<NetworkDto>? Networks { get; set; }

	[JsonPropertyName("seasons")]
	public List<SeasonDto>? Seasons { get; set; }

	[JsonPropertyName("status")]
	public TvStatus? Status { get; set; }

	[JsonPropertyName("tagline")]
	public string? Tagline { get; set; }

	[JsonPropertyName("type")]
	public string? Type { get; set; }

	[JsonPropertyName("homepage")]
	public string? Homepage { get; set; }
}