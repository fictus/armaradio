using Microsoft.Extensions.Logging;
using armaoffline.Repositories;
using armaoffline.Services;
using CommunityToolkit.Maui;
using armaoffline.Providers;
using armaoffline.Data;

namespace armaoffline
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkitMediaElement()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddSingleton<GlobalState>();
            builder.Services.AddTransient<IArmaApi, ArmaApi>();
            builder.Services.AddTransient<IDapperHelper, DapperHelper>();


            builder.Services.AddMauiBlazorWebView();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            var app = builder.Build();

            ServiceLocator.ServiceProvider = app.Services;

            return app;
        }
    }
}
