using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using MauiMovies.Infrastructure.Api.Converters;
using MauiMovies.Infrastructure.Api.Dtos;

namespace MauiMovies.Infrastructure.Api.Http;

public class RequestProvider : IRequestProvider
{
	#region Fields

	readonly Lazy<HttpClient> httpClient;
	readonly JsonSerializerOptions jsonSerializerContext;

	#endregion Fields

	#region Constructors

	public RequestProvider(HttpMessageHandler messageHandler)
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

	public Task<TResult?> GetAsync<TResult>(
		string uri, string token = "", Dictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
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
