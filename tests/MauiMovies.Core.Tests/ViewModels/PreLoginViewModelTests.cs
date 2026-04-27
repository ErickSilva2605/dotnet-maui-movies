using MauiMovies.Core.Interfaces.Services;
using MauiMovies.Core.ViewModels;
using Moq;

namespace MauiMovies.Core.Tests.ViewModels;

public class PreLoginViewModelTests
{
	readonly Mock<INavigationService> navigationService = new(MockBehavior.Strict);

	PreLoginViewModel BuildSut() => new(navigationService.Object);

	[Fact]
	public async Task GoBackCommand_DelegatesToNavigationService()
	{
		navigationService.Setup(n => n.GoBackAsync()).Returns(Task.CompletedTask);

		await BuildSut().GoBackCommand.ExecuteAsync(null);

		navigationService.Verify(n => n.GoBackAsync(), Times.Once);
	}

	[Fact]
	public async Task NavigateToLoginCommand_DelegatesToNavigationService()
	{
		navigationService.Setup(n => n.NavigateToLoginAsync()).Returns(Task.CompletedTask);

		await BuildSut().NavigateToLoginCommand.ExecuteAsync(null);

		navigationService.Verify(n => n.NavigateToLoginAsync(), Times.Once);
	}
}
