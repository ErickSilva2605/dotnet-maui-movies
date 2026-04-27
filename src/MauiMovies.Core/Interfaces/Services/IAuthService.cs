namespace MauiMovies.Core.Interfaces.Services;

public interface IAuthService
{
	bool IsAuthenticated { get; }
	string? Username { get; }

	event EventHandler? AuthenticationChanged;

	Task InitializeAsync(CancellationToken cancellationToken = default);
	Task<bool> SignInAsync(string username, string password, CancellationToken cancellationToken = default);
	Task SignOutAsync(CancellationToken cancellationToken = default);
}
