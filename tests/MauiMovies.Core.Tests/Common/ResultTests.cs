using MauiMovies.Core.Common;

namespace MauiMovies.Core.Tests.Common;

public class ResultTests
{
	[Fact]
	public void Success_WithValue_ReturnsIsSuccessTrue()
	{
		var result = Result<int>.Success(42);

		Assert.True(result.IsSuccess);
		Assert.Equal(42, result.Value);
		Assert.Null(result.Error);
	}

	[Fact]
	public void Fail_WithError_ReturnsIsSuccessFalse()
	{
		var result = Result<int>.Fail("network error");

		Assert.False(result.IsSuccess);
		Assert.Equal(default, result.Value);
		Assert.Equal("network error", result.Error);
	}

	[Fact]
	public void Success_WithReferenceTypeValue_PreservesValue()
	{
		var list = new List<string> { "a", "b" };

		var result = Result<List<string>>.Success(list);

		Assert.True(result.IsSuccess);
		Assert.Same(list, result.Value);
	}

	[Fact]
	public void Fail_WithReferenceType_ValueIsNull()
	{
		var result = Result<List<string>>.Fail("error");

		Assert.False(result.IsSuccess);
		Assert.Null(result.Value);
	}
}
