using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using AndroidX.Core.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace armaoffline.Platforms.Android
{
    [Service]
    internal class AudioService : Service
    {
        private MediaPlayer mediaPlayer;
        private const int NOTIFICATION_ID = 1;

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            // Create a foreground notification
            var notification = CreateForegroundNotification();
            StartForeground(NOTIFICATION_ID, notification);

            return StartCommandResult.Sticky;
        }

        private Notification CreateForegroundNotification()
        {
            var channelId = "audio_playback_channel";

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channel = new NotificationChannel(
                    channelId,
                    "Audio Playback",
                    NotificationImportance.Low);

                var notificationManager = GetSystemService(NotificationService) as NotificationManager;
                notificationManager.CreateNotificationChannel(channel);
            }

            var builder = new NotificationCompat.Builder(this, channelId)
                .SetContentTitle("Audio Playing")
                .SetContentText("Background audio playback")
                .SetSmallIcon(Resource.Mipmap.appicon)
                .SetPriority(NotificationCompat.PriorityLow);

            return builder.Build();
        }

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override void OnDestroy()
        {
            mediaPlayer?.Release();
            base.OnDestroy();
        }
    }
}
