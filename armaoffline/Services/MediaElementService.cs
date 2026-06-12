using CommunityToolkit.Maui.Core;
//using CommunityToolkit.Maui.Core.Primitives;
using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls;

namespace armaoffline.Services
{
    public class MediaElementService : IMediaElementService
    {
        private readonly MediaElement _mediaElement;

        public event EventHandler<bool>? MediaLoadedChanged;
        public event EventHandler? MediaEndedEvent;
        public event EventHandler<MediaFailedEventArgs>? MediaFailedEvent;
        public event EventHandler? MediaOpenedEvent;
        private string? _pendingArtist;
        private string? _pendingSong;

        public MediaElementService(MediaElement mediaElement)
        {
            _mediaElement = mediaElement
                ?? throw new ArgumentNullException(nameof(mediaElement));

            _mediaElement.MediaEnded  += OnMediaEnded;
            _mediaElement.MediaFailed += OnMediaFailedEvent;
            _mediaElement.MediaOpened += OnMediaOpened;
            _mediaElement.ShouldKeepScreenOn = false;
            _mediaElement.ShouldAutoPlay = true;

#if IOS || MACCATALYST
            // Configure the iOS audio session so playback continues when the
            // screen locks or the app goes to the background.
            ConfigureIosAudioSession();
#endif
        }

        // ------------------------------------------------------------------ //
        // Playback controls
        // ------------------------------------------------------------------ //

        public async Task SetMediaSource(string source, string artist, string song)
        {
            if (_mediaElement == null)
            {
                return;
            }

            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                _mediaElement.Stop();
                _mediaElement.MetadataArtist = artist;
                _mediaElement.MetadataTitle = song;
                _mediaElement.Source = new Uri(source);
            });
        }

        public async Task SetMetaData(string artistName, string songName)
        {
            _pendingArtist = artistName;
            _pendingSong = songName;

            if (_mediaElement != null)
            {
                _mediaElement.MetadataArtist = artistName;
                _mediaElement.MetadataTitle  = songName;
            }

            await Task.CompletedTask;
        }

        public async Task Play()
        {
            if (_mediaElement != null &&
                !string.IsNullOrEmpty(_mediaElement.Source?.ToString()))
            {
                _mediaElement.Play();
            }

            await Task.CompletedTask;
        }

        public async Task Pause()
        {
            _mediaElement?.Pause();
            await Task.CompletedTask;
        }

        public async Task Stop()
        {
            _mediaElement?.Stop();
            await Task.CompletedTask;
        }

        public async Task Seek(TimeSpan position)
        {
            _mediaElement?.SeekTo(position);
            await Task.CompletedTask;
        }

        public async Task<TimeSpan> GetDuration()
            => await Task.FromResult(_mediaElement?.Duration ?? TimeSpan.Zero);

        public async Task<TimeSpan> GetCurrentPosition()
            => await Task.FromResult(_mediaElement?.Position ?? TimeSpan.Zero);

        public bool IsMediaLoaded()
            => _mediaElement != null
               && !string.IsNullOrEmpty(_mediaElement.Source?.ToString())
               && _mediaElement.Duration > TimeSpan.Zero;

        public void CleanupMediaSession()
        {
            if (_mediaElement != null)
            {
                _mediaElement.MediaEnded  -= OnMediaEnded;
                _mediaElement.MediaFailed -= OnMediaFailedEvent;
                _mediaElement.Stop();
                _mediaElement.Source = null;
                _mediaElement.Dispose();
            }
        }

        // ------------------------------------------------------------------ //
        // Event handlers
        // ------------------------------------------------------------------ //

        private void OnMediaEnded(object? sender, EventArgs e)
            => MediaEndedEvent?.Invoke(this, e);

        private void OnMediaOpened(object sender, EventArgs e)
        {
            if (_mediaElement != null)
            {
                // Re-apply metadata now that the new MediaSession exists,
                // so the notification/lock-screen reflects the *current* track.
                _mediaElement.MetadataArtist = _pendingArtist;
                _mediaElement.MetadataTitle = _pendingSong;
            }

            MediaLoadedChanged?.Invoke(this, IsMediaLoaded());
        }

        private void OnMediaFailedEvent(object? sender, MediaFailedEventArgs e)
            => MediaFailedEvent?.Invoke(this, e);

        // ------------------------------------------------------------------ //
        // Platform-specific helpers
        // ------------------------------------------------------------------ //

#if IOS || MACCATALYST
        /// <summary>
        /// Sets the AVAudioSession category to <c>Playback</c> so audio
        /// continues when the device is locked or the ringer switch is off.
        /// Must be called on the main thread before playback starts.
        /// </summary>
        private static void ConfigureIosAudioSession()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    AVFoundation.AVAudioSession.SharedInstance().SetCategory(
                        AVFoundation.AVAudioSession.CategoryPlayback,
                        AVFoundation.AVAudioSessionCategoryOptions.MixWithOthers,
                        out Foundation.NSError? error);

                    if (error != null)
                        System.Diagnostics.Debug.WriteLine(
                            $"[AudioSession] SetCategory error: {error.LocalizedDescription}");

                    AVFoundation.AVAudioSession.SharedInstance().SetActive(true, out error);

                    if (error != null)
                        System.Diagnostics.Debug.WriteLine(
                            $"[AudioSession] SetActive error: {error.LocalizedDescription}");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(
                        $"[AudioSession] Configuration failed: {ex.Message}");
                }
            });
        }
#endif
    }
}
