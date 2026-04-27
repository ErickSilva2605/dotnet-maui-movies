using MauiMovies.Core.Common;
using MauiMovies.Core.Entities;
using MauiMovies.Core.Interfaces.DataSources;
using MauiMovies.Core.Interfaces.Repositories;

namespace MauiMovies.Core.UseCases;

public class GetTrendingAllUseCase
{
	readonly IMediaRepository repository;
	readonly IMediaRemoteDataSource dataSource;

	public GetTrendingAllUseCase(IMediaRepository repository, IMediaRemoteDataSource dataSource)
	{
		this.repository = repository;
		this.dataSource = dataSource;
	}

	public async Task<Result<IReadOnlyList<MediaItem>>> ExecuteAsync(CancellationToken cancellationToken = default)
	{
		try
		{
			var items = await dataSource.FetchTrendingAllAsync(cancellationToken);
			await repository.SaveTrendingAllAsync(items, cancellationToken);
			return Result<IReadOnlyList<MediaItem>>.Success(items);
		}
		catch
		{
			try
			{
				var cached = await repository.GetTrendingAllAsync(cancellationToken);

				if (cached.Count > 0)
					return Result<IReadOnlyList<MediaItem>>.Success(cached);

				return Result<IReadOnlyList<MediaItem>>.Fail("No trending data available offline");
			}
			catch (Exception ex)
			{
				return Result<IReadOnlyList<MediaItem>>.Fail(ex.Message);
			}
		}
	}
}
