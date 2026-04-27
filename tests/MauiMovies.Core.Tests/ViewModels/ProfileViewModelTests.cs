using MauiMovies.Core.Interfaces.Services;
using MauiMovies.Core.ViewModels;
using Moq;

namespace MauiMovies.Core.Tests.ViewModels;

public class ProfileViewModelTests
{
	readonly Mock<IAuthService> authService = new(MockBehavior.Strict);
	readonly Mock<INavigationService> navigationService = new(MockBehavior.Strict);

	ProfileViewModel BuildSut()
	{
		authService.SetupGet(a => a.Username).Returns("erick");
		return new ProfileViewModel(authService.Object, navigationService.Object);
	}

	[Fact]
	public void Constructor_HydratesUsernameFromAuthService()
	{
		var sut = BuildSut();

		Assert.Equal("erick", sut.Username);
	}

	[Fact]
	public async Task GoBackCommand_DelegatesToNavigationService()
	{
		navigationService.Setup(n => n.GoBackAsync()).Returns(Task.CompletedTask);

		await BuildSut().GoBackCommand.ExecuteAsync(null);

		navigationService.Verify(n => n.GoBackAsync(), Times.Once);
	}

	[Fact]
	public async Task SignOutCommand_SignsOutAndNavigatesToRoot()
	{
		authService.Setup(a => a.SignOutAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
		navigationService.Setup(n => n.GoToRootAsync()).Returns(Task.CompletedTask);

		await BuildSut().SignOutCommand.ExecuteAsync(null);

		authService.Verify(a => a.SignOutAsync(It.IsAny<CancellationToken>()), Times.Once);
		navigationService.Verify(n => n.GoToRootAsync(), Times.Once);
	}
}
