using MauiMovies.Core.Entities;
using MauiMovies.Infrastructure.Persistence.Mapping;

namespace MauiMovies.Infrastructure.Tests.Persistence.Mapping;

public class MovieEntityMapperTests
{
	[Fact]
	public void RoundTrip_PreservesAllScalarFields()
	{
		var original = new Movie
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
			Adult = true,
			OriginalLanguage = "en",
			GenreIds = [28, 878, 12],
			Video = false,
		};

		var roundTripped = original.ToEntity().ToDomain();

		Assert.Equal(original.Id, roundTripped.Id);
		Assert.Equal(original.Title, roundTripped.Title);
		Assert.Equal(original.OriginalTitle, roundTripped.OriginalTitle);
		Assert.Equal(original.Overview, roundTripped.Overview);
		Assert.Equal(original.PosterPath, roundTripped.PosterPath);
		Assert.Equal(original.BackdropPath, roundTripped.BackdropPath);
		Assert.Equal(original.ReleaseDate, roundTripped.ReleaseDate);
		Assert.Equal(original.VoteAverage, roundTripped.VoteAverage);
		Assert.Equal(original.VoteCount, roundTripped.VoteCount);
		Assert.Equal(original.Popularity, roundTripped.Popularity);
		Assert.Equal(original.Adult, roundTripped.Adult);
		Assert.Equal(original.OriginalLanguage, roundTripped.OriginalLanguage);
		Assert.Equal(original.Video, roundTripped.Video);
	}

	[Fact]
	public void ToEntity_GenreIdsList_SerializesAsCsv()
	{
		var movie = new Movie { Id = 1, Title = "X", GenreIds = [28, 878, 12] };

		var entity = movie.ToEntity();

		Assert.Equal("28,878,12", entity.GenreIds);
	}

	[Fact]
	public void ToDomain_GenreIdsCsv_DeserializesAsList()
	{
		var entity = new MauiMovies.Infrastructure.Persistence.Entities.MovieEntity
		{
			Id = 1,
			Title = "X",
			GenreIds = "28,878,12",
		};

		var movie = entity.ToDomain();

		Assert.Equal(new[] { 28, 878, 12 }, movie.GenreIds);
	}

	[Fact]
	public void ToDomain_EmptyGenreIdsCsv_ReturnsEmptyList()
	{
		var entity = new MauiMovies.Infrastructure.Persistence.Entities.MovieEntity
		{
			Id = 1,
			Title = "X",
			GenreIds = string.Empty,
		};

		var movie = entity.ToDomain();

		Assert.Empty(movie.GenreIds);
	}

	[Fact]
	public void RoundTrip_EmptyGenreIds_PreservesEmpty()
	{
		var original = new Movie { Id = 1, Title = "X" };

		var roundTripped = original.ToEntity().ToDomain();

		Assert.Empty(roundTripped.GenreIds);
	}
}
