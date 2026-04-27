using MauiMovies.Infrastructure.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace MauiMovies.Infrastructure.Tests.Persistence;

public abstract class DatabaseTestBase : IDisposable
{
	readonly SqliteConnection connection;
	bool disposed;

	protected IDbContextFactory<AppDbContext> ContextFactory { get; }

	protected DatabaseTestBase()
	{
		connection = new SqliteConnection("Data Source=:memory:");
		connection.Open();

		var options = new DbContextOptionsBuilder<AppDbContext>()
			.UseSqlite(connection)
			.Options;

		ContextFactory = new TestDbContextFactory(options);

		using var context = ContextFactory.CreateDbContext();
		context.Database.EnsureCreated();
	}

	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposed)
			return;

		if (disposing)
			connection.Dispose();

		disposed = true;
	}

	sealed class TestDbContextFactory : IDbContextFactory<AppDbContext>
	{
		readonly DbContextOptions<AppDbContext> options;

		public TestDbContextFactory(DbContextOptions<AppDbContext> options) =>
			this.options = options;

		public AppDbContext CreateDbContext() => new(options);
	}
}
