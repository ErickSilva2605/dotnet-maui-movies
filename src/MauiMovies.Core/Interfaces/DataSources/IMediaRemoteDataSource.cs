using MauiMovies.Core.Entities;

namespace MauiMovies.Core.Interfaces.DataSources;

public interface IMediaRemoteDataSource
{
	Task<IReadOnlyList<MediaItem>> FetchTrendingAllAsync(CancellationToken cancellationToken = default);
}
