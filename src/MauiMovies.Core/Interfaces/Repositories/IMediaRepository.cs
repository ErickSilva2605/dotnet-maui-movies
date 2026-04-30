
namespace MauiMovies.Core.Interfaces.Repositories;

public interface IMediaRepository
{
	Task<IReadOnlyList<MediaItem>> GetTrendingAllAsync(TimeWindow timeWindow, CancellationToken cancellationToken = default);
	Task SaveTrendingAllAsync(IEnumerable<MediaItem> items, TimeWindow timeWindow, CancellationToken cancellationToken = default);
}
