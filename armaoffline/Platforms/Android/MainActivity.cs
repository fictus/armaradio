using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using AndroidX.Annotations;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using armaoffline.Platforms.Android;
using Microsoft.Extensions.Logging;
using static Android.OS.PowerManager;

namespace armaoffline
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        private WakeLock wakeLock;
        const int RequestStoragePermissionCode = 123;
        const int REQUEST_RECORD_AUDIO_PERMISSION = 200;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //// Request audio permissions at runtime for Android 6.0+
            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.RecordAudio)
                != Android.Content.PM.Permission.Granted)
            {
                ActivityCompat.RequestPermissions(this, new[]
                {
                    Manifest.Permission.RecordAudio 
                },
                REQUEST_RECORD_AUDIO_PERMISSION);
            }

            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage)
            != Android.Content.PM.Permission.Granted)
            {
                ActivityCompat.RequestPermissions(this, new[]
                {
                    Manifest.Permission.WriteExternalStorage,
                    Manifest.Permission.ReadExternalStorage
                },
                RequestStoragePermissionCode);
            }

            var intent = new Intent(Android.App.Application.Context, typeof(ScreenOffService));
            intent.SetAction(ScreenOffService.ActionStartScreenOffService);

            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
            {
                Android.App.Application.Context.StartForegroundService(intent); // For Android 8.0 and higher
            }
            else
            {
                Android.App.Application.Context.StartService(intent); // For Android versions below 8.0
            }

            //// Request battery optimization exemption
            //PowerManager powerManager = (PowerManager)GetSystemService(Context.PowerService);

            //if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            //{
            //    // Check if the app is already ignoring battery optimizations
            //    if (!powerManager.IsIgnoringBatteryOptimizations(Platform.CurrentActivity?.PackageName))
            //    {
            //        // Request battery optimization exemption
            //        var intent = new Intent(Android.Provider.Settings.ActionRequestIgnoreBatteryOptimizations);
            //        intent.SetData(Android.Net.Uri.Parse("package:" + Platform.CurrentActivity?.PackageName));

            //        try
            //        {
            //            Platform.CurrentActivity?.StartActivity(intent);
            //        }
            //        catch (ActivityNotFoundException)
            //        {
            //            // Fallback if the intent is not supported
            //            Android.Util.Log.Warn("BatteryOptimization", "Could not request battery optimization exemption");
            //        }
            //    }
            //}

            //// Acquire a wake lock with the correct flags
            //wakeLock = powerManager.NewWakeLock(WakeLockFlags.Partial, "armaoffline:AudioPlaybackLock");
            ////wakeLock = powerManager.NewWakeLock(
            ////    WakeLockFlags.Partial |
            ////    WakeLockFlags.AcquireCausesWakeup |
            ////    WakeLockFlags.OnAfterRelease,
            ////    "armaoffline:AudioPlaybackLock"
            ////);
            //wakeLock.Acquire();

            //// Start foreground service for audio playback
            //StartForegroundService(new Intent(this, typeof(AudioService)));
        }

        private void RequestRequiredPermissions()
        {
            List<string> permissionsToRequest = new List<string>
            {
                Manifest.Permission.RecordAudio,
                Manifest.Permission.WriteExternalStorage,
                Manifest.Permission.ReadExternalStorage
            };

            List<string> missingPermissions = new List<string>();

            foreach (var permission in permissionsToRequest)
            {
                if (ContextCompat.CheckSelfPermission(this, permission)
                    != Permission.Granted)
                {
                    missingPermissions.Add(permission);
                }
            }

            if (missingPermissions.Any())
            {
                ActivityCompat.RequestPermissions(this,
                    missingPermissions.ToArray(),
                    RequestStoragePermissionCode);
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            if (requestCode == RequestStoragePermissionCode)
            {
                if (grantResults.Length > 0 &&
                    grantResults[0] == Android.Content.PM.Permission.Granted)
                {
                    // Permission granted, you can now write to storage
                }
                else
                {
                    // Permission denied
                    Console.WriteLine("Storage permissions were denied!");
                }
            }
        }

        //protected override void OnDestroy()
        //{
        //    // Release the wake lock if it exists and is held
        //    if (wakeLock != null)
        //    {
        //        if (wakeLock.IsHeld)
        //        {
        //            wakeLock.Release();
        //        }
        //        wakeLock.Dispose();
        //        wakeLock = null;
        //    }

        //    //// Stop the foreground service
        //    //StopService(new Intent(this, typeof(AudioService)));

        //    base.OnDestroy();
        //}
    }

    [Service(Label = nameof(ScreenOffService))]
    [RequiresApi(Api = (int)BuildVersionCodes.R)]
    public class ScreenOffService : Service
    {
        private static readonly string TypeName = typeof(ScreenOffService).FullName;
        public static readonly string ActionStartScreenOffService = TypeName + ".action.START";

        internal const int NOTIFICATION_ID = 12345678;
        private const string NOTIFICATION_CHANNEL_ID = "screen_off_service_channel_01";
        private const string NOTIFICATION_CHANNEL_NAME = "screen_off_service_channel_name";
        private NotificationManager _notificationManager;

        private bool _isStarted;

        private readonly ScreenOffBroadcastReceiver _screenOffBroadcastReceiver;

        public ScreenOffService()
        {
            _screenOffBroadcastReceiver = Microsoft.Maui.Controls.Application.Current.Handler.MauiContext.Services.GetService<ScreenOffBroadcastReceiver>();
        }

        public override void OnCreate()
        {
            base.OnCreate();

            _notificationManager = GetSystemService(Context.NotificationService) as NotificationManager;

            RegisterScreenOffBroadcastReceiver();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            UnregisterScreenOffBroadcastReceiver();
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            CreateNotificationChannel(); // Elsewhere we must've prompted user to allow Notifications

            if (intent.Action == ActionStartScreenOffService)
            {
                try
                {
                    StartForeground();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unable to start Screen On/Off foreground svc: " + ex);
                }
            }

            return StartCommandResult.Sticky;
        }

        private void RegisterScreenOffBroadcastReceiver()
        {
            var filter = new IntentFilter();
            filter.AddAction(Intent.ActionScreenOff);
            RegisterReceiver(_screenOffBroadcastReceiver, filter);
        }

        private void UnregisterScreenOffBroadcastReceiver()
        {
            try
            {
                if (_screenOffBroadcastReceiver != null)
                {
                    UnregisterReceiver(_screenOffBroadcastReceiver);
                }
            }
            catch (Java.Lang.IllegalArgumentException ex)
            {
                Console.WriteLine($"Error while unregistering {nameof(ScreenOffBroadcastReceiver)}. {ex}");
            }
        }

        private void StartForeground()
        {
            if (!_isStarted)
            {
                Notification notification = BuildInitialNotification();
                StartForeground(NOTIFICATION_ID, notification);

                _isStarted = true;
            }
        }

        private Notification BuildInitialNotification()
        {
            var intentToShowMainActivity = BuildIntentToShowMainActivity();

            var notification = new NotificationCompat.Builder(this, NOTIFICATION_CHANNEL_ID)
                .SetContentTitle("armaoffline Screen Off Service") //Resource.String.app_name
                .SetContentText("Service is running...")
                .SetSmallIcon(Resource.Drawable.navigation_empty_icon) // .eip_logo_symbol_yellow Android top bar icon and Notification drawer item LHS icon
                .SetLargeIcon(Android.Graphics.BitmapFactory.DecodeResource(Resources, Resource.Drawable.splash)) // .eip_logo_yellow Notification drawer item RHS icon
                .SetContentIntent(intentToShowMainActivity)
                .SetOngoing(true)
                .Build();

            return notification;
        }

        private PendingIntent BuildIntentToShowMainActivity()
        {
            var mainActivityIntent = new Intent(this, typeof(MainActivity));
            mainActivityIntent.SetAction("ACTION_MAIN_ACTIVITY"); // Constants.ACTION_MAIN_ACTIVITY
            mainActivityIntent.SetFlags(ActivityFlags.SingleTop | ActivityFlags.ClearTask);
            mainActivityIntent.PutExtra("SERVICE_STARTED_KEY", true); // Constants.SERVICE_STARTED_KEY

            PendingIntent pendingIntent = PendingIntent.GetActivity(this, 0, mainActivityIntent, PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable);

            return pendingIntent;
        }

        private void CreateNotificationChannel()
        {
            NotificationChannel chan = new NotificationChannel(NOTIFICATION_CHANNEL_ID, NOTIFICATION_CHANNEL_NAME, NotificationImportance.Default)
            {
                LightColor = Microsoft.Maui.Graphics.Color.FromRgba(0, 0, 255, 0).ToInt(),
                LockscreenVisibility = NotificationVisibility.Public
            };

            _notificationManager.CreateNotificationChannel(chan);
        }

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }
    }

    [BroadcastReceiver(Name = "com.armaoffline.MobileApp.ScreenOffBroadcastReceiver", Label = "ScreenOffBroadcastReceiver", Exported = true)]
    [IntentFilter(new[] { Intent.ActionScreenOff }, Priority = (int)IntentFilterPriority.HighPriority)]
    public class ScreenOffBroadcastReceiver : BroadcastReceiver
    {
        private readonly ILogger<ScreenOffBroadcastReceiver> _logger;

        private PowerManager.WakeLock _wakeLock;

        public ScreenOffBroadcastReceiver()
        {
            _logger = Microsoft.Maui.Controls.Application.Current.Handler.MauiContext.Services.GetService<ILogger<ScreenOffBroadcastReceiver>>();
        }

        public override void OnReceive(Context context, Intent intent)
        {
            if (intent.Action == Intent.ActionScreenOff)
            {
                AcquireWakeLock();
            }
        }

        private void AcquireWakeLock()
        {
            _wakeLock?.Release();

            WakeLockFlags wakeFlags = WakeLockFlags.Partial;

            PowerManager pm = (PowerManager)Android.App.Application.Context.GetSystemService(Android.Content.Context.PowerService);
            _wakeLock = pm.NewWakeLock(wakeFlags, typeof(ScreenOffBroadcastReceiver).FullName);
            _wakeLock.Acquire();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            _wakeLock?.Release();
        }
    }
}
