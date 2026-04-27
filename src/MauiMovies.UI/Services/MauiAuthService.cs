using MauiMovies.Core.Interfaces.Services;

namespace MauiMovies.UI.Services;

public class MauiAuthService : IAuthService
{
	const string usernameKey = "auth.username";
	const string sessionIdKey = "auth.session_id";

	string? username;
	string? sessionId;

	public bool IsAuthenticated => !string.IsNullOrEmpty(sessionId);
	public string? Username => username;

	public event EventHandler? AuthenticationChanged;

	public async Task InitializeAsync(CancellationToken cancellationToken = default)
	{
		username = await SecureStorage.Default.GetAsync(usernameKey);
		sessionId = await SecureStorage.Default.GetAsync(sessionIdKey);
	}

	public async Task<bool> SignInAsync(string username, string password, CancellationToken cancellationToken = default)
	{
		if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
			return false;

		// Stub auth — TODO: substituir pelo fluxo real TMDB (request_token → validate_with_login → create_session)
		var fakeSessionId = $"stub-{Guid.NewGuid():N}";

		await SecureStorage.Default.SetAsync(usernameKey, username);
		await SecureStorage.Default.SetAsync(sessionIdKey, fakeSessionId);

		this.username = username;
		this.sessionId = fakeSessionId;

		AuthenticationChanged?.Invoke(this, EventArgs.Empty);
		return true;
	}

	public Task SignOutAsync(CancellationToken cancellationToken = default)
	{
		SecureStorage.Default.Remove(usernameKey);
		SecureStorage.Default.Remove(sessionIdKey);

		username = null;
		sessionId = null;

		AuthenticationChanged?.Invoke(this, EventArgs.Empty);
		return Task.CompletedTask;
	}
}
