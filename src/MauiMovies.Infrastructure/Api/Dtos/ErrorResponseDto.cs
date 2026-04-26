namespace MauiMovies.Infrastructure.Api.Dtos;

public class ErrorResponseDto
{
	[JsonPropertyName("success")]
	public bool Success { get; set; }

	[JsonPropertyName("status_code")]
	public int StatusCode { get; set; }

	[JsonPropertyName("status_message")]
	public string? StatusMessage { get; set; }
}
