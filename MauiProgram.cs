using Microsoft.Extensions.Logging;
using CalculateurAge.Services;
using CalculateurAge.ViewModels;
using CalculateurAge.Views;

namespace CalculateurAge;

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
			});

		builder.Services.AddSingleton<INavigationService, ShellNavigationService>();
		builder.Services.AddSingleton<CalculateurViewModel>();
		builder.Services.AddTransient<ResultatViewModel>();
		builder.Services.AddSingleton<MainPage>();
		builder.Services.AddTransient<ResultatPage>();
		builder.Services.AddSingleton<AppShell>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
