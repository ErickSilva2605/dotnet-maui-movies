using MauiMovies.Core.Entities;
using MauiMovies.Core.Enums;

namespace MauiMovies.Core.Interfaces.DataSources;

public interface IMediaRemoteDataSource
{
	Task<IReadOnlyList<MediaItem>> FetchTrendingAllAsync(TimeWindow timeWindow, CancellationToken cancellationToken = default);
}
