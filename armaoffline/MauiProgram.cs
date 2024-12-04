using Microsoft.Extensions.Logging;
using armaoffline.Repositories;
using armaoffline.Services;
using armaoffline.Providers;
using armaoffline.Data;
using Plugin.Maui.Audio;

namespace armaoffline
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            MauiApp app = null;

            try
            {
                var builder = MauiApp.CreateBuilder();
                builder
                    .UseMauiApp<App>()
                    //.UseMauiCommunityToolkitMediaElement()
                    .AddAudio(
                    playbackOptions =>
                    {
    #if IOS || MACCATALYST
					    playbackOptions.Category = AVFoundation.AVAudioSessionCategory.Playback;
    #endif
                    },
                    recordingOptions =>
                    {
    #if IOS || MACCATALYST
					    recordingOptions.Category = AVFoundation.AVAudioSessionCategory.Record;
					    recordingOptions.Mode = AVFoundation.AVAudioSessionMode.Default;
					    recordingOptions.CategoryOptions = AVFoundation.AVAudioSessionCategoryOptions.MixWithOthers;
    #endif
                    })
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
                }
                catch (Exception serviceEx)
                {
                    System.Diagnostics.Debug.WriteLine($"Service Registration Error: {serviceEx}");
                    Console.WriteLine($"Service Registration Error: {serviceEx}");
                    throw;
                }

                app = builder.Build();

                ServiceLocator.ServiceProvider = app.Services;

                return app;
            }
            catch (Exception ex)
            {
                // Multiple logging strategies
                System.Diagnostics.Debug.WriteLine($"MAUI Startup Error: {ex}");
                Console.WriteLine($"MAUI Startup Error: {ex}");

                // Log full stack trace
                System.Diagnostics.Debug.WriteLine($"Full Stack Trace: {ex.StackTrace}");

                // Rethrow to ensure the error is visible
                throw;
            }
        }
    }
}
