using MauiMovies.Infrastructure.Persistence.Mapping;

namespace MauiMovies.Infrastructure.Tests.Persistence.Mapping;

public class TvEntityMapperTests
{
	[Fact]
	public void RoundTrip_PreservesAllFieldsIncludingCsvLists()
	{
		var original = new Tv
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
			OriginCountry = ["US", "CA"],
		};

		var roundTripped = original.ToEntity().ToDomain();

		Assert.Equal(original.Id, roundTripped.Id);
		Assert.Equal(original.Name, roundTripped.Name);
		Assert.Equal(original.FirstAirDate, roundTripped.FirstAirDate);
		Assert.Equal(new[] { 18, 80 }, roundTripped.GenreIds);
		Assert.Equal(new[] { "US", "CA" }, roundTripped.OriginCountry);
	}

	[Fact]
	public void ToEntity_OriginCountryList_SerializesAsCsv()
	{
		var tv = new Tv { Id = 1, Name = "X", OriginCountry = ["US", "CA", "UK"] };

		var entity = tv.ToEntity();

		Assert.Equal("US,CA,UK", entity.OriginCountry);
	}

	[Fact]
	public void RoundTrip_EmptyOriginCountry_PreservesEmpty()
	{
		var original = new Tv { Id = 1, Name = "X" };

		var roundTripped = original.ToEntity().ToDomain();

		Assert.Empty(roundTripped.OriginCountry);
	}
}
