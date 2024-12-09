using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace armaoffline.Services
{
    public interface IMediaElementService
    {
        event EventHandler<bool> MediaLoadedChanged;
        event EventHandler MediaEndedEvent;
        Task SetMediaSource(string source);
        Task SetMetaData(string artistName, string songName);
        Task Play();
        Task Pause();
        Task Stop();
        Task Seek(TimeSpan position);
        Task<TimeSpan> GetDuration();
        Task<TimeSpan> GetCurrentPosition();
        bool IsMediaLoaded();
        void CleanupMediaSession();
    }
}
