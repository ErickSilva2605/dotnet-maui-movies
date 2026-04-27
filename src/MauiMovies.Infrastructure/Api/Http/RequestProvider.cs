using System.Net.Http.Headers;
using System.Text.Json;
using MauiMovies.Infrastructure.Api.Converters;

namespace MauiMovies.Infrastructure.Api.Http;

public class RequestProvider : IRequestProvider
{
	#region Fields

	readonly Lazy<HttpClient> httpClient;
	readonly JsonSerializerOptions jsonSerializerContext;

	#endregion Fields

	#region Constructors

	public RequestProvider(HttpMessageHandler? messageHandler = null)
	{
		httpClient = new(() =>
		{
			var httpClient = messageHandler is not null ? new HttpClient(messageHandler) : new HttpClient();
			httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			return httpClient;
		}, LazyThreadSafetyMode.ExecutionAndPublication);

		jsonSerializerContext = new()
		{
			PropertyNameCaseInsensitive = false,
			IgnoreReadOnlyProperties = true,
			NumberHandling = JsonNumberHandling.AllowReadingFromString,
		};

		jsonSerializerContext.Converters.Add(new PolymorphicListConverter<BaseDto>());
	}

	#endregion Constructors

	#region Methods

	public async Task<TResult?> GetAsync<TResult>(
		string uri, string token = "", Dictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
	{
		using var request = new HttpRequestMessage(HttpMethod.Get, uri);

		if (!string.IsNullOrEmpty(token))
			request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

		if (headers is not null)
			foreach (var (key, value) in headers)
				request.Headers.Add(key, value);

		using var response = await httpClient.Value.SendAsync(request, cancellationToken);
		response.EnsureSuccessStatusCode();

		await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
		return await JsonSerializer.DeserializeAsync<TResult>(stream, jsonSerializerContext, cancellationToken);
	}

	public Task<TResult?> GetAsync<TResult>(
		string uri, string clientId, string clientSecret, CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}

	public Task<TResponse?> PostAsync<TRequest, TResponse>(
		string uri, TRequest data, string token = "", Dictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}

	public Task<bool> PostAsync<TRequest>(
		string uri, TRequest data, string token = "", Dictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}

	public Task<TResult?> PostAsync<TResult>(
		string uri, string rawData, string clientId = "", string clientSecret = "", CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}

	public Task<TResult?> PutAsync<TResult>(
		string uri, TResult data, string token = "", Dictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}

	public Task<TResult?> PatchAsync<TResult>(
		string uri, TResult data, string token = "", Dictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}

	public Task DeleteAsync(
		string uri, string token = "", Dictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}

	#endregion Methods
}
