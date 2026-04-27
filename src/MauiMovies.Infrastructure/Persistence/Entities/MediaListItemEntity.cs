using MauiMovies.Core.Enums;

namespace MauiMovies.Infrastructure.Persistence.Entities;

public class MediaListItemEntity
{
	public int Id { get; set; }
	public MediaListType ListType { get; set; }
	public MediaType MediaType { get; set; }
	public int MediaId { get; set; }
	public int Position { get; set; }
}
