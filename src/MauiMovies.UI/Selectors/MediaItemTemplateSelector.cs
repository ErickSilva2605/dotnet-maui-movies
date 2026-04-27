using MauiMovies.Core.Models;

namespace MauiMovies.UI.Selectors;

public class MediaItemTemplateSelector : DataTemplateSelector
{
	public DataTemplate? MovieTemplate { get; set; }
	public DataTemplate? TvTemplate { get; set; }
	public DataTemplate? PersonTemplate { get; set; }

	protected override DataTemplate OnSelectTemplate(object item, BindableObject container) =>
		item switch
		{
			MovieModel => MovieTemplate ?? throw new InvalidOperationException($"{nameof(MovieTemplate)} not set on MediaItemTemplateSelector"),
			TvModel => TvTemplate ?? throw new InvalidOperationException($"{nameof(TvTemplate)} not set on MediaItemTemplateSelector"),
			PersonModel => PersonTemplate ?? throw new InvalidOperationException($"{nameof(PersonTemplate)} not set on MediaItemTemplateSelector"),
			_ => throw new InvalidOperationException($"Unknown media item type: {item?.GetType().FullName ?? "null"}"),
		};
}
