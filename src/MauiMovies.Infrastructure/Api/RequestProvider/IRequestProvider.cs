namespace MauiMovies.Infrastructure.Api.RequestProvider;

public interface IRequestProvider
{
	Task<TResult?> GetAsync<TResult>(
		string uri,
		string token = "",
		Dictionary<string, string>? headers = null,
		CancellationToken cancellationToken = default);

	Task<TResult?> GetAsync<TResult>(
		string uri,
		string clientId,
		string clientSecret,
		CancellationToken cancellationToken = default);

	Task<TResponse?> PostAsync<TRequest, TResponse>(
		string uri,
		TRequest data,
		string token = "",
		Dictionary<string, string>? headers = null,
		CancellationToken cancellationToken = default);

	Task<bool> PostAsync<TRequest>(
		string uri,
		TRequest data,
		string token = "",
		Dictionary<string, string>? headers = null,
		CancellationToken cancellationToken = default);

	Task<TResult?> PostAsync<TResult>(
		string uri,
		string rawData,
		string clientId = "",
		string clientSecret = "",
		CancellationToken cancellationToken = default);

	Task<TResult?> PutAsync<TResult>(
		string uri,
		TResult data,
		string token = "",
		Dictionary<string, string>? headers = null,
		CancellationToken cancellationToken = default);

	Task<TResult?> PatchAsync<TResult>(
		string uri,
		TResult data,
		string token = "",
		Dictionary<string, string>? headers = null,
		CancellationToken cancellationToken = default);

	Task DeleteAsync(
		string uri,
		string token = "",
		Dictionary<string, string>? headers = null,
		CancellationToken cancellationToken = default);
}