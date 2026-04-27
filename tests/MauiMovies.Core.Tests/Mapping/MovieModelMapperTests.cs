using MauiMovies.Core.Entities;
using MauiMovies.Core.Mapping;

namespace MauiMovies.Core.Tests.Mapping;

public class MovieModelMapperTests
{
	[Fact]
	public void ToModel_WithFullMovie_MapsAllFields()
	{
		var movie = new Movie
		{
			Id = 42,
			Title = "Inception",
			OriginalTitle = "Inception",
			Overview = "Dreams within dreams",
			PosterPath = "/poster.jpg",
			BackdropPath = "/backdrop.jpg",
			ReleaseDate = "2010-07-16",
			VoteAverage = 8.4,
			VoteCount = 30000,
			Popularity = 100.5,
			Adult = false,
			OriginalLanguage = "en",
			GenreIds = [28, 878],
			Video = false,
		};

		var model = movie.ToModel();

		Assert.Equal(42, model.Id);
		Assert.Equal("Inception", model.Title);
		Assert.Equal("Inception", model.OriginalTitle);
		Assert.Equal("Dreams within dreams", model.Overview);
		Assert.Equal("/poster.jpg", model.PosterPath);
		Assert.Equal("/backdrop.jpg", model.BackdropPath);
		Assert.Equal("2010-07-16", model.ReleaseDate);
		Assert.Equal(8.4, model.VoteAverage);
		Assert.Equal(30000, model.VoteCount);
		Assert.Equal(100.5, model.Popularity);
		Assert.False(model.Adult);
		Assert.Equal("en", model.OriginalLanguage);
		Assert.Equal(new[] { 28, 878 }, model.GenreIds);
		Assert.False(model.Video);
	}

	[Fact]
	public void ToModel_WithMinimalMovie_PreservesNullables()
	{
		var movie = new Movie
		{
			Id = 1,
			Title = "X",
		};

		var model = movie.ToModel();

		Assert.Null(model.PosterPath);
		Assert.Null(model.BackdropPath);
		Assert.Null(model.ReleaseDate);
		Assert.Null(model.OriginalLanguage);
		Assert.Empty(model.GenreIds);
	}
}
