using armaradio.Attributes;
using armaradio.Models;
using armaradio.Models.ArmaAuth;
using armaradio.Models.Request;
using armaradio.Models.Response;
using armaradio.Models.Youtube;
using armaradio.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Pipes;
using System.Net;
using System.Net.Mime;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using YoutubeExplode;
using YoutubeExplode.Converter;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;

namespace armaradio.Controllers
{
    [DisableCors]
    public class MusicController : Controller
    {
        private readonly bool IsLinux = false;
        private readonly IMusicRepo _musicRepo;
        private readonly IArmaAuth _authControl;
        private readonly IWebHostEnvironment _hostEnvironment;
        //private IPage _page;
        public MusicController(
            IMusicRepo musicRepo,
            IArmaAuth authControl,
            IWebHostEnvironment hostEnvironment
            //IPage page
        )
        {
            _musicRepo = musicRepo;
            _authControl = authControl;
            _hostEnvironment = hostEnvironment;
            //_page = page;

            IsLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        }

        [HttpGet]
        public IActionResult GetCurrentTop100()
        {
            try
            {
                List<TrackDataItem> returnItem = _musicRepo.GetCurrentTop100();
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

        [HttpGet]
        public IActionResult GetCurrentTop40DanceSingles()
        {
            try
            {
                List<TrackDataItem> returnItem = _musicRepo.GetCurrentTop40DanceSingles();
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

        [HttpGet]
        public IActionResult GetCurrentTranceTop100()
        {
            try
            {
                List<TrackDataItem> returnItem = _musicRepo.GetCurrentTranceTop100();
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

        [HttpGet]
        public IActionResult GetCurrentTranceHype100()
        {
            try
            {
                List<TrackDataItem> returnItem = _musicRepo.GetCurrentTranceHype100();
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
        public IActionResult FindArtists([FromBody] MusicSearchArtistRequest value)
        {
            try
            {
                List<ArmaArtistDataItem> returnItem = _musicRepo.Artist_FindArtists(value.SearchPhrase) ?? new List<ArmaArtistDataItem>();

                return new JsonResult(returnItem);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message.ToString());
            }
        }

        [HttpPost]
        public IActionResult FindAlbumsForArtists([FromBody] ArtistAlbumResponse value)
        {
            try
            {
                ArmaArtistAlbumsResponse returnItem = _musicRepo.Albums_GetArtistsAlbums(value.ArtistId);

                return new JsonResult(returnItem);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message.ToString());
            }
        }

        [HttpPost]
        public IActionResult LoadAlbumSongs([FromBody] AlbumSongsRequest value)
        {
            try
            {
                List<ArmaAlbumSongDataItem> returnItem = _musicRepo.Albums_GetAlbumSongs(value.ArtistId, value.AlbumId);
                var finalList = returnItem.Select(sg =>
                {
                    return new
                    {
                        tid = sg.Id,
                        artistName = sg.NameSearch,
                        songName = sg.SongTitleFlat
                    };
                });

                return new JsonResult(finalList);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message.ToString());
            }
        }

        [HttpGet]
        public IActionResult GetArtistList(string seach)
        {
            try
            {
                List<ArtistDataItem> returnItem = _musicRepo.Artist_GetArtistList(seach) ?? new List<ArtistDataItem>();
                var finalList = returnItem.Select(a =>
                {
                    return new
                    {
                        label = a.artist_name,
                        value = a.id
                    };
                });

                return new JsonResult(finalList);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message.ToString());
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> GetSongsLike([FromBody] SongAlikeRequest value)
        {
            try
            {
                if (value == null)
                {
                    throw new Exception("Invalid Request");
                }

                RadioSessionRecommendedResponse songs = _musicRepo.GetRadioSessionRecommendedSongsFromArtist(value.ArtistName, value.SongName);
                List<ArmaAlbumSongDataItem> tracks = new List<ArmaAlbumSongDataItem>();

                if (songs != null && songs.tracks != null && songs.tracks.Count > 0)
                {
                    int tempId = 0;
                    foreach (var track in songs.tracks)
                    {
                        tempId++;

                        tracks.Add(new ArmaAlbumSongDataItem()
                        {
                            Id = tempId,
                            NameSearch = (track.artists != null && track.artists.Count > 0 ? track.artists[0].name ?? "" : ""),
                            SongTitle = track.name
                        });
                    }
                }

                //RadioSessionSongsResponse songs = _musicRepo.GetRadioSessionSongsFromArtist(value.ArtistName);
                //List<ArmaAlbumSongDataItem> tracks = new List<ArmaAlbumSongDataItem>();

                //if (songs != null && songs.items != null && songs.items.Count > 0)
                //{
                //    int tempId = 0;
                //    foreach (var song in  songs.items)
                //    {
                //        if (song.track != null)
                //        {
                //            tempId++;

                //            tracks.Add(new ArmaAlbumSongDataItem()
                //            {
                //                Id = tempId,
                //                NameSearch = (song.track.artists != null && song.track.artists.Count > 0 ? song.track.artists[0].name ?? "" : ""),
                //                SongTitle = song.track.name
                //            });
                //        }
                //    }
                //}

                return new JsonResult(tracks.Select(tr =>
                {
                    return new
                    {
                        Id = tr.Id,
                        ArtistName = tr.NameSearch,
                        SongName = tr.SongTitle
                    };
                }));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult GeRandomtSongsFromPlaylists()
        {
            try
            {
                ArmaUser currentUser = _authControl.GetCurrentUser();

                if (currentUser == null)
                {
                    throw new Exception("Authentication required");
                }

                List<ArmaRandomSongDataItem> retunItem = _musicRepo.Songs_GetRandomFromPlaylists(currentUser.UserId);

                return new JsonResult(retunItem);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAudioFileDetails(string VideoId)
        {
            try
            {
                var youtube = new YoutubeExplode.YoutubeClient();
                var streamManifest = await youtube.Videos.Streams.GetManifestAsync($"https://www.youtube.com/watch?v={(VideoId ?? "").Trim()}");
                var allStreams = streamManifest.GetAudioOnlyStreams();
                var streamInfo = allStreams.GetWithHighestBitrate();
                var fileType = MimeTypes.GetMimeType($"tmpFileName.{(streamInfo.Container.Name == "webm" ? "weba" : streamInfo.Container.Name)}");

                return new JsonResult(new
                {
                    FileExtension = streamInfo.Container.Name,
                    MimeType = fileType
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message.ToString());
            }
        }

        [HttpGet]
        public async Task<IActionResult> FetchAudioFile(string VideoId)
        {
            try
            {
                var youtube = new YoutubeExplode.YoutubeClient();
                var streamManifest = await youtube.Videos.Streams.GetManifestAsync($"https://www.youtube.com/watch?v={(VideoId ?? "").Trim()}");
                var allStreams = streamManifest.GetAudioOnlyStreams();
                var streamInfo = allStreams.GetWithHighestBitrate();
                var fileType = MimeTypes.GetMimeType($"tmpFileName.{streamInfo.Container.Name}");
                var fileName = $"{VideoId.Trim()}.{streamInfo.Container.Name}";

                string rootPath = _hostEnvironment.WebRootPath.TrimEnd('/').TrimEnd('\\');
                string downloadFolder = (IsLinux ? $"{rootPath}/AudioFiles/" : $"{rootPath}\\AudioFiles\\");
                string endFileName = $"{downloadFolder}{fileName}";

                if (!System.IO.Directory.Exists(downloadFolder))
                {
                    System.IO.Directory.CreateDirectory(downloadFolder);
                }

                if (!System.IO.File.Exists(endFileName))
                {
                    await youtube.Videos.Streams.DownloadAsync(streamInfo, endFileName);
                }

                byte[] fileBytes = System.IO.File.ReadAllBytes(endFileName);

                long size, start, end, length, fp = 0;
                using (var reader = new System.IO.StreamReader(endFileName))
                {
                    size = reader.BaseStream.Length;
                    start = 0;
                    end = size - 1;
                    length = size;

                    // Set the "Accept-Ranges" header to indicate that we support range requests
                    HttpContext.Response.Headers.Add("Accept-Ranges", "0-" + size);

                    // Handle range requests
                    if (!string.IsNullOrEmpty(HttpContext.Request.Headers["Range"]))
                    {
                        long anotherStart = start;
                        long anotherEnd = end;
                        var rangeParts = HttpContext.Request.Headers["Range"].ToString().Split(new char[] { '=' }, 2);
                        var range = rangeParts[1];

                        // Ensure the client hasn't sent a multi-byte range
                        if (range.Contains(","))
                        {
                            HttpContext.Response.Headers.Add("Content-Range", $"bytes {start}-{end}/{size}");
                            return new StatusCodeResult(StatusCodes.Status416RangeNotSatisfiable);
                        }

                        // Parse the range request
                        if (range.StartsWith("-"))
                        {
                            anotherStart = size - long.Parse(range.Substring(1));
                        }
                        else
                        {
                            var rangeBounds = range.Split(new char[] { '-' }, 2);
                            anotherStart = long.Parse(rangeBounds[0]);
                            long temp = 0;
                            anotherEnd = (rangeBounds.Length > 1 && long.TryParse(rangeBounds[1], out temp)) ? long.Parse(rangeBounds[1]) : size;
                        }

                        // Validate the requested range
                        anotherEnd = (anotherEnd > end) ? end : anotherEnd;
                        if (anotherStart > anotherEnd || anotherStart > size - 1 || anotherEnd >= size)
                        {
                            HttpContext.Response.Headers.Add("Content-Range", $"bytes {start}-{end}/{size}");
                            return new StatusCodeResult(StatusCodes.Status416RangeNotSatisfiable);
                        }

                        start = anotherStart;
                        end = anotherEnd;
                        length = end - start + 1;
                        fp = reader.BaseStream.Seek(start, SeekOrigin.Begin);
                        HttpContext.Response.StatusCode = StatusCodes.Status206PartialContent;
                    }
                }

                HttpContext.Response.ContentType = fileType;
                HttpContext.Response.Headers.Add("Cache-Control", "no-cache");
                HttpContext.Response.Headers.Add("Content-Disposition", $"inline; filename=\"{VideoId}.{streamInfo.Container.Name}\"");
                HttpContext.Response.Headers.Add("Content-Range", $"bytes {start}-{end}/{size}");
                HttpContext.Response.Headers.Add("Content-Length", length.ToString());


                System.IO.File.Delete(endFileName);

                var returnItem = new FileContentResult(fileBytes, fileType);

                //Response.RegisterForDispose(fileBytes);

                return returnItem;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message.ToString());
            }
        }

        //[HttpGet]
        //public async Task<IActionResult> FetchAudioFile(string VideoId)
        //{
        //    try
        //    {
        //        var youtube = new YoutubeExplode.YoutubeClient();
        //        var streamManifest = await youtube.Videos.Streams.GetManifestAsync($"https://www.youtube.com/watch?v={(VideoId ?? "").Trim()}");
        //        var allStreams = streamManifest.GetAudioOnlyStreams();
        //        var streamInfo = allStreams.GetWithHighestBitrate();
        //        var fileType = MimeTypes.GetMimeType($"tmpFileName.{streamInfo.Container.Name}");
        //        var fileName = $"{VideoId.Trim()}.{streamInfo.Container.Name}";

        //        string rootPath = _hostEnvironment.WebRootPath.TrimEnd('/').TrimEnd('\\');
        //        string downloadFolder = (IsLinux ? $"{rootPath}/AudioFiles/" : $"{rootPath}\\AudioFiles\\");
        //        string endFileName = $"{downloadFolder}{fileName}";

        //        if (!System.IO.Directory.Exists(downloadFolder))
        //        {
        //            System.IO.Directory.CreateDirectory(downloadFolder);
        //        }

        //        if (!System.IO.File.Exists(endFileName))
        //        {
        //            await youtube.Videos.Streams.DownloadAsync(streamInfo, endFileName);
        //        }

        //        byte[] fileBytes = System.IO.File.ReadAllBytes(endFileName);
        //        long fileLength = fileBytes.Length;

        //        var requestHeaders = Request.Headers;
        //        if (requestHeaders.ContainsKey("Range"))
        //        {
        //            string rangeHeader = requestHeaders["Range"];
        //            long start, end;
        //            if (RangeHelper.TryParseRange(rangeHeader, fileLength, out start, out end))
        //            {
        //                Response.Headers.Add("Content-Range", $"bytes {start}-{end}/{fileLength}");
        //                Response.StatusCode = (int)HttpStatusCode.PartialContent;
        //                return new FileContentResult(fileBytes.AsSpan((int)start, (int)(end - start + 1)).ToArray(), "video/webm");
        //            }
        //        }

        //        HttpContext.Response.StatusCode = StatusCodes.Status206PartialContent;

        //        return new FileContentResult(fileBytes, fileType); //fileName
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, ex.Message.ToString());
        //    }
        //}

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAudioFile(
            string ArtistName,
            string SongName,
            string VideoId
        )
        {
            try
            {
                List<string> fileNameParts = new List<string>();
                string rootPath = _hostEnvironment.WebRootPath.TrimEnd('/').TrimEnd('\\');
                string downloadFolder = (IsLinux ? $"{rootPath}/tempMp3/" : $"{rootPath}\\tempMp3\\");
                string fileHandle = $"{Guid.NewGuid().ToString().ToLower()}.mp3";
                string fileHandleTemp = $"{Guid.NewGuid().ToString().ToLower()}";
                string endFile = $"{downloadFolder}{fileHandle}";
                string endTempFile = $"{downloadFolder}{fileHandleTemp}";

                if (!string.IsNullOrWhiteSpace(ArtistName))
                {
                    fileNameParts.Add(ArtistName.Trim());
                }
                if (!string.IsNullOrWhiteSpace(SongName))
                {
                    fileNameParts.Add(SongName.Trim());
                }

                string fileName = $"{(SanitizeFileName(string.Join(" - ", fileNameParts.ToArray()))).Trim()}";

                if (!System.IO.Directory.Exists(downloadFolder))
                {
                    System.IO.Directory.CreateDirectory(downloadFolder);
                }

                var youtube = new YoutubeExplode.YoutubeClient();
                var streamManifest = await youtube.Videos.Streams.GetManifestAsync($"https://www.youtube.com/watch?v={VideoId}");
                var allStreams = streamManifest.GetAudioOnlyStreams();
                var streamInfo = allStreams.GetWithHighestBitrate();
                fileName = $"{fileName}.{streamInfo.Container.Name}";
                endTempFile = $"{endTempFile}.{streamInfo.Container.Name}";
                fileHandleTemp = $"{fileHandleTemp}.{streamInfo.Container.Name}";

                await youtube.Videos.Streams.DownloadAsync(streamInfo, endTempFile);
                //await DownloadStreamAsync(streamInfo, endTempFile);
                //ConvertToMp3(endTempFile, endFile);

                //System.IO.File.Delete(endTempFile);

                //await youtube.Videos.DownloadAsync($"https://www.youtube.com/watch?v={VideoId}", endFile);

                MemoryStream memoryStream = new MemoryStream();
                using (FileStream fileStream = new FileStream(endTempFile, FileMode.Open, FileAccess.Read))
                {
                    fileStream.CopyTo(memoryStream);
                }

                System.IO.File.Delete(endTempFile);

                memoryStream.Position = 0;
                var fileType = MimeTypes.GetMimeType(fileName);
                var returnItem = new FileStreamResult(memoryStream, fileType)
                {
                    FileDownloadName = fileName
                };

                Response.RegisterForDispose(memoryStream);

                return returnItem;

                //MemoryStream memoryStream = new MemoryStream();

                //using (FileStream fileStream = new FileStream(endFile, FileMode.Open, FileAccess.Read))
                //{
                //    fileStream.CopyTo(memoryStream);
                //}

                //System.IO.File.Delete(endFile);

                //memoryStream.Position = 0;

                //return new FileStreamResult(memoryStream, "audio/mpeg")
                //{
                //    FileDownloadName = fileName,
                //};
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message.ToString());
            }
        }

        //private static void ConvertToMp3(string inputFilePath, string outputFilePath)
        //{
        //    FFMpegArguments
        //        .FromFileInput(inputFilePath)
        //        .OutputToFile(outputFilePath, true, options => options
        //            .WithAudioCodec("libmp3lame")
        //            .WithAudioBitrate(192)
        //        )
        //        .ProcessSynchronously();
        //}

        [HttpGet]
        [Authorize]
        public IActionResult GetUserPlaylists()
        {
            try
            {
                ArmaUser currentUser = _authControl.GetCurrentUser();

                if (currentUser == null)
                {
                    throw new Exception("Invalid request");
                }

                List<ArmaUserPlaylistDataItem> returnItem = _musicRepo.GetUserPlaylists(currentUser.UserId);

                return new JsonResult(returnItem);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message.ToString());
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult LoadUserSelectedPlaylist(int PlaylistId)
        {
            try
            {
                ArmaUser currentUser = _authControl.GetCurrentUser();

                if (currentUser == null)
                {
                    throw new Exception("Invalid request");
                }

                List<ArmaPlaylistDataItem> returnItem = _musicRepo.GetPlaylistById(PlaylistId, currentUser.UserId);
                var finalList = returnItem.Select(sg =>
                {
                    return new
                    {
                        tid = sg.Id,
                        artistName = sg.Artist,
                        songName = sg.Song,
                        videoId = sg.VideoId
                    };
                });

                return new JsonResult(finalList);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message.ToString());
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult DeleteSongFromPlaylist(int SongId)
        {
            try
            {
                ArmaUser currentUser = _authControl.GetCurrentUser();

                if (currentUser == null)
                {
                    throw new Exception("Invalid request");
                }

                _musicRepo.DeleteSongFromPlaylist(SongId, currentUser.UserId);

                return new JsonResult(Ok());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message.ToString());
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult DeleteUserPlaylistAndData(int PlaylistId)
        {
            try
            {
                ArmaUser currentUser = _authControl.GetCurrentUser();

                if (currentUser == null)
                {
                    throw new Exception("Invalid request");
                }

                _musicRepo.DeleteUserPlaylistAndData(PlaylistId, currentUser.UserId);

                return new JsonResult(Ok());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message.ToString());
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddSongToPlaylist([FromBody] MusicAddSongToPlaylistRequest value)
        {
            try
            {
                if (value == null)
                {
                    throw new Exception("Invalid request");
                }
                if (!(!string.IsNullOrWhiteSpace(value.Artist) || !string.IsNullOrWhiteSpace(value.Song)))
                {
                    throw new Exception("Invalid request");
                }

                ArmaUser currentUser = _authControl.GetCurrentUser();

                if (currentUser == null)
                {
                    throw new Exception("Invalid request");
                }

                _musicRepo.AddSongToPlaylist(value.PlaylistId, value.Artist, value.Song, value.VideoId);

                return new JsonResult(Ok());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message.ToString());
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddSongToNewPlaylist([FromBody] MusicAddSongToPlaylistRequest value)
        {
            try
            {
                if (value == null)
                {
                    throw new Exception("Invalid request");
                }
                if (string.IsNullOrWhiteSpace(value.PlaylistName))
                {
                    throw new Exception("'Playlist Name' is required");
                }
                if (!(!string.IsNullOrWhiteSpace(value.Artist) || !string.IsNullOrWhiteSpace(value.Song)))
                {
                    throw new Exception("Invalid request");
                }

                ArmaUser currentUser = _authControl.GetCurrentUser();

                if (currentUser == null)
                {
                    throw new Exception("Invalid request");
                }

                bool playlistExists = _musicRepo.CheckIfPlaylistExists(value.PlaylistName, currentUser.UserId);

                if (playlistExists)
                {
                    throw new Exception($"Playlist '{value.PlaylistName.Trim()}' already exists. Please specify a different name");
                }

                int? playlistId = _musicRepo.InsertPlaylistName(value.PlaylistName.Trim(), currentUser.UserId);

                if (!playlistId.HasValue)
                {
                    throw new Exception("An error occurred creating Playlist");
                }

                _musicRepo.AddSongToPlaylist(playlistId.Value, value.Artist, value.Song, value.VideoId);

                return new JsonResult(Ok());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message.ToString());
            }
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
                YTVideoIdsDataItem videoIds = _musicRepo.Youtube_GetUrlByArtistNameSongName(value.artistName, value.songName);

                if (videoIds != null)
                {
                    string currentVideoId = videoIds.VideoId;

                    return new JsonResult(new
                    {
                        hasVideo = !string.IsNullOrWhiteSpace(currentVideoId),
                        url = $"https://www.youtube.com/watch?v={currentVideoId}",
                        embedUrl = $"https://www.youtube.com/embed/{currentVideoId}?enablejsapi=1", //&autoplay=1
                        videoId = currentVideoId,
                        alternateIds = videoIds.AlternateIds
                    });
                }
                else
                {
                    return new JsonResult(new
                    {
                        hasVideo = false
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message.ToString());
            }
        }

        [HttpGet]
        public IActionResult GeneralSearch(string SearchText)
        {
            try
            {
                List<YTGeneralSearchDataItem> returnItem = _musicRepo.Youtube_PerformGeneralSearch(SearchText);

                return new JsonResult(returnItem);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message.ToString());
            }
        }

        //[Authorize]
        //[AllowAnonymous]
        [HttpPost]
        public IActionResult UploadCustomPlaylist([FromBody] MusicCustomPlaylistRequest value)
        {
            try
            {
                if (value == null || string.IsNullOrWhiteSpace(value.PlayList))
                {
                    throw new Exception("Playlist is required");
                }

                ArmaUser currentUser = _authControl.GetCurrentUser();

                if (currentUser != null && value.CreateNewPlaylist)
                {
                    if (string.IsNullOrWhiteSpace(value.PlaylistName))
                    {
                        throw new Exception("'Playlist Name' is required");
                    }

                    bool playlistExists = _musicRepo.CheckIfPlaylistExists(value.PlaylistName, currentUser.UserId);

                    if (playlistExists)
                    {
                        throw new Exception($"Playlist '{value.PlaylistName.Trim()}' already exists. Please specify a different name");
                    }
                }

                List<TrackDataItem> returnItem = new List<TrackDataItem>();
                List<string> allLines = value.PlayList.Split('\n', StringSplitOptions.RemoveEmptyEntries).ToList();
                int? playlistId = null;

                if (allLines.Count > 0)
                {
                    if (currentUser != null && value.CreateNewPlaylist)
                    {
                        playlistId = _musicRepo.InsertPlaylistName(value.PlaylistName.Trim(), currentUser.UserId);
                    }

                    for (int i = 0; i < allLines.Count; i++)
                    {
                        if (allLines[i].Contains("|"))
                        {
                            List<string> parts = allLines[i].Split('|', StringSplitOptions.RemoveEmptyEntries).ToList();

                            if (parts.Count >= 2)
                            {
                                returnItem.Add(new TrackDataItem()
                                {
                                    tid = i,
                                    artist_name = parts[0].Trim(),
                                    track_name = parts[1].Trim()
                                });

                                if (currentUser != null && value.CreateNewPlaylist && playlistId.HasValue)
                                {
                                    _musicRepo.InsertSongToPlaylist(playlistId.Value, returnItem.Last().artist_name, returnItem.Last().track_name);
                                }
                            }
                        }
                    }
                }

                if (currentUser != null && value.CreateNewPlaylist && playlistId.HasValue)
                {
                    var finalList = _musicRepo.GetPlaylistById(playlistId.Value, currentUser.UserId).Select(sg =>
                    {
                        return new
                        {
                            tid = sg.Id,
                            artistName = sg.Artist,
                            songName = sg.Song,
                            videoId = sg.VideoId
                        };
                    });

                    return new JsonResult(new
                    {
                        playlistId = playlistId.Value,
                        playlistName = value.PlaylistName,
                        songList = finalList
                    });
                }
                else
                {
                    var finalList = returnItem.Select(sg =>
                    {
                        return new
                        {
                            tid = sg.tid,
                            artistName = sg.artist_name,
                            songName = sg.track_name
                        };
                    });

                    return new JsonResult(new
                    {
                        playlistId = (int?)null,
                        playlistName = "",
                        songList = finalList
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message.ToString());
            }
        }

        private class RangeHelper
        {
            public static bool TryParseRange(string rangeHeader, long fileLength, out long start, out long end)
            {
                start = 0;
                end = fileLength - 1;

                if (rangeHeader.StartsWith("bytes="))
                {
                    rangeHeader = rangeHeader.Substring("bytes=".Length);
                    var parts = rangeHeader.Split('-');
                    if (parts.Length == 2 && long.TryParse(parts[0], out start) && long.TryParse(parts[1], out end))
                    {
                        if (start > end)
                        {
                            (start, end) = (end, start);
                        }
                        if (start < 0)
                        {
                            start = 0;
                        }
                        if (end >= fileLength)
                        {
                            end = fileLength - 1;
                        }
                        return true;
                    }
                }
                return false;
            }
        }

        private string Latinize(string Input)
        {
            Encoding latinizeEncoding = Encoding.GetEncoding("ISO-8859-8");
            var strBytes = latinizeEncoding.GetBytes(Input);

            return latinizeEncoding.GetString(strBytes);
        }

        public static string SanitizeFileName(string fileName)
        {
            // Define a regular expression pattern to match harmful characters
            string pattern = @"[/\\?%*:|""<>]";

            // Replace harmful characters with an empty string
            return Regex.Replace(fileName, pattern, "");
        }
    }
}
