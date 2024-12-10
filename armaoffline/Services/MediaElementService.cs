using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Primitives;
using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls;

namespace armaoffline.Services
{
    public class MediaElementService : IMediaElementService
    {
        private readonly MediaElement _mediaElement;
        public event EventHandler<bool> MediaLoadedChanged;
        public event EventHandler MediaEndedEvent;
        public event EventHandler<MediaFailedEventArgs> MediaFailedEvent;

        public MediaElementService(MediaElement mediaElement)
        {
            _mediaElement = mediaElement ?? throw new ArgumentNullException(nameof(mediaElement));
            //_mediaElement.MediaOpened += OnMediaOpened;
            _mediaElement.MediaEnded += OnMediaEnded;
            _mediaElement.MediaFailed += OnMediaFailedEvent;

#if ANDROID
            _mediaElement.ShouldKeepScreenOn = true;
#endif
#if IOS
            _mediaElement.ShouldKeepScreenOn = true;  
#endif
        }

        public Task SetMediaSource(string source)
        {
            if (_mediaElement != null)
            {
                _mediaElement.Stop();
                _mediaElement.Source = null;

                Task.Delay(100);

                _mediaElement.Source = new Uri(source);
                //_mediaElement.IsVisible = false;
            }

            return Task.CompletedTask;
        }

        public Task SetMetaData(string artistName, string songName)
        {
            if (_mediaElement != null)
            {
                _mediaElement.MetadataArtist = artistName;
                _mediaElement.MetadataTitle = songName;
            }

            return Task.CompletedTask;
        }

        public Task Play()
        {
            if (_mediaElement != null)
            {
                if (!string.IsNullOrEmpty(_mediaElement.Source?.ToString()))
                {
                    _mediaElement.Play();
                }
            }

            return Task.CompletedTask;
        }

        public Task Pause()
        {
            _mediaElement?.Pause();

            return Task.CompletedTask;
        }

        public Task Stop()
        {
            _mediaElement?.Stop();

            return Task.CompletedTask;
        }

        public Task Seek(TimeSpan position)
        {
            if (_mediaElement != null)
            {
                _mediaElement.SeekTo(position);
            }

            return Task.CompletedTask;
        }

        public Task<TimeSpan> GetDuration()
        {
            return Task.FromResult(_mediaElement?.Duration ?? TimeSpan.Zero);
        }

        public Task<TimeSpan> GetCurrentPosition()
        {
            return Task.FromResult(_mediaElement?.Position ?? TimeSpan.Zero);
        }

        private void OnMediaOpened(object sender, EventArgs e)
        {
            // Trigger the event when media is loaded
            MediaLoadedChanged?.Invoke(this, IsMediaLoaded());
        }

        private void OnMediaEnded(object sender, EventArgs e)
        {
            // Trigger the media ended event
            MediaEndedEvent?.Invoke(this, e);
        }

        private void OnMediaFailedEvent(object sender, MediaFailedEventArgs e)
        {
            // Trigger the media ended event
            MediaFailedEvent?.Invoke(this, e);
        }

        public bool IsMediaLoaded()
        {
            return _mediaElement != null
                   && !string.IsNullOrEmpty(_mediaElement.Source?.ToString())
                   && _mediaElement.Duration > TimeSpan.Zero;
        }

        public void CleanupMediaSession()
        {
            if (_mediaElement != null)
            {
                _mediaElement.MediaEnded -= OnMediaEnded;
                _mediaElement.Stop();
                _mediaElement.Source = null;
                _mediaElement.Dispose();
            }
        }
    }
}
