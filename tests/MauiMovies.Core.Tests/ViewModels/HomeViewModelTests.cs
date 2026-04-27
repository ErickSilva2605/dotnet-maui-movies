using MauiMovies.Core.Entities;
using MauiMovies.Core.Interfaces.DataSources;
using MauiMovies.Core.Interfaces.Repositories;
using MauiMovies.Core.Interfaces.Services;
using MauiMovies.Core.Models;
using MauiMovies.Core.UseCases;
using MauiMovies.Core.ViewModels;
using Moq;

namespace MauiMovies.Core.Tests.ViewModels;

public class HomeViewModelTests
{
	readonly Mock<IMediaRepository> repository = new(MockBehavior.Loose);
	readonly Mock<IMediaRemoteDataSource> dataSource = new(MockBehavior.Loose);
	readonly Mock<INavigationService> navigationService = new(MockBehavior.Loose);

	HomeViewModel BuildSut() =>
		new(new GetTrendingAllUseCase(repository.Object, dataSource.Object), navigationService.Object);

	[Fact]
	public async Task OnAppearingAsync_WhenTrendingEmpty_LoadsTrendingFromUseCase()
	{
		dataSource.Setup(d => d.FetchTrendingAllAsync(It.IsAny<CancellationToken>()))
			.ReturnsAsync([new Movie { Id = 1, Title = "M" }]);

		var sut = BuildSut();
		await sut.OnAppearingAsync();

		Assert.Single(sut.Trending);
		Assert.IsType<MovieModel>(sut.Trending[0]);
	}

	[Fact]
	public async Task OnAppearingAsync_WhenTrendingAlreadyLoaded_DoesNotReload()
	{
		dataSource.Setup(d => d.FetchTrendingAllAsync(It.IsAny<CancellationToken>()))
			.ReturnsAsync([new Movie { Id = 1, Title = "M" }]);

		var sut = BuildSut();
		await sut.OnAppearingAsync();
		await sut.OnAppearingAsync();

		dataSource.Verify(d => d.FetchTrendingAllAsync(It.IsAny<CancellationToken>()), Times.Once);
	}

	[Fact]
	public async Task LoadTrendingAsync_WhenSucceeds_PopulatesTrendingWithMixedTypes()
	{
		IReadOnlyList<MediaItem> items =
		[
			new Movie { Id = 1, Title = "M" },
			new Tv { Id = 2, Name = "T" },
			new Person { Id = 3, Name = "P" },
		];
		dataSource.Setup(d => d.FetchTrendingAllAsync(It.IsAny<CancellationToken>()))
			.ReturnsAsync(items);

		var sut = BuildSut();
		await sut.LoadTrendingCommand.ExecuteAsync(null);

		Assert.Equal(3, sut.Trending.Count);
		Assert.IsType<MovieModel>(sut.Trending[0]);
		Assert.IsType<TvModel>(sut.Trending[1]);
		Assert.IsType<PersonModel>(sut.Trending[2]);
	}

	[Fact]
	public async Task LoadTrendingAsync_WhenFails_SetsTrendingError()
	{
		dataSource.Setup(d => d.FetchTrendingAllAsync(It.IsAny<CancellationToken>()))
			.ThrowsAsync(new HttpRequestException("offline"));
		repository.Setup(r => r.GetTrendingAllAsync(It.IsAny<CancellationToken>()))
			.ReturnsAsync([]);

		var sut = BuildSut();
		await sut.LoadTrendingCommand.ExecuteAsync(null);

		Assert.NotNull(sut.TrendingError);
		Assert.Empty(sut.Trending);
	}

	[Fact]
	public async Task LoadTrendingAsync_WhenSucceeds_ClearsPreviousError()
	{
		dataSource.SetupSequence(d => d.FetchTrendingAllAsync(It.IsAny<CancellationToken>()))
			.ThrowsAsync(new HttpRequestException("offline"))
			.ReturnsAsync([new Movie { Id = 1, Title = "M" }]);
		repository.Setup(r => r.GetTrendingAllAsync(It.IsAny<CancellationToken>()))
			.ReturnsAsync([]);

		var sut = BuildSut();
		await sut.LoadTrendingCommand.ExecuteAsync(null);
		Assert.NotNull(sut.TrendingError);

		await sut.LoadTrendingCommand.ExecuteAsync(null);

		Assert.Null(sut.TrendingError);
	}

	[Fact]
	public async Task LoadTrendingAsync_WhenItemsHaveBackdrop_SetsWelcomeBackdropPath()
	{
		dataSource.Setup(d => d.FetchTrendingAllAsync(It.IsAny<CancellationToken>()))
			.ReturnsAsync([new Movie { Id = 1, Title = "M", BackdropPath = "/bd.jpg" }]);

		var sut = BuildSut();
		await sut.LoadTrendingCommand.ExecuteAsync(null);

		Assert.Equal("/bd.jpg", sut.WelcomeBackdropPath);
	}

	[Fact]
	public async Task LoadTrendingAsync_WhenOnlyPersonsReturned_DoesNotSetWelcomeBackdropPath()
	{
		dataSource.Setup(d => d.FetchTrendingAllAsync(It.IsAny<CancellationToken>()))
			.ReturnsAsync([new Person { Id = 1, Name = "P" }]);

		var sut = BuildSut();
		await sut.LoadTrendingCommand.ExecuteAsync(null);

		Assert.Null(sut.WelcomeBackdropPath);
	}

	[Fact]
	public async Task NavigateToMediaItem_WhenMovieModel_NavigatesToMovieDetails()
	{
		var sut = BuildSut();
		var model = new MovieModel { Id = 42 };

		await sut.NavigateToMediaItemCommand.ExecuteAsync(model);

		navigationService.Verify(n => n.NavigateToMovieDetailsAsync(42), Times.Once);
	}

	[Fact]
	public async Task NavigateToMediaItem_WhenTvModel_NavigatesToTvDetails()
	{
		var sut = BuildSut();
		var model = new TvModel { Id = 99 };

		await sut.NavigateToMediaItemCommand.ExecuteAsync(model);

		navigationService.Verify(n => n.NavigateToTvDetailsAsync(99), Times.Once);
	}

	[Fact]
	public async Task NavigateToMediaItem_WhenPersonModel_NavigatesToPersonDetails()
	{
		var sut = BuildSut();
		var model = new PersonModel { Id = 7 };

		await sut.NavigateToMediaItemCommand.ExecuteAsync(model);

		navigationService.Verify(n => n.NavigateToPersonDetailsAsync(7), Times.Once);
	}

	[Fact]
	public async Task NavigateToMediaItem_WhenNull_DoesNothing()
	{
		var sut = BuildSut();

		await sut.NavigateToMediaItemCommand.ExecuteAsync(null);

		navigationService.VerifyNoOtherCalls();
	}
}
