using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using AndroidX.Annotations;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using armaoffline.Services;
using CommunityToolkit.Maui.Views;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls;
using static Android.OS.PowerManager;

namespace armaoffline
{
    [Activity(Theme = "@style/Maui.SplashTheme", ResizeableActivity = true, MainLauncher = true, LaunchMode = LaunchMode.SingleTask, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        private WakeLock wakeLock;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Acquire a wake lock with the correct flags
            PowerManager powerManager = (PowerManager)GetSystemService(Context.PowerService);
            wakeLock = powerManager.NewWakeLock(WakeLockFlags.Partial, "armaoffline:AudioPlaybackLock");
            wakeLock.Acquire();
        }

        protected override void OnDestroy()
        {
            wakeLock?.Release();

            // Properly release media session and stop service

            var _mediaPlayer = IPlatformApplication.Current.Services.GetService(typeof(IMediaElementService)) as IMediaElementService;
            _mediaPlayer?.CleanupMediaSession();

            // Cancel the media notification
            NotificationManagerCompat notificationManager = NotificationManagerCompat.From(this);
            notificationManager.CancelAll();

            // for some reason the app does not fully close when Audio is playing, the only way to fully terminate it is to kill it this way 
            Process.KillProcess(Process.MyPid());

            base.OnDestroy();
        }
    }
}
