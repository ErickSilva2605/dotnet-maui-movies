using MauiMovies.Core.Models;

namespace MauiMovies.Core.Tests.Mapping;

public class MediaItemModelMapperTests
{
	[Fact]
	public void ToModel_WhenMovie_ReturnsMovieModel()
	{
		MediaItem movie = new Movie { Id = 1, Title = "M" };

		var model = movie.ToModel();

		Assert.IsType<MovieModel>(model);
		Assert.Equal(1, model.Id);
	}

	[Fact]
	public void ToModel_WhenTv_ReturnsTvModel()
	{
		MediaItem tv = new Tv { Id = 2, Name = "T" };

		var model = tv.ToModel();

		Assert.IsType<TvModel>(model);
		Assert.Equal(2, model.Id);
	}

	[Fact]
	public void ToModel_WhenPerson_ReturnsPersonModel()
	{
		MediaItem person = new Person { Id = 3, Name = "P" };

		var model = person.ToModel();

		Assert.IsType<PersonModel>(model);
		Assert.Equal(3, model.Id);
	}
}
