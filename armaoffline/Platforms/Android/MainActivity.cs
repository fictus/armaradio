using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using armaoffline.Services;
using CommunityToolkit.Maui.Views;
using static Android.OS.PowerManager;

namespace armaoffline
{
    [Activity(
        Theme = "@style/Maui.SplashTheme",
        ResizeableActivity = true,
        MainLauncher = true,
        LaunchMode = LaunchMode.SingleTask,
        ConfigurationChanges =
            ConfigChanges.ScreenSize |
            ConfigChanges.Orientation |
            ConfigChanges.UiMode |
            ConfigChanges.ScreenLayout |
            ConfigChanges.SmallestScreenSize |
            ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        //// PARTIAL wake lock keeps the CPU running when the screen turns off,
        //// which is required for background audio on Android.
        //private WakeLock? _wakeLock;

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //AcquireWakeLock();
            RequestBatteryOptimisationExemption();
        }

        //// Re-acquire the wake lock if the activity is resumed (e.g. user returns
        //// to the app after the screen came back on and the lock was released).
        //protected override void OnResume()
        //{
        //    base.OnResume();
        //    if (_wakeLock == null || !_wakeLock.IsHeld)
        //        AcquireWakeLock();
        //}

        protected override void OnDestroy()
        {
            //ReleaseWakeLock();

            // Stop the media session so the foreground service and its
            // notification are cleaned up properly.
            var mediaService = IPlatformApplication.Current?.Services
                .GetService(typeof(IMediaElementService)) as IMediaElementService;
            mediaService?.CleanupMediaSession();

            NotificationManagerCompat.From(this).CancelAll();

            // The MediaElement foreground service keeps the process alive even
            // after OnDestroy when audio is playing. Kill the process explicitly
            // so the app fully terminates when the user closes it.
            Process.KillProcess(Process.MyPid());

            base.OnDestroy();
        }

        // ------------------------------------------------------------------ //

        //private void AcquireWakeLock()
        //{
        //    var pm = (PowerManager?)GetSystemService(Context.PowerService);
        //    if (pm == null) return;

        //    _wakeLock = pm.NewWakeLock(WakeLockFlags.Partial, "armaoffline:AudioPlaybackLock");
        //    // No timeout — we release manually in OnDestroy / when playback stops.
        //    _wakeLock.Acquire();
        //}

        //private void ReleaseWakeLock()
        //{
        //    if (_wakeLock?.IsHeld == true)
        //        _wakeLock.Release();
        //    _wakeLock?.Dispose();
        //    _wakeLock = null;
        //}

        private void RequestBatteryOptimisationExemption()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.M) return;

            var pm = (PowerManager?)GetSystemService(Context.PowerService);
            if (pm == null) return;

            var packageName = Platform.CurrentActivity?.PackageName;
            if (string.IsNullOrEmpty(packageName)) return;

            if (pm.IsIgnoringBatteryOptimizations(packageName)) return;

            try
            {
                var intent = new Intent(
                    Android.Provider.Settings.ActionRequestIgnoreBatteryOptimizations);
                intent.SetData(Android.Net.Uri.Parse("package:" + packageName));
                Platform.CurrentActivity?.StartActivity(intent);
            }
            catch (ActivityNotFoundException)
            {
                Android.Util.Log.Warn(
                    "BatteryOptimization",
                    "Device does not support ACTION_REQUEST_IGNORE_BATTERY_OPTIMIZATIONS");
            }
        }
    }
}
