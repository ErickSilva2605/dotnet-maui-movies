using MauiMovies.Core.Interfaces.DataSources;
using MauiMovies.Core.Interfaces.Repositories;
using MauiMovies.Infrastructure.Api;
using MauiMovies.Infrastructure.Api.DataSources;
using MauiMovies.Infrastructure.Api.Http;
using MauiMovies.Infrastructure.Persistence;
using MauiMovies.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace MauiMovies.Infrastructure.DI;

public static class InfrastructureExtensions
{
	public static IServiceCollection AddInfrastructure(
		this IServiceCollection services,
		string sqlitePath,
		TmdbOptions tmdbOptions)
	{
		services.AddSingleton(tmdbOptions);

		services.AddDbContextFactory<AppDbContext>(options =>
			options.UseSqlite($"Data Source={sqlitePath}"));

		services.AddSingleton<DatabaseInitializer>();

		services.AddSingleton<IRequestProvider, RequestProvider>();

		services.AddTransient<IMediaRemoteDataSource, TmdbMediaDataSource>();
		services.AddTransient<IMediaRepository, MediaRepository>();

		return services;
	}
}
