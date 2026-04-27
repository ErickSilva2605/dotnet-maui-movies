using MauiMovies.Core.Entities;
using MauiMovies.Core.Interfaces.DataSources;
using MauiMovies.Infrastructure.Api.Http;
using MauiMovies.Infrastructure.Api.Mapping;

namespace MauiMovies.Infrastructure.Api.DataSources;

public class TmdbMediaDataSource : IMediaRemoteDataSource
{
	readonly IRequestProvider requestProvider;
	readonly TmdbOptions options;

	public TmdbMediaDataSource(IRequestProvider requestProvider, TmdbOptions options)
	{
		this.requestProvider = requestProvider;
		this.options = options;
	}

	public async Task<IReadOnlyList<MediaItem>> FetchTrendingAllAsync(CancellationToken cancellationToken = default)
	{
		var uri = $"{options.BaseUrl}trending/all/day";

		var response = await requestProvider.GetAsync<PagedResponseDto<BaseDto>>(
			uri,
			options.ApiKey,
			cancellationToken: cancellationToken);

		if (response?.Results is null)
			return [];

		return response.Results
			.Select(dto => dto.ToMediaItem())
			.OfType<MediaItem>()
			.ToList();
	}
}
