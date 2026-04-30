
namespace MauiMovies.Core.Models;

public abstract partial class MediaItemModel : ObservableObject
{
	[ObservableProperty] int id;
	[ObservableProperty] double popularity;
	[ObservableProperty] bool adult;

	public abstract MediaType MediaType { get; }
}
