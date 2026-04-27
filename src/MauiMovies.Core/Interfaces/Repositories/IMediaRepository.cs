using MauiMovies.Core.Entities;

namespace MauiMovies.Core.Interfaces.Repositories;

public interface IMediaRepository
{
	Task<IReadOnlyList<MediaItem>> GetTrendingAllAsync(CancellationToken cancellationToken = default);
	Task SaveTrendingAllAsync(IEnumerable<MediaItem> items, CancellationToken cancellationToken = default);
}
