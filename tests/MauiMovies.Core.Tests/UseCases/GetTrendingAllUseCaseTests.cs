using MauiMovies.Core.Enums;
using MauiMovies.Core.Interfaces.DataSources;
using MauiMovies.Core.Interfaces.Repositories;
using MauiMovies.Core.UseCases;

namespace MauiMovies.Core.Tests.UseCases;

public class GetTrendingAllUseCaseTests
{
	readonly Mock<IMediaRepository> repository = new(MockBehavior.Strict);
	readonly Mock<IMediaRemoteDataSource> dataSource = new(MockBehavior.Strict);

	GetTrendingAllUseCase BuildSut() => new(repository.Object, dataSource.Object);

	[Fact]
	public async Task ExecuteAsync_WhenRemoteSucceeds_SavesToCacheAndReturnsRemoteItems()
	{
		IReadOnlyList<MediaItem> remote = [new Movie { Id = 1, Title = "M" }];
		dataSource.Setup(d => d.FetchTrendingAllAsync(TimeWindow.Day, It.IsAny<CancellationToken>()))
			.ReturnsAsync(remote);
		repository.Setup(r => r.SaveTrendingAllAsync(remote, TimeWindow.Day, It.IsAny<CancellationToken>()))
			.Returns(Task.CompletedTask);

		var result = await BuildSut().ExecuteAsync(TimeWindow.Day);

		Assert.True(result.IsSuccess);
		Assert.Same(remote, result.Value);
		repository.Verify(r => r.SaveTrendingAllAsync(remote, TimeWindow.Day, It.IsAny<CancellationToken>()), Times.Once);
	}

	[Fact]
	public async Task ExecuteAsync_WhenWeekWindow_PropagatesToDataSourceAndRepository()
	{
		IReadOnlyList<MediaItem> remote = [new Tv { Id = 9, Name = "W" }];
		dataSource.Setup(d => d.FetchTrendingAllAsync(TimeWindow.Week, It.IsAny<CancellationToken>()))
			.ReturnsAsync(remote);
		repository.Setup(r => r.SaveTrendingAllAsync(remote, TimeWindow.Week, It.IsAny<CancellationToken>()))
			.Returns(Task.CompletedTask);

		var result = await BuildSut().ExecuteAsync(TimeWindow.Week);

		Assert.True(result.IsSuccess);
		dataSource.Verify(d => d.FetchTrendingAllAsync(TimeWindow.Week, It.IsAny<CancellationToken>()), Times.Once);
		repository.Verify(r => r.SaveTrendingAllAsync(remote, TimeWindow.Week, It.IsAny<CancellationToken>()), Times.Once);
	}

	[Fact]
	public async Task ExecuteAsync_WhenRemoteThrowsAndCacheHasItems_ReturnsCachedItems()
	{
		IReadOnlyList<MediaItem> cached = [new Tv { Id = 2, Name = "T" }];
		dataSource.Setup(d => d.FetchTrendingAllAsync(TimeWindow.Day, It.IsAny<CancellationToken>()))
			.ThrowsAsync(new HttpRequestException("offline"));
		repository.Setup(r => r.GetTrendingAllAsync(TimeWindow.Day, It.IsAny<CancellationToken>()))
			.ReturnsAsync(cached);

		var result = await BuildSut().ExecuteAsync(TimeWindow.Day);

		Assert.True(result.IsSuccess);
		Assert.Same(cached, result.Value);
	}

	[Fact]
	public async Task ExecuteAsync_WhenRemoteThrowsAndCacheEmpty_ReturnsFailWithMessage()
	{
		dataSource.Setup(d => d.FetchTrendingAllAsync(TimeWindow.Day, It.IsAny<CancellationToken>()))
			.ThrowsAsync(new HttpRequestException("offline"));
		repository.Setup(r => r.GetTrendingAllAsync(TimeWindow.Day, It.IsAny<CancellationToken>()))
			.ReturnsAsync([]);

		var result = await BuildSut().ExecuteAsync(TimeWindow.Day);

		Assert.False(result.IsSuccess);
		Assert.Equal("No trending data available offline", result.Error);
	}

	[Fact]
	public async Task ExecuteAsync_WhenRemoteThrowsAndCacheThrows_ReturnsFailWithCacheError()
	{
		dataSource.Setup(d => d.FetchTrendingAllAsync(TimeWindow.Day, It.IsAny<CancellationToken>()))
			.ThrowsAsync(new HttpRequestException("offline"));
		repository.Setup(r => r.GetTrendingAllAsync(TimeWindow.Day, It.IsAny<CancellationToken>()))
			.ThrowsAsync(new InvalidOperationException("db locked"));

		var result = await BuildSut().ExecuteAsync(TimeWindow.Day);

		Assert.False(result.IsSuccess);
		Assert.Equal("db locked", result.Error);
	}

	[Fact]
	public async Task ExecuteAsync_WhenRemoteSucceeds_DoesNotQueryCache()
	{
		dataSource.Setup(d => d.FetchTrendingAllAsync(TimeWindow.Day, It.IsAny<CancellationToken>()))
			.ReturnsAsync([]);
		repository.Setup(r => r.SaveTrendingAllAsync(It.IsAny<IEnumerable<MediaItem>>(), TimeWindow.Day, It.IsAny<CancellationToken>()))
			.Returns(Task.CompletedTask);

		await BuildSut().ExecuteAsync(TimeWindow.Day);

		repository.Verify(r => r.GetTrendingAllAsync(It.IsAny<TimeWindow>(), It.IsAny<CancellationToken>()), Times.Never);
	}
}
