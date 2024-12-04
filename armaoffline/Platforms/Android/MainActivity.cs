using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using armaoffline.Platforms.Android;
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
                ActivityCompat.RequestPermissions(this,
                    new[] { Manifest.Permission.RecordAudio },
                    REQUEST_RECORD_AUDIO_PERMISSION);
            }

            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage)
            != Android.Content.PM.Permission.Granted)
            {
                ActivityCompat.RequestPermissions(this,
                    new[] {
                    Manifest.Permission.WriteExternalStorage,
                    Manifest.Permission.ReadExternalStorage
                    },
                    RequestStoragePermissionCode);
            }

            // Acquire wake lock
            PowerManager powerManager = (PowerManager)GetSystemService(Context.PowerService);
            wakeLock = powerManager.NewWakeLock(WakeLockFlags.Partial, "armaoffline:AudioPlaybackLock");
            wakeLock.Acquire();

            // Start foreground service for audio playback
            StartForegroundService(new Intent(this, typeof(AudioService)));
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
    }
}
