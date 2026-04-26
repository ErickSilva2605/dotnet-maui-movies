namespace MauiMovies.Infrastructure.Api.Dtos.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MediaType : byte
{
	Movie,
	Tv,
	Person
}
