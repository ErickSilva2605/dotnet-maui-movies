
namespace MauiMovies.Core.Tests.Mapping;

public class TvModelMapperTests
{
	[Fact]
	public void ToModel_WithFullTv_MapsAllFields()
	{
		var tv = new Tv
		{
			Id = 99,
			Name = "Breaking Bad",
			OriginalName = "Breaking Bad",
			Overview = "A teacher turned cook",
			PosterPath = "/poster.jpg",
			BackdropPath = "/backdrop.jpg",
			FirstAirDate = "2008-01-20",
			VoteAverage = 8.9,
			VoteCount = 10000,
			Popularity = 200.0,
			Adult = false,
			OriginalLanguage = "en",
			GenreIds = [18, 80],
			OriginCountry = ["US"],
		};

		var model = tv.ToModel();

		Assert.Equal(99, model.Id);
		Assert.Equal("Breaking Bad", model.Name);
		Assert.Equal("Breaking Bad", model.OriginalName);
		Assert.Equal("A teacher turned cook", model.Overview);
		Assert.Equal("/poster.jpg", model.PosterPath);
		Assert.Equal("/backdrop.jpg", model.BackdropPath);
		Assert.Equal("2008-01-20", model.FirstAirDate);
		Assert.Equal(8.9, model.VoteAverage);
		Assert.Equal(10000, model.VoteCount);
		Assert.Equal(200.0, model.Popularity);
		Assert.Equal("en", model.OriginalLanguage);
		Assert.Equal(new[] { 18, 80 }, model.GenreIds);
		Assert.Equal(new[] { "US" }, model.OriginCountry);
	}

	[Fact]
	public void ToModel_WithMinimalTv_PreservesEmptyCollections()
	{
		var tv = new Tv { Id = 1, Name = "X" };

		var model = tv.ToModel();

		Assert.Empty(model.GenreIds);
		Assert.Empty(model.OriginCountry);
	}
}
