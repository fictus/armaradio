using Microsoft.Extensions.Logging;
using armaoffline.Repositories;
using armaoffline.Services;
using armaoffline.Providers;
using armaoffline.Data;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Views;

namespace armaoffline
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            MauiApp app = null;
            MauiAppBuilder builder = null;
            var startupErrorService = new StartupErrorService();

            try
            {
                builder = MauiApp.CreateBuilder();
                builder.Services.AddSingleton(startupErrorService);

                builder
                    .UseMauiApp<App>()
                    .UseMauiCommunityToolkitMediaElement()                   
                    .ConfigureFonts(fonts =>
                    {
                        fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    });

                // Detailed logging for service registration
                builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
                builder.Logging.AddConsole();
                builder.Logging.AddDebug();
                builder.Logging.SetMinimumLevel(LogLevel.Trace);

                try
                {
                    builder.Services.AddSingleton<GlobalState>();
                    builder.Services.AddTransient<IArmaApi, ArmaApi>();
                    builder.Services.AddTransient<IDapperHelper, DapperHelper>();
                    builder.Services.AddMauiBlazorWebView();

    #if DEBUG
                    builder.Services.AddBlazorWebViewDeveloperTools();
                    builder.Logging.AddDebug();
    #endif

                    builder.Services.AddSingleton<IMediaElementService>(sp =>
                    {
                        var mainPage = Application.Current?.MainPage as MainPage;
                        var mediaElement = mainPage?.FindByName<MediaElement>("myMediaElement");

                        if (mediaElement == null)
                        {
                            throw new InvalidOperationException("MediaElement not found");
                        }

                        return new MediaElementService(mediaElement);
                    });
                }
                catch (Exception serviceEx)
                {
                    System.Diagnostics.Debug.WriteLine($"Service Registration Error: {serviceEx}");
                    Console.WriteLine($"Service Registration Error: {serviceEx}");

                    startupErrorService.AddError($"Startup Error: {serviceEx.Message} - {serviceEx.StackTrace}");

                    //throw;
                }

                app = builder.Build();

                ServiceLocator.ServiceProvider = app.Services;
            }
            catch (Exception ex)
            {
                // Multiple logging strategies
                System.Diagnostics.Debug.WriteLine($"MAUI Startup Error: {ex}");
                Console.WriteLine($"MAUI Startup Error: {ex}");

                // Log full stack trace
                System.Diagnostics.Debug.WriteLine($"Full Stack Trace: {ex.StackTrace}");

                startupErrorService.AddError($"Startup Error: {ex.Message} - {ex.StackTrace}");

                // Rethrow to ensure the error is visible
                //throw;
            }

            return app;
        }

        public class StartupErrorService
        {
            public List<string> ErrorMessage { get; set; } = new List<string>();
            public bool HasError => ErrorMessage.Count > 0;

            public void AddError(string errorMessage)
            {
                ErrorMessage.Add(errorMessage);
            }
        }
    }
}
