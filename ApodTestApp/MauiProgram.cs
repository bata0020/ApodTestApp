using Microsoft.Extensions.Logging;

namespace ApodTestApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");

				//added fonts
				fonts.AddFont("fa-regular-400.ttf", "FA-R");
				fonts.AddFont("fa-brands-400.ttf", "FA-B");
				fonts.AddFont("fa-solid-900.ttf", "FA-S");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
