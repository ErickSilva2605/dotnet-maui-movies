using System.Globalization;

namespace MauiMovies.UI.Converters;

public class YearFromDateStringConverter : IValueConverter
{
	public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
		value is string date && date.Length >= 4 ? date[..4] : null;

	public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
		throw new NotImplementedException();
}
