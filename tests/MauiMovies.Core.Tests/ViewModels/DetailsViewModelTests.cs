using MauiMovies.Core.Interfaces.Services;
using MauiMovies.Core.ViewModels;
using Moq;

namespace MauiMovies.Core.Tests.ViewModels;

public class DetailsViewModelTests
{
	readonly Mock<INavigationService> navigationService = new(MockBehavior.Strict);

	[Fact]
	public void MovieDetails_ApplyParameters_ParsesIdFromQuery()
	{
		var sut = new MovieDetailsViewModel(navigationService.Object);

		sut.ApplyParameters(new Dictionary<string, object> { ["id"] = "42" });

		Assert.Equal(42, sut.MovieId);
	}

	[Fact]
	public void TvDetails_ApplyParameters_ParsesIdFromQuery()
	{
		var sut = new TvDetailsViewModel(navigationService.Object);

		sut.ApplyParameters(new Dictionary<string, object> { ["id"] = 99 });

		Assert.Equal(99, sut.TvId);
	}

	[Fact]
	public void PersonDetails_ApplyParameters_ParsesIdFromQuery()
	{
		var sut = new PersonDetailsViewModel(navigationService.Object);

		sut.ApplyParameters(new Dictionary<string, object> { ["id"] = "7" });

		Assert.Equal(7, sut.PersonId);
	}

	[Fact]
	public void MovieDetails_ApplyParameters_WithoutId_LeavesIdAtDefault()
	{
		var sut = new MovieDetailsViewModel(navigationService.Object);

		sut.ApplyParameters(new Dictionary<string, object>());

		Assert.Equal(0, sut.MovieId);
	}

	[Fact]
	public async Task MovieDetails_GoBackCommand_DelegatesToNavigation()
	{
		navigationService.Setup(n => n.GoBackAsync()).Returns(Task.CompletedTask);
		var sut = new MovieDetailsViewModel(navigationService.Object);

		await sut.GoBackCommand.ExecuteAsync(null);

		navigationService.Verify(n => n.GoBackAsync(), Times.Once);
	}
}
