using CommunityToolkit.Maui;
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

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}