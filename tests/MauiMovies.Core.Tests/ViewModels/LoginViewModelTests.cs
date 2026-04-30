
namespace MauiMovies.Core.Tests.ViewModels;

public class LoginViewModelTests
{
	readonly Mock<IAuthService> authService = new(MockBehavior.Strict);
	readonly Mock<INavigationService> navigationService = new(MockBehavior.Strict);

	LoginViewModel BuildSut() => new(authService.Object, navigationService.Object);

	[Fact]
	public async Task SignInAsync_WhenSucceeds_ReplacesStackWithProfile()
	{
		authService.Setup(a => a.SignInAsync("user", "pass", It.IsAny<CancellationToken>()))
			.ReturnsAsync(true);
		navigationService.Setup(n => n.ReplaceStackWithProfileAsync()).Returns(Task.CompletedTask);

		var sut = BuildSut();
		sut.Username = "user";
		sut.Password = "pass";

		await sut.SignInCommand.ExecuteAsync(null);

		navigationService.Verify(n => n.ReplaceStackWithProfileAsync(), Times.Once);
		Assert.Null(sut.ErrorMessage);
		Assert.False(sut.IsBusy);
	}

	[Fact]
	public async Task SignInAsync_WhenFailsWithFalse_SetsErrorMessage()
	{
		authService.Setup(a => a.SignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(false);

		var sut = BuildSut();
		sut.Username = "user";
		sut.Password = "wrong";

		await sut.SignInCommand.ExecuteAsync(null);

		Assert.Equal("Invalid credentials", sut.ErrorMessage);
		Assert.False(sut.IsBusy);
	}

	[Fact]
	public async Task SignInAsync_WhenAuthThrows_SetsErrorMessageWithException()
	{
		authService.Setup(a => a.SignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
			.ThrowsAsync(new InvalidOperationException("network fail"));

		var sut = BuildSut();
		sut.Username = "user";
		sut.Password = "pass";

		await sut.SignInCommand.ExecuteAsync(null);

		Assert.Equal("network fail", sut.ErrorMessage);
		Assert.False(sut.IsBusy);
	}

	[Fact]
	public async Task GoBackCommand_DelegatesToNavigationService()
	{
		navigationService.Setup(n => n.GoBackAsync()).Returns(Task.CompletedTask);

		await BuildSut().GoBackCommand.ExecuteAsync(null);

		navigationService.Verify(n => n.GoBackAsync(), Times.Once);
	}

	[Fact]
	public async Task SignInAsync_WhenAlreadyBusy_DoesNotInvokeAuth()
	{
		var sut = BuildSut();
		sut.IsBusy = true;

		await sut.SignInCommand.ExecuteAsync(null);

		authService.Verify(a => a.SignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
	}
}
