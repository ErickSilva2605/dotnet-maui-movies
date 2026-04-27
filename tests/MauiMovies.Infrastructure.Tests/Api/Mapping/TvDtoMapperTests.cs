using MauiMovies.Infrastructure.Api.Dtos;
using MauiMovies.Infrastructure.Api.Mapping;

namespace MauiMovies.Infrastructure.Tests.Api.Mapping;

public class TvDtoMapperTests
{
	[Fact]
	public void ToDomain_WithFullDto_MapsAllFields()
	{
		var dto = new TvDto
		{
			Id = 99,
			Name = "Breaking Bad",
			OriginalName = "Breaking Bad",
			Overview = "Cooking",
			PosterPath = "/p.jpg",
			BackdropPath = "/b.jpg",
			FirstAirDate = "2008-01-20",
			VoteAverage = 8.9,
			VoteCount = 10000,
			Popularity = 200.0,
			Adult = false,
			OriginalLanguage = "en",
			GenreIds = [18, 80],
			OriginCountry = ["US"],
		};

		var tv = dto.ToDomain();

		Assert.Equal(99, tv.Id);
		Assert.Equal("Breaking Bad", tv.Name);
		Assert.Equal("2008-01-20", tv.FirstAirDate);
		Assert.Equal(new[] { 18, 80 }, tv.GenreIds);
		Assert.Equal(new[] { "US" }, tv.OriginCountry);
	}

	[Fact]
	public void ToDomain_WithNullCollections_ReturnsEmptyLists()
	{
		var dto = new TvDto { Id = 1, GenreIds = null, OriginCountry = null };

		var tv = dto.ToDomain();

		Assert.Empty(tv.GenreIds);
		Assert.Empty(tv.OriginCountry);
	}
}
