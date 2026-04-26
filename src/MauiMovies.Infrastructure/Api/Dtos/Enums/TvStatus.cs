namespace MauiMovies.Infrastructure.Api.Dtos.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TvStatus : byte
{
	ReturningSeries,
	Planned,
	InProduction,
	Ended,
	Canceled,
	Pilot
}
