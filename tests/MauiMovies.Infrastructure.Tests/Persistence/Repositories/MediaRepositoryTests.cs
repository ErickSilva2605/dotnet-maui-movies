using MauiMovies.Core.Entities;
using MauiMovies.Core.Enums;
using MauiMovies.Infrastructure.Persistence.Repositories;

namespace MauiMovies.Infrastructure.Tests.Persistence.Repositories;

public class MediaRepositoryTests : DatabaseTestBase
{
	MediaRepository BuildSut() => new(ContextFactory);

	[Fact]
	public async Task GetTrendingAllAsync_WhenEmpty_ReturnsEmptyList()
	{
		var sut = BuildSut();

		var result = await sut.GetTrendingAllAsync();

		Assert.Empty(result);
	}

	[Fact]
	public async Task SaveTrendingAllAsync_WithMixedTypes_PersistsAndPreservesOrder()
	{
		IReadOnlyList<MediaItem> items =
		[
			new Movie { Id = 1, Title = "M1" },
			new Tv { Id = 2, Name = "T1" },
			new Person { Id = 3, Name = "P1" },
			new Movie { Id = 4, Title = "M2" },
		];

		var sut = BuildSut();
		await sut.SaveTrendingAllAsync(items);

		var loaded = await sut.GetTrendingAllAsync();

		Assert.Equal(4, loaded.Count);
		Assert.IsType<Movie>(loaded[0]);
		Assert.IsType<Tv>(loaded[1]);
		Assert.IsType<Person>(loaded[2]);
		Assert.IsType<Movie>(loaded[3]);
		Assert.Equal(1, loaded[0].Id);
		Assert.Equal(2, loaded[1].Id);
		Assert.Equal(3, loaded[2].Id);
		Assert.Equal(4, loaded[3].Id);
	}

	[Fact]
	public async Task SaveTrendingAllAsync_PreservesAllFieldsForMovie()
	{
		var movie = new Movie
		{
			Id = 100,
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
			GenreIds = [28, 878, 12],
			Video = false,
		};

		var sut = BuildSut();
		await sut.SaveTrendingAllAsync([movie]);

		var loaded = await sut.GetTrendingAllAsync();

		var loadedMovie = Assert.IsType<Movie>(Assert.Single(loaded));
		Assert.Equal("Inception", loadedMovie.Title);
		Assert.Equal(8.4, loadedMovie.VoteAverage);
		Assert.Equal(new[] { 28, 878, 12 }, loadedMovie.GenreIds);
	}

	[Fact]
	public async Task SaveTrendingAllAsync_PreservesAllFieldsForTv()
	{
		var tv = new Tv
		{
			Id = 200,
			Name = "Breaking Bad",
			OriginalName = "Breaking Bad",
			OriginCountry = ["US"],
			GenreIds = [18, 80],
			FirstAirDate = "2008-01-20",
		};

		var sut = BuildSut();
		await sut.SaveTrendingAllAsync([tv]);

		var loaded = await sut.GetTrendingAllAsync();

		var loadedTv = Assert.IsType<Tv>(Assert.Single(loaded));
		Assert.Equal("Breaking Bad", loadedTv.Name);
		Assert.Equal(new[] { 18, 80 }, loadedTv.GenreIds);
		Assert.Equal(new[] { "US" }, loadedTv.OriginCountry);
	}

	[Fact]
	public async Task SaveTrendingAllAsync_PreservesAllFieldsForPerson()
	{
		var person = new Person
		{
			Id = 300,
			Name = "Cillian Murphy",
			OriginalName = "Cillian Murphy",
			ProfilePath = "/profile.jpg",
			KnownForDepartment = "Acting",
			Gender = Gender.Male,
		};

		var sut = BuildSut();
		await sut.SaveTrendingAllAsync([person]);

		var loaded = await sut.GetTrendingAllAsync();

		var loadedPerson = Assert.IsType<Person>(Assert.Single(loaded));
		Assert.Equal("Cillian Murphy", loadedPerson.Name);
		Assert.Equal(Gender.Male, loadedPerson.Gender);
		Assert.Equal("Acting", loadedPerson.KnownForDepartment);
	}

	[Fact]
	public async Task SaveTrendingAllAsync_CalledTwice_ReplacesPreviousList()
	{
		var sut = BuildSut();

		await sut.SaveTrendingAllAsync([new Movie { Id = 1, Title = "Old1" }, new Movie { Id = 2, Title = "Old2" }]);
		await sut.SaveTrendingAllAsync([new Movie { Id = 3, Title = "New1" }]);

		var loaded = await sut.GetTrendingAllAsync();

		Assert.Single(loaded);
		Assert.Equal(3, loaded[0].Id);
	}

	[Fact]
	public async Task SaveTrendingAllAsync_ReSavingSameMovie_UpsertsInsteadOfDuplicating()
	{
		var sut = BuildSut();

		await sut.SaveTrendingAllAsync([new Movie { Id = 1, Title = "Original" }]);
		await sut.SaveTrendingAllAsync([new Movie { Id = 1, Title = "Updated" }]);

		var loaded = await sut.GetTrendingAllAsync();

		var single = Assert.Single(loaded);
		var movie = Assert.IsType<Movie>(single);
		Assert.Equal("Updated", movie.Title);
	}

	[Fact]
	public async Task SaveTrendingAllAsync_WithEmptyList_ClearsAllEntries()
	{
		var sut = BuildSut();
		await sut.SaveTrendingAllAsync([new Movie { Id = 1, Title = "X" }]);

		await sut.SaveTrendingAllAsync([]);

		var loaded = await sut.GetTrendingAllAsync();
		Assert.Empty(loaded);
	}

	[Fact]
	public async Task SaveTrendingAllAsync_SameMovieInDifferentSaves_DoesNotDuplicateOnMoviesTable()
	{
		var sut = BuildSut();

		await sut.SaveTrendingAllAsync([new Movie { Id = 1, Title = "M" }]);
		await sut.SaveTrendingAllAsync([new Movie { Id = 1, Title = "M" }, new Movie { Id = 2, Title = "Other" }]);

		await using var context = await ContextFactory.CreateDbContextAsync();
		var movieCount = context.Movies.Count();

		Assert.Equal(2, movieCount);
	}
}
