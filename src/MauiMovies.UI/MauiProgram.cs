using System.Reflection;
using CommunityToolkit.Maui;
using MauiMovies.Infrastructure.Api;
using MauiMovies.Infrastructure.DI;
using MauiMovies.UI.DI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MauiMovies.UI;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();

		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureMauiHandlers(handlers =>
			{
#if ANDROID || IOS
				handlers.AddHandler<Shell, Handlers.AppShellRenderer>();
#endif
			})
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("SourceSans3-Light.ttf", "SourceSans3Light");
				fonts.AddFont("SourceSans3-Regular.ttf", "SourceSans3Regular");
				fonts.AddFont("SourceSans3-Semibold.ttf", "SourceSans3SemiBold");
				fonts.AddFont("lucide.ttf", "Lucide");
			});

		var configuration = LoadConfiguration();
		var tmdbOptions = configuration.GetSection("Tmdb").Get<TmdbOptions>() ?? new TmdbOptions();
		var sqlitePath = Path.Combine(FileSystem.AppDataDirectory, "mauimovies.db");

		builder.Services.AddInfrastructure(sqlitePath, tmdbOptions);
		builder.Services.AddUI();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}

	static IConfiguration LoadConfiguration()
	{
		var assembly = Assembly.GetExecutingAssembly();
		using var stream = assembly.GetManifestResourceStream("MauiMovies.UI.appsettings.json");

		if (stream is null)
			return new ConfigurationBuilder().Build();

		return new ConfigurationBuilder().AddJsonStream(stream).Build();
	}
}
