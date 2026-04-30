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

		var result = await sut.GetTrendingAllAsync(TimeWindow.Day);

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
		await sut.SaveTrendingAllAsync(items, TimeWindow.Day);

		var loaded = await sut.GetTrendingAllAsync(TimeWindow.Day);

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
		await sut.SaveTrendingAllAsync([movie], TimeWindow.Day);

		var loaded = await sut.GetTrendingAllAsync(TimeWindow.Day);

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
		await sut.SaveTrendingAllAsync([tv], TimeWindow.Day);

		var loaded = await sut.GetTrendingAllAsync(TimeWindow.Day);

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
		await sut.SaveTrendingAllAsync([person], TimeWindow.Day);

		var loaded = await sut.GetTrendingAllAsync(TimeWindow.Day);

		var loadedPerson = Assert.IsType<Person>(Assert.Single(loaded));
		Assert.Equal("Cillian Murphy", loadedPerson.Name);
		Assert.Equal(Gender.Male, loadedPerson.Gender);
		Assert.Equal("Acting", loadedPerson.KnownForDepartment);
	}

	[Fact]
	public async Task SaveTrendingAllAsync_CalledTwice_ReplacesPreviousList()
	{
		var sut = BuildSut();

		await sut.SaveTrendingAllAsync([new Movie { Id = 1, Title = "Old1" }, new Movie { Id = 2, Title = "Old2" }], TimeWindow.Day);
		await sut.SaveTrendingAllAsync([new Movie { Id = 3, Title = "New1" }], TimeWindow.Day);

		var loaded = await sut.GetTrendingAllAsync(TimeWindow.Day);

		Assert.Single(loaded);
		Assert.Equal(3, loaded[0].Id);
	}

	[Fact]
	public async Task SaveTrendingAllAsync_ReSavingSameMovie_UpsertsInsteadOfDuplicating()
	{
		var sut = BuildSut();

		await sut.SaveTrendingAllAsync([new Movie { Id = 1, Title = "Original" }], TimeWindow.Day);
		await sut.SaveTrendingAllAsync([new Movie { Id = 1, Title = "Updated" }], TimeWindow.Day);

		var loaded = await sut.GetTrendingAllAsync(TimeWindow.Day);

		var single = Assert.Single(loaded);
		var movie = Assert.IsType<Movie>(single);
		Assert.Equal("Updated", movie.Title);
	}

	[Fact]
	public async Task SaveTrendingAllAsync_WithEmptyList_ClearsAllEntries()
	{
		var sut = BuildSut();
		await sut.SaveTrendingAllAsync([new Movie { Id = 1, Title = "X" }], TimeWindow.Day);

		await sut.SaveTrendingAllAsync([], TimeWindow.Day);

		var loaded = await sut.GetTrendingAllAsync(TimeWindow.Day);
		Assert.Empty(loaded);
	}

	[Fact]
	public async Task SaveTrendingAllAsync_SameMovieInDifferentSaves_DoesNotDuplicateOnMoviesTable()
	{
		var sut = BuildSut();

		await sut.SaveTrendingAllAsync([new Movie { Id = 1, Title = "M" }], TimeWindow.Day);
		await sut.SaveTrendingAllAsync([new Movie { Id = 1, Title = "M" }, new Movie { Id = 2, Title = "Other" }], TimeWindow.Day);

		await using var context = await ContextFactory.CreateDbContextAsync();
		var movieCount = context.Movies.Count();

		Assert.Equal(2, movieCount);
	}

	[Fact]
	public async Task SaveTrendingAllAsync_DayAndWeek_AreIsolated()
	{
		var sut = BuildSut();

		await sut.SaveTrendingAllAsync([new Movie { Id = 1, Title = "DayMovie" }], TimeWindow.Day);
		await sut.SaveTrendingAllAsync([new Tv { Id = 2, Name = "WeekTv" }], TimeWindow.Week);

		var day = await sut.GetTrendingAllAsync(TimeWindow.Day);
		var week = await sut.GetTrendingAllAsync(TimeWindow.Week);

		var dayMovie = Assert.IsType<Movie>(Assert.Single(day));
		var weekTv = Assert.IsType<Tv>(Assert.Single(week));
		Assert.Equal("DayMovie", dayMovie.Title);
		Assert.Equal("WeekTv", weekTv.Name);
	}

	[Fact]
	public async Task SaveTrendingAllAsync_OverwritingDay_DoesNotAffectWeek()
	{
		var sut = BuildSut();

		await sut.SaveTrendingAllAsync([new Movie { Id = 1, Title = "DayOriginal" }], TimeWindow.Day);
		await sut.SaveTrendingAllAsync([new Movie { Id = 99, Title = "WeekOnly" }], TimeWindow.Week);

		await sut.SaveTrendingAllAsync([new Movie { Id = 2, Title = "DayReplaced" }], TimeWindow.Day);

		var week = await sut.GetTrendingAllAsync(TimeWindow.Week);
		var weekMovie = Assert.IsType<Movie>(Assert.Single(week));
		Assert.Equal("WeekOnly", weekMovie.Title);
	}
}
