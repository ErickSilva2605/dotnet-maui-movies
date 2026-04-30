
namespace MauiMovies.Core.Tests.ViewModels;

public class MainContainerViewModelTests
{
	readonly Mock<INavigationService> navigationService = new(MockBehavior.Strict);
	readonly Mock<IAuthService> authService = new(MockBehavior.Strict);

	MainContainerViewModel BuildSut() => new(navigationService.Object, authService.Object);

	[Fact]
	public async Task NavigateToUser_WhenAuthenticated_NavigatesToProfile()
	{
		authService.SetupGet(a => a.IsAuthenticated).Returns(true);
		navigationService.Setup(n => n.NavigateToProfileAsync()).Returns(Task.CompletedTask);

		await BuildSut().NavigateToUserCommand.ExecuteAsync(null);

		navigationService.Verify(n => n.NavigateToProfileAsync(), Times.Once);
		navigationService.Verify(n => n.NavigateToPreLoginAsync(), Times.Never);
	}

	[Fact]
	public async Task NavigateToUser_WhenNotAuthenticated_NavigatesToPreLogin()
	{
		authService.SetupGet(a => a.IsAuthenticated).Returns(false);
		navigationService.Setup(n => n.NavigateToPreLoginAsync()).Returns(Task.CompletedTask);

		await BuildSut().NavigateToUserCommand.ExecuteAsync(null);

		navigationService.Verify(n => n.NavigateToPreLoginAsync(), Times.Once);
		navigationService.Verify(n => n.NavigateToProfileAsync(), Times.Never);
	}
}
