using Microsoft.EntityFrameworkCore;

namespace MauiMovies.Infrastructure.Persistence;

public class DatabaseInitializer
{
	readonly IDbContextFactory<AppDbContext> contextFactory;

	public DatabaseInitializer(IDbContextFactory<AppDbContext> contextFactory)
	{
		this.contextFactory = contextFactory;
	}

	public async Task InitializeAsync(CancellationToken cancellationToken = default)
	{
		await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
		await context.Database.MigrateAsync(cancellationToken);
	}
}
