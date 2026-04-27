using MauiMovies.Core.Entities;
using MauiMovies.Core.Enums;
using MauiMovies.Core.Interfaces.Repositories;
using MauiMovies.Infrastructure.Persistence.Entities;
using MauiMovies.Infrastructure.Persistence.Mapping;
using Microsoft.EntityFrameworkCore;

namespace MauiMovies.Infrastructure.Persistence.Repositories;

public class MediaRepository : IMediaRepository
{
	readonly IDbContextFactory<AppDbContext> contextFactory;

	public MediaRepository(IDbContextFactory<AppDbContext> contextFactory)
	{
		this.contextFactory = contextFactory;
	}

	public async Task<IReadOnlyList<MediaItem>> GetTrendingAllAsync(TimeWindow timeWindow, CancellationToken cancellationToken = default)
	{
		var listType = ToListType(timeWindow);

		await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

		var entries = await context.MediaListItems
			.AsNoTracking()
			.Where(e => e.ListType == listType)
			.OrderBy(e => e.Position)
			.ToListAsync(cancellationToken);

		if (entries.Count == 0)
			return [];

		var movieIds = entries.Where(e => e.MediaType == MediaType.Movie).Select(e => e.MediaId).ToHashSet();
		var tvIds = entries.Where(e => e.MediaType == MediaType.Tv).Select(e => e.MediaId).ToHashSet();
		var personIds = entries.Where(e => e.MediaType == MediaType.Person).Select(e => e.MediaId).ToHashSet();

		var movies = await LoadDictionaryAsync(context.Movies, movieIds, m => m.Id, cancellationToken);
		var tvShows = await LoadDictionaryAsync(context.TvShows, tvIds, t => t.Id, cancellationToken);
		var persons = await LoadDictionaryAsync(context.Persons, personIds, p => p.Id, cancellationToken);

		var result = new List<MediaItem>(entries.Count);

		foreach (var entry in entries)
		{
			MediaItem? item = entry.MediaType switch
			{
				MediaType.Movie => movies.GetValueOrDefault(entry.MediaId)?.ToDomain(),
				MediaType.Tv => tvShows.GetValueOrDefault(entry.MediaId)?.ToDomain(),
				MediaType.Person => persons.GetValueOrDefault(entry.MediaId)?.ToDomain(),
				_ => null,
			};

			if (item is not null)
				result.Add(item);
		}

		return result;
	}

	public async Task SaveTrendingAllAsync(IEnumerable<MediaItem> items, TimeWindow timeWindow, CancellationToken cancellationToken = default)
	{
		var listType = ToListType(timeWindow);
		var itemList = items.ToList();

		await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
		await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

		await UpsertMoviesAsync(context, itemList.OfType<Movie>().ToList(), cancellationToken);
		await UpsertTvShowsAsync(context, itemList.OfType<Tv>().ToList(), cancellationToken);
		await UpsertPersonsAsync(context, itemList.OfType<Person>().ToList(), cancellationToken);
		await context.SaveChangesAsync(cancellationToken);

		await context.MediaListItems
			.Where(e => e.ListType == listType)
			.ExecuteDeleteAsync(cancellationToken);

		var newEntries = itemList.Select((item, index) => new MediaListItemEntity
		{
			ListType = listType,
			MediaType = item.MediaType,
			MediaId = item.Id,
			Position = index,
		}).ToList();

		context.MediaListItems.AddRange(newEntries);
		await context.SaveChangesAsync(cancellationToken);

		await transaction.CommitAsync(cancellationToken);
	}

	static MediaListType ToListType(TimeWindow timeWindow) => timeWindow switch
	{
		TimeWindow.Week => MediaListType.TrendingAllWeek,
		_ => MediaListType.TrendingAllDay,
	};

	static async Task<Dictionary<int, TEntity>> LoadDictionaryAsync<TEntity>(
		DbSet<TEntity> dbSet,
		HashSet<int> ids,
		Func<TEntity, int> keySelector,
		CancellationToken cancellationToken) where TEntity : class
	{
		if (ids.Count == 0)
			return [];

		var list = await dbSet
			.AsNoTracking()
			.Where(e => ids.Contains(EF.Property<int>(e, "Id")))
			.ToListAsync(cancellationToken);

		return list.ToDictionary(keySelector);
	}

	static async Task UpsertMoviesAsync(AppDbContext context, List<Movie> movies, CancellationToken cancellationToken)
	{
		if (movies.Count == 0)
			return;

		var ids = movies.Select(m => m.Id).ToList();
		var existingIds = await context.Movies
			.AsNoTracking()
			.Where(m => ids.Contains(m.Id))
			.Select(m => m.Id)
			.ToHashSetAsync(cancellationToken);

		foreach (var movie in movies)
		{
			var entity = movie.ToEntity();
			context.Entry(entity).State = existingIds.Contains(movie.Id)
				? EntityState.Modified
				: EntityState.Added;
		}
	}

	static async Task UpsertTvShowsAsync(AppDbContext context, List<Tv> shows, CancellationToken cancellationToken)
	{
		if (shows.Count == 0)
			return;

		var ids = shows.Select(s => s.Id).ToList();
		var existingIds = await context.TvShows
			.AsNoTracking()
			.Where(s => ids.Contains(s.Id))
			.Select(s => s.Id)
			.ToHashSetAsync(cancellationToken);

		foreach (var show in shows)
		{
			var entity = show.ToEntity();
			context.Entry(entity).State = existingIds.Contains(show.Id)
				? EntityState.Modified
				: EntityState.Added;
		}
	}

	static async Task UpsertPersonsAsync(AppDbContext context, List<Person> persons, CancellationToken cancellationToken)
	{
		if (persons.Count == 0)
			return;

		var ids = persons.Select(p => p.Id).ToList();
		var existingIds = await context.Persons
			.AsNoTracking()
			.Where(p => ids.Contains(p.Id))
			.Select(p => p.Id)
			.ToHashSetAsync(cancellationToken);

		foreach (var person in persons)
		{
			var entity = person.ToEntity();
			context.Entry(entity).State = existingIds.Contains(person.Id)
				? EntityState.Modified
				: EntityState.Added;
		}
	}
}
