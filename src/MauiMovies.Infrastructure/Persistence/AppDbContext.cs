using MauiMovies.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace MauiMovies.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
	{
	}

	public DbSet<MovieEntity> Movies => Set<MovieEntity>();
	public DbSet<TvEntity> TvShows => Set<TvEntity>();
	public DbSet<PersonEntity> Persons => Set<PersonEntity>();
	public DbSet<MediaListItemEntity> MediaListItems => Set<MediaListItemEntity>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
	}
}
