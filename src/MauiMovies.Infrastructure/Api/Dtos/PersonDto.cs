using System.Text.Json.Serialization;
using MauiMovies.Infrastructure.Api.Converters;
using MauiMovies.Infrastructure.Api.Dtos.Enums;

namespace MauiMovies.Infrastructure.Api.Dtos;

public class PersonDto : BaseDto
{
	[JsonPropertyName("name")]
	public string? Name { get; set; }

	[JsonPropertyName("also_known_as")]
	public List<string>? AlsoKnownAs { get; set; }

	[JsonPropertyName("known_for_department")]
	public string? KnownForDepartment { get; set; }

	[JsonPropertyName("biography")]
	public string? Biography { get; set; }

	[JsonPropertyName("birthday")]
	public string? Birthday { get; set; }

	[JsonPropertyName("deathday")]
	public string? Deathday { get; set; }

	[JsonPropertyName("gender")]
	public Gender Gender { get; set; }

	[JsonPropertyName("place_of_birth")]
	public string? PlaceOfBirth { get; set; }

	[JsonPropertyName("profile_path")]
	public string? ProfilePath { get; set; }

	[JsonPropertyName("homepage")]
	public string? Homepage { get; set; }

	[JsonPropertyName("known_for")]
	[JsonConverter(typeof(PolymorphicListConverter<BaseDto>))]
	public List<BaseDto>? KnownFor { get; set; }
}