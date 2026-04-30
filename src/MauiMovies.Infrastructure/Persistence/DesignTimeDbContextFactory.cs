using Microsoft.EntityFrameworkCore.Design;

namespace MauiMovies.Infrastructure.Persistence;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
	public AppDbContext CreateDbContext(string[] args)
	{
		var options = new DbContextOptionsBuilder<AppDbContext>()
			.UseSqlite("Data Source=design_time.db")
			.Options;

		return new AppDbContext(options);
	}
}
