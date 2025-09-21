using System.Text.Json.Serialization;

namespace MauiMovies.Infrastructure.Api.Dtos.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MovieStatus : byte
{
	Rumored,
	Planned,
	InProduction,
	PostProduction,
	Released,
	Canceled
}
