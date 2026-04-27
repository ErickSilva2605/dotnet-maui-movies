using System.Globalization;

namespace MauiMovies.UI.Converters;

public class TmdbImageUrlConverter : IValueConverter
{
	const string baseUrl = "https://image.tmdb.org/t/p/";
	const string defaultSize = "w342";

	public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (value is not string path || string.IsNullOrEmpty(path))
			return null;

		var size = parameter as string ?? defaultSize;
		return $"{baseUrl}{size}{path}";
	}

	public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
		throw new NotImplementedException();
}
