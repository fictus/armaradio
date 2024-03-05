using armaradio.Models;
using armaradio.Models.Request;
using armaradio.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace armaradio.Controllers
{
    public class MusicController : Controller
    {
        private readonly IMusicRepo _musicRepo;
        public MusicController(
            IMusicRepo musicRepo    
        )
        {
            _musicRepo = musicRepo;
        }

        [HttpGet]
        public IActionResult GetTop50UserPickedSongs()
        {
            try
            {
                List<TrackDataItem> returnItem = _musicRepo.Tracks_GetTop50Songs() ?? new List<TrackDataItem>();
                var finalList = returnItem.Select(sg =>
                {
                    return new
                    {
                        tid = sg.tid,
                        artistName = sg.artist_name,
                        songName = sg.track_name
                    };
                });

                return new JsonResult(finalList);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message.ToString());
            }
        }

        [HttpPost]
        public IActionResult GetUrlByArtistSongName([FromBody] MusicUrlByArtistSongRequest value)
        {
            try
            {
                string currentVideoId = _musicRepo.Youtube_GetUrlByArtistNameSongName(value.artistName, value.songName);

                return new JsonResult(new
                {
                    hasVideo = !string.IsNullOrWhiteSpace(currentVideoId),
                    url = $"https://www.youtube.com/watch?v={currentVideoId}",
                    embedUrl = $"https://www.youtube.com/embed/{currentVideoId}?enablejsapi=1", //&autoplay=1
                    videoId = currentVideoId
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message.ToString());
            }
        }

        [HttpPost]
        public IActionResult UploadCustomPlaylist([FromBody] MusicCustomPlaylistRequest value)
        {
            try
            {
                if (value == null || string.IsNullOrWhiteSpace(value.PlayList))
                {
                    throw new Exception("Playlist is required");
                }

                List<TrackDataItem> returnItem = new List<TrackDataItem>();
                List<string> allLines = value.PlayList.Split('\n', StringSplitOptions.RemoveEmptyEntries).ToList();

                for (int i = 0; i < allLines.Count; i++)
                {
                    string currentLine = allLines[i].Replace("--", "^");
                    List<string> parts = currentLine.Split('^', StringSplitOptions.RemoveEmptyEntries).ToList();

                    if (parts.Count >= 2)
                    {
                        returnItem.Add(new TrackDataItem()
                        {
                            tid = i,
                            artist_name = parts[0].Trim(),
                            track_name = parts[1].Trim()
                        });
                    }
                }

                var finalList = returnItem.Select(sg =>
                {
                    return new
                    {
                        tid = sg.tid,
                        artistName = sg.artist_name,
                        songName = sg.track_name
                    };
                });

                return new JsonResult(finalList);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message.ToString());
            }
        }
    }
}
