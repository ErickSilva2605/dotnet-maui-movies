using MauiMovies.Infrastructure.Api.Dtos;
using MauiMovies.Infrastructure.Api.Mapping;

namespace MauiMovies.Infrastructure.Tests.Api.Mapping;

public class MovieDtoMapperTests
{
	[Fact]
	public void ToDomain_WithFullDto_MapsAllFields()
	{
		var dto = new MovieDto
		{
			Id = 42,
			Title = "Inception",
			OriginalTitle = "Inception",
			Overview = "Dreams",
			PosterPath = "/p.jpg",
			BackdropPath = "/b.jpg",
			ReleaseDate = "2010-07-16",
			VoteAverage = 8.4,
			VoteCount = 30000,
			Popularity = 100.5,
			Adult = false,
			OriginalLanguage = "en",
			GenreIds = [28, 878],
			Video = false,
		};

		var movie = dto.ToDomain();

		Assert.Equal(42, movie.Id);
		Assert.Equal("Inception", movie.Title);
		Assert.Equal("Dreams", movie.Overview);
		Assert.Equal(8.4, movie.VoteAverage);
		Assert.Equal(new[] { 28, 878 }, movie.GenreIds);
	}

	[Fact]
	public void ToDomain_WithNullStrings_FallbacksToEmpty()
	{
		var dto = new MovieDto
		{
			Id = 1,
			Title = null,
			OriginalTitle = null,
			Overview = null,
		};

		var movie = dto.ToDomain();

		Assert.Equal(string.Empty, movie.Title);
		Assert.Equal(string.Empty, movie.OriginalTitle);
		Assert.Equal(string.Empty, movie.Overview);
	}

	[Fact]
	public void ToDomain_WithNullAdult_DefaultsToFalse()
	{
		var dto = new MovieDto { Id = 1, Adult = null };

		var movie = dto.ToDomain();

		Assert.False(movie.Adult);
	}

	[Fact]
	public void ToDomain_WithNullGenreIds_ReturnsEmptyList()
	{
		var dto = new MovieDto { Id = 1, GenreIds = null };

		var movie = dto.ToDomain();

		Assert.Empty(movie.GenreIds);
	}
}
