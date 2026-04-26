using System.Text.Json;

namespace MauiMovies.Infrastructure.Api.Converters;

public class PolymorphicListConverter<T> : JsonConverter<List<T>> where T : BaseDto
{
	public override List<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		using var jsonDoc = JsonDocument.ParseValue(ref reader);
		var root = jsonDoc.RootElement;

		var results = new List<T>();

		foreach (var element in root.EnumerateArray())
		{
			if (element.TryGetProperty("media_type", out var mediaTypeElement) &&
				Enum.TryParse<MediaType>(mediaTypeElement.GetString(), true, out var mediaType))
			{
				T? item = mediaType switch
				{
					MediaType.Movie => JsonSerializer.Deserialize<MovieDto>(element.GetRawText(), options) as T,
					MediaType.Tv => JsonSerializer.Deserialize<TvDto>(element.GetRawText(), options) as T,
					MediaType.Person => JsonSerializer.Deserialize<PersonDto>(element.GetRawText(), options) as T,
					_ => null
				};

				if (item != null)
					results.Add(item);
			}
			else
			{
				var item = JsonSerializer.Deserialize<T>(element.GetRawText(), options);

				if (item != null)
					results.Add(item);
			}
		}

		return results;
	}

	public override void Write(Utf8JsonWriter writer, List<T> value, JsonSerializerOptions options)
	{
		JsonSerializer.Serialize(writer, value, options);
	}
}
