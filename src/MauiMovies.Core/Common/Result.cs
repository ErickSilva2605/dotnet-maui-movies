namespace MauiMovies.Core.Common;

public sealed class Result<T>
{
	public bool IsSuccess { get; }
	public T? Value { get; }
	public string? Error { get; }

	Result(bool isSuccess, T? value, string? error)
	{
		IsSuccess = isSuccess;
		Value = value;
		Error = error;
	}

	public static Result<T> Success(T value) => new(true, value, null);
	public static Result<T> Fail(string error) => new(false, default, error);
}
