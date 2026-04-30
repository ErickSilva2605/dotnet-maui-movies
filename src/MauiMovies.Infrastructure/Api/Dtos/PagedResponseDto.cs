using MauiMovies.Infrastructure.Api.Converters;

namespace MauiMovies.Infrastructure.Api.Dtos;

public class PagedResponseDto<T>
{
	[JsonPropertyName("page")]
	public int Page { get; set; }

	[JsonPropertyName("results")]
	[JsonConverter(typeof(PolymorphicListConverter<BaseDto>))]
	public List<T> Results { get; set; } = [];

	[JsonPropertyName("total_pages")]
	public int TotalPages { get; set; }

	[JsonPropertyName("total_results")]
	public int TotalResults { get; set; }
}
