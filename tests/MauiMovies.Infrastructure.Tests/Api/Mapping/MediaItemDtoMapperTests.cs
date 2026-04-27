using MauiMovies.Core.Entities;
using MauiMovies.Infrastructure.Api.Dtos;
using MauiMovies.Infrastructure.Api.Mapping;

namespace MauiMovies.Infrastructure.Tests.Api.Mapping;

public class MediaItemDtoMapperTests
{
	[Fact]
	public void ToMediaItem_WhenMovieDto_ReturnsMovie()
	{
		BaseDto dto = new MovieDto { Id = 1, Title = "M" };

		var item = dto.ToMediaItem();

		Assert.IsType<Movie>(item);
		Assert.Equal(1, item.Id);
	}

	[Fact]
	public void ToMediaItem_WhenTvDto_ReturnsTv()
	{
		BaseDto dto = new TvDto { Id = 2, Name = "T" };

		var item = dto.ToMediaItem();

		Assert.IsType<Tv>(item);
		Assert.Equal(2, item.Id);
	}

	[Fact]
	public void ToMediaItem_WhenPersonDto_ReturnsPerson()
	{
		BaseDto dto = new PersonDto { Id = 3, Name = "P" };

		var item = dto.ToMediaItem();

		Assert.IsType<Person>(item);
		Assert.Equal(3, item.Id);
	}
}
