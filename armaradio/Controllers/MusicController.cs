using armaradio.Attributes;
using armaradio.Models;
using armaradio.Models.ArmaAuth;
using armaradio.Models.Odysee;
using armaradio.Models.Request;
using armaradio.Models.Response;
using armaradio.Models.Youtube;
using armaradio.Repositories;
using FFMpegCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
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
using YoutubeExplode.Playlists;
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
        private readonly Operations.ArmaUserOperation _operation;
        private readonly ITeraboxUploader _terabox;
        //private IPage _page;
        public MusicController(
            IMusicRepo musicRepo,
            IArmaAuth authControl,
            IWebHostEnvironment hostEnvironment,
            Operations.ArmaUserOperation operation,
            ITeraboxUploader terabox
        //IPage page
        )
        {
            _musicRepo = musicRepo;
            _authControl = authControl;
            _hostEnvironment = hostEnvironment;
            _operation = operation;
            _terabox = terabox;
            //_page = page;

            IsLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentTop100()
        {
            try
            {
                using (_operation)
                {
                    List<TrackDataItem> returnItem = await _musicRepo.GetCurrentTop100();
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
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message.ToString());
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentTop40DanceSingles()
        {
            try
            {
                using (_operation)
                {
                    List<TrackDataItem> returnItem = await _musicRepo.GetCurrentTop40DanceSingles();
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
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message.ToString());
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentTranceTop100()
        {
            try
            {
                using (_operation)
                {
                    List<TrackDataItem> returnItem = await _musicRepo.GetCurrentTranceTop100();
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
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message.ToString());
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentTranceHype100()
        {
            try
            {
                using (_operation)
                {
                    List<TrackDataItem> returnItem = await _musicRepo.GetCurrentTranceHype100();
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
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message.ToString());
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentLatinTop50()
        {
            try
            {
                using (_operation)
                {
                    List<TrackDataItem> returnItem = await _musicRepo.GetCurrentLatinTop50();
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
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message.ToString());
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentTopDanceElectronic()
        {
            try
            {
                using (_operation)
                {
                    List<TrackDataItem> returnItem = await _musicRepo.GetCurrentTopDanceElectronic();
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
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message.ToString());
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentTopRockAlternative()
        {
            try
            {
                using (_operation)
                {
                    List<TrackDataItem> returnItem = await _musicRepo.GetCurrentTopRockAlternative();
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
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message.ToString());
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetTopEmergingArtists()
        {
            try
            {
                using (_operation)
                {
                    List<TrackDataItem> returnItem = await _musicRepo.GetTopEmergingArtists();
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
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message.ToString());
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentTopCountrySongs()
        {
            try
            {
                using (_operation)
                {
                    List<TrackDataItem> returnItem = await _musicRepo.GetCurrentTopCountrySongs();
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
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message.ToString());
            }
        }

        [HttpGet]
        public IActionResult GetTrendingLastFMSongs()
        {
            try
            {
                using (_operation)
                {
                    List<TrackDataItem> returnItem = new List<TrackDataItem>();
                    Guid? requestId = _musicRepo.Tracks_CacheTop100LastFMTrending();

                    if (requestId.HasValue)
                    {
                        returnItem = _musicRepo.Tracks_GetTop100LastFMTrending(requestId.Value);
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

                    return new JsonResult(new List<TrackDataItem>());
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message.ToString());
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentTopRegionalMexicanoSongs()
        {
            try
            {
                using (_operation)
                {
                    List<TrackDataItem> returnItem = await _musicRepo.GetCurrentTopRegionalMexicanoSongs();
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
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message.ToString());
            }
        }

        [HttpGet]
        public IActionResult GetTopRankedArtists5stars()
        {
            try
            {
                using (_operation)
                {
                    List<TrackDataItem> returnItem = _musicRepo.GetTopUserRankedArtists5stars();
                    var finalList = returnItem.Select(sg =>
                    {
                        return new
                        {
                            tid = sg.tid,
                            artistName = sg.artist_name,
                            songName = sg.track_name,
                            showAlbumsButton = true
                        };
                    });

                    return new JsonResult(finalList);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message.ToString());
            }
        }

        [HttpGet]
        public IActionResult GetTopRankedArtists4stars()
        {
            try
            {
                using (_operation)
                {
                    List<TrackDataItem> returnItem = _musicRepo.GetTopUserRankedArtists4stars();
                    var finalList = returnItem.Select(sg =>
                    {
                        return new
                        {
                            tid = sg.tid,
                            artistName = sg.artist_name,
                            songName = sg.track_name,
                            showAlbumsButton = true
                        };
                    });

                    return new JsonResult(finalList);
                }
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
                _operation.SetRequestBody(value);
                using (_operation)
                {
                    List<ArmaArtistDataItem> returnItem = _musicRepo.Artist_FindArtists(value.SearchPhrase) ?? new List<ArmaArtistDataItem>();

                    return new JsonResult(returnItem);
                }
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
                _operation.SetRequestBody(value);
                using (_operation)
                {
                    ArmaArtistAlbumsResponse returnItem = _musicRepo.Albums_GetArtistsAlbums(value.ArtistId);

                    var acceptEncoding = Request.Headers["Accept-Encoding"];

                    if (acceptEncoding.ToString().Contains("gzip"))
                    {
                        Response.Headers.Add("Content-Encoding", "gzip");
                        var serializerSettings = new Newtonsoft.Json.JsonSerializerSettings
                        {
                            ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
                        };

                        var gzipData = _musicRepo.CompressJsonString(Newtonsoft.Json.JsonConvert.SerializeObject(returnItem, serializerSettings));

                        return new FileContentResult(gzipData, "application/gzip");
                    }
                    else
                    {
                        return new JsonResult(returnItem);
                    }
                }
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
                _operation.SetRequestBody(value);
                using (_operation)
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
                _operation.SetRequestBody(new
                {
                    search = seach
                });
                using (_operation)
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
                _operation.SetRequestBody(value);
                using (_operation)
                {
                    if (value == null)
                    {
                        throw new Exception("Invalid Request");
                    }

                    List<ArmaRecommendationDataItem> songs = _musicRepo.GetRadioSessionRecommendedSongsFromArtist(value.ArtistName, value.SongName);
                    List<ArmaAlbumSongDataItem> tracks = new List<ArmaAlbumSongDataItem>();

                    if (songs != null && songs.Count > 0)
                    {
                        int tempId = 0;
                        foreach (var track in songs)
                        {
                            tempId++;

                            tracks.Add(new ArmaAlbumSongDataItem()
                            {
                                Id = tempId,
                                NameSearch = track.artist_name,
                                SongTitle = track.song_name
                            });
                        }
                    }

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
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> GetAISongSuggestions([FromBody] SongAlikeRequest value)
        {
            try
            {
                _operation.SetRequestBody(value);
                using (_operation)
                {
                    if (value == null)
                    {
                        throw new Exception("Invalid Request");
                    }
                    if (string.IsNullOrWhiteSpace(value.ArtistName) && string.IsNullOrWhiteSpace(value.SongName))
                    {
                        throw new Exception("Artist or a Song Name is required");
                    }

                    ArmaAISongsResponse returnItem = await _musicRepo.GetAIRecommendedSongs(value.ArtistName, value.SongName);

                    if (returnItem == null || returnItem.Rerun)
                    {
                        throw new Exception("An error occurred. Please try again");
                    }

                    return new JsonResult(returnItem);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult GeRandomSongsFromPlaylists()
        {
            try
            {
                using (_operation)
                {
                    ArmaUser currentUser = _authControl.GetCurrentUser();

                    if (currentUser == null)
                    {
                        throw new Exception("Authentication required");
                    }

                    List<ArmaRandomSongDataItem> retunItem = _musicRepo.Songs_GetRandomFromPlaylists(currentUser.UserId);

                    return new JsonResult(retunItem);
                }
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
                _operation.SetRequestBody(new
                {
                    VideoId = VideoId
                });
                using (_operation)
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
                _operation.SetRequestBody(new
                {
                    VideoId = VideoId
                });
                using (_operation)
                {
                    //List<AdaptiveFormatDataItem> audioStreams = _musicRepo.GetAudioStreams(VideoId);
                    string rootPath = _hostEnvironment.WebRootPath.TrimEnd('/').TrimEnd('\\');
                    string downloadFolder = (IsLinux ? $"{rootPath}/AudioFiles/" : $"{rootPath}\\AudioFiles\\");
                    string endFileName = endFileName = $"{downloadFolder}{VideoId.Trim()}.m4a";
                    string fileType = "audio/mp4"; // MimeTypes.GetMimeType($"tmpFileName.m4a");
                    string containerName = "m4a";
                    bool fromConvertedFile = false;

                    if (!System.IO.Directory.Exists(downloadFolder))
                    {
                        System.IO.Directory.CreateDirectory(downloadFolder);
                    }

                    if (!System.IO.File.Exists(endFileName))
                    {
                        await _musicRepo.DownloadMp4File($"https://www.youtube.com/watch?v={(VideoId ?? "").Trim()}", endFileName);

                        //long fileSize = new FileInfo(endFileName).Length;
                        //DateTime localMtime = DateTime.Now;

                        //try
                        //{
                        //    var preCreateResponse = await _terabox.PreCreateUpload(endFileName, fileSize, localMtime);
                        //    Console.WriteLine($"Pre-create successful. Upload ID: {preCreateResponse.Uploadid}");

                        //    string uploadResult = await _terabox.UploadFile(endFileName, preCreateResponse);
                        //    Console.WriteLine($"Upload result: {uploadResult}");
                        //}
                        //catch (Exception ex)
                        //{
                        //    Console.WriteLine($"Error: {ex.Message}");
                        //}

                        //AdaptiveFormatDataItem maxBitrateStream = audioStreams.OrderByDescending(br => br.bitrate).FirstOrDefault();
                        //var backupStreamInfo = maxBitrateStream;

                        //if (!(maxBitrateStream.containerName.ToLower() == "m4a" || maxBitrateStream.containerName.ToLower() == "mp4"))
                        //{
                        //    maxBitrateStream = audioStreams
                        //        .Where(s => s.containerName.ToLower() == "mp4")
                        //        .OrderByDescending(s => s.bitrate)
                        //        .FirstOrDefault();

                        //    if (maxBitrateStream != null)
                        //    {
                        //        backupStreamInfo = maxBitrateStream;
                        //    }
                        //    else
                        //    {
                        //        fromConvertedFile = true;
                        //        //endFileName = $"{downloadFolder}{VideoId.Trim()}.mp4";

                        //        ConvertToM4A(backupStreamInfo.streamUrl, endFileName);
                        //    }
                        //}

                        //if (!fromConvertedFile)
                        //{
                        //    var fileName = $"{VideoId.Trim()}.{backupStreamInfo.containerName}";

                        //    //endFileName = $"{downloadFolder}{fileName}";

                        //    if (!System.IO.File.Exists(endFileName))
                        //    {
                        //        _musicRepo.DownloadMp4File(backupStreamInfo.streamUrl, endFileName);
                        //    }
                        //}





                        //var youtube = new YoutubeExplode.YoutubeClient();
                        //var streamManifest = await youtube.Videos.Streams.GetManifestAsync($"https://www.youtube.com/watch?v={(VideoId ?? "").Trim()}");
                        //var allStreams = streamManifest.GetAudioOnlyStreams();

                        //var streamInfo = allStreams.GetWithHighestBitrate();
                        //var backupStreamInfo = streamInfo;

                        //if (!(streamInfo.Container.Name.ToLower() == "m4a" || streamInfo.Container.Name.ToLower() == "mp4"))
                        //{
                        //    streamInfo = streamManifest.GetAudioOnlyStreams()
                        //        .Where(s => s.Container == Container.Mp4)
                        //        .OrderByDescending(s => s.Bitrate)
                        //        .FirstOrDefault();

                        //    if (streamInfo != null)
                        //    {
                        //        backupStreamInfo = streamInfo;
                        //    }
                        //    else
                        //    {
                        //        fromConvertedFile = true;
                        //        //endFileName = $"{downloadFolder}{VideoId.Trim()}.mp4";

                        //        ConvertToM4A(backupStreamInfo.Url, endFileName);
                        //    }
                        //}

                        //if (!fromConvertedFile)
                        //{
                        //    var fileName = $"{VideoId.Trim()}.{backupStreamInfo.Container.Name}";

                        //    //endFileName = $"{downloadFolder}{fileName}";

                        //    if (!System.IO.File.Exists(endFileName))
                        //    {
                        //        await youtube.Videos.Streams.DownloadAsync(backupStreamInfo, endFileName);
                        //    }
                        //}
                    }

                    //byte[] fileBytes = System.IO.File.ReadAllBytes(endFileName);

                    //long size, start, end, length, fp = 0;
                    //using (var reader = new System.IO.StreamReader(endFileName))
                    //{
                    //    size = reader.BaseStream.Length;
                    //    start = 0;
                    //    end = size - 1;
                    //    length = size;

                    //    // Set the "Accept-Ranges" header to indicate that we support range requests
                    //    HttpContext.Response.Headers.Append("Accept-Ranges", "0-" + size);

                    //    // Handle range requests
                    //    if (!string.IsNullOrEmpty(HttpContext.Request.Headers["Range"]))
                    //    {
                    //        long anotherStart = start;
                    //        long anotherEnd = end;
                    //        var rangeParts = HttpContext.Request.Headers["Range"].ToString().Split(new char[] { '=' }, 2);
                    //        var range = rangeParts[1];

                    //        // Ensure the client hasn't sent a multi-byte range
                    //        if (range.Contains(","))
                    //        {
                    //            HttpContext.Response.Headers.Append("Content-Range", $"bytes {start}-{end}/{size}");
                    //            return new StatusCodeResult(StatusCodes.Status416RangeNotSatisfiable);
                    //        }

                    //        // Parse the range request
                    //        if (range.StartsWith("-"))
                    //        {
                    //            anotherStart = size - long.Parse(range.Substring(1));
                    //        }
                    //        else
                    //        {
                    //            var rangeBounds = range.Split(new char[] { '-' }, 2);
                    //            anotherStart = long.Parse(rangeBounds[0]);
                    //            long temp = 0;
                    //            anotherEnd = (rangeBounds.Length > 1 && long.TryParse(rangeBounds[1], out temp)) ? long.Parse(rangeBounds[1]) : size;
                    //        }

                    //        // Validate the requested range
                    //        anotherEnd = (anotherEnd > end) ? end : anotherEnd;
                    //        if (anotherStart > anotherEnd || anotherStart > size - 1 || anotherEnd >= size)
                    //        {
                    //            HttpContext.Response.Headers.Append("Content-Range", $"bytes {start}-{end}/{size}");
                    //            return new StatusCodeResult(StatusCodes.Status416RangeNotSatisfiable);
                    //        }

                    //        start = anotherStart;
                    //        end = anotherEnd;
                    //        length = end - start + 1;
                    //        fp = reader.BaseStream.Seek(start, SeekOrigin.Begin);
                    //        HttpContext.Response.StatusCode = StatusCodes.Status206PartialContent;
                    //    }
                    //}

                    //HttpContext.Response.ContentType = fileType;
                    //HttpContext.Response.Headers.Append("Cache-Control", "no-cache");
                    //HttpContext.Response.Headers.Append("Content-Disposition", $"inline; filename=\"{VideoId}.{containerName}\"");
                    //HttpContext.Response.Headers.Append("Content-Range", $"bytes {start}-{end}/{size}");
                    //HttpContext.Response.Headers.Append("Content-Length", length.ToString());


                    ////System.IO.File.Delete(endFileName);

                    //Task.Delay(TimeSpan.FromHours(1))
                    //    .ContinueWith(_ => {
                    //        if (System.IO.File.Exists(endFileName))
                    //        {
                    //            System.IO.File.Delete(endFileName);
                    //        }
                    //    });

                    //var returnItem = new FileContentResult(fileBytes, fileType);

                    bool fileWasFlaggedForDeletion = false;
                    var fileInfo = new FileInfo(endFileName);
                    long fileLength = fileInfo.Length;

                    // Handle range requests
                    var rangeHeader = Request.Headers["Range"].ToString();
                    if (string.IsNullOrEmpty(rangeHeader))
                    {
                        // No range requested, return full file
                        Response.Headers.Append("Accept-Ranges", "bytes");
                        Response.ContentType = fileType;
                        Response.Headers.Append("Content-Disposition", $"inline; filename=\"{VideoId}.{containerName}\"");

                        _musicRepo.FlagFileForDeletion(endFileName);

                        fileWasFlaggedForDeletion = true;

                        return PhysicalFile(endFileName, fileType, enableRangeProcessing: true);
                    }

                    // Parse range
                    var rangeValue = rangeHeader.Replace("bytes=", string.Empty);
                    var rangeParts = rangeValue.Split('-');
                    var rangeStart = long.Parse(rangeParts[0]);
                    var rangeEnd = rangeParts.Length > 1 && long.TryParse(rangeParts[1], out var temp)
                        ? Math.Min(temp, fileLength - 1)
                        : fileLength - 1;

                    // Validate range
                    if (rangeStart > rangeEnd || rangeStart > fileLength - 1 || rangeEnd >= fileLength)
                    {
                        Response.Headers.Append("Content-Range", $"bytes */{fileLength}");
                        return StatusCode(StatusCodes.Status416RangeNotSatisfiable);
                    }

                    Response.StatusCode = StatusCodes.Status206PartialContent;
                    Response.Headers.Append("Content-Range", $"bytes {rangeStart}-{rangeEnd}/{fileLength}");
                    Response.Headers.Append("Accept-Ranges", "bytes");
                    Response.Headers.Append("Content-Type", fileType);
                    Response.Headers.Append("Content-Disposition", $"inline; filename=\"{VideoId}.{containerName}\"");

                    if (!fileWasFlaggedForDeletion)
                    {
                        _musicRepo.FlagFileForDeletion(endFileName);
                    }

                    return new FileStreamResult(new FileStream(endFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite), fileType)
                    {
                        EnableRangeProcessing = true
                    };
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message.ToString());
            }
        }

        [HttpGet]
        public async Task<IActionResult> BufferAudioFile(string VideoId)
        {
            try
            {
                _operation.SetRequestBody(new
                {
                    VideoId = VideoId
                });
                using (_operation)
                {
                    //List<AdaptiveFormatDataItem> audioStreams = _musicRepo.GetAudioStreams(VideoId);
                    string rootPath = _hostEnvironment.WebRootPath.TrimEnd('/').TrimEnd('\\');
                    string downloadFolder = (IsLinux ? $"{rootPath}/AudioFiles/" : $"{rootPath}\\AudioFiles\\");
                    string endFileName = endFileName = $"{downloadFolder}{VideoId.Trim()}.m4a";
                    string fileType = "audio/mp4"; // MimeTypes.GetMimeType($"tmpFileName.m4a");
                    string containerName = "m4a";
                    bool fromConvertedFile = false;

                    if (!System.IO.Directory.Exists(downloadFolder))
                    {
                        System.IO.Directory.CreateDirectory(downloadFolder);
                    }

                    if (!System.IO.File.Exists(endFileName))
                    {
                        await _musicRepo.DownloadMp4File($"https://www.youtube.com/watch?v={(VideoId ?? "").Trim()}", endFileName);
                    }

                    bool fileWasFlaggedForDeletion = false;
                    var fileInfo = new FileInfo(endFileName);
                    long fileLength = fileInfo.Length;

                    // Handle range requests
                    var rangeHeader = Request.Headers["Range"].ToString();
                    if (string.IsNullOrEmpty(rangeHeader))
                    {
                        // No range requested, return full file
                        Response.Headers.Append("Accept-Ranges", "bytes");
                        Response.ContentType = fileType;
                        Response.Headers.Append("Content-Disposition", $"inline; filename=\"{VideoId}.{containerName}\"");

                        _musicRepo.FlagFileForDeletion(endFileName);

                        fileWasFlaggedForDeletion = true;

                        return PhysicalFile(endFileName, fileType, enableRangeProcessing: true);
                    }

                    // Parse range
                    var rangeValue = rangeHeader.Replace("bytes=", string.Empty);
                    var rangeParts = rangeValue.Split('-');
                    var rangeStart = long.Parse(rangeParts[0]);
                    var rangeEnd = rangeParts.Length > 1 && long.TryParse(rangeParts[1], out var temp)
                        ? Math.Min(temp, fileLength - 1)
                        : fileLength - 1;

                    // Validate range
                    if (rangeStart > rangeEnd || rangeStart > fileLength - 1 || rangeEnd >= fileLength)
                    {
                        Response.Headers.Append("Content-Range", $"bytes */{fileLength}");
                        return StatusCode(StatusCodes.Status416RangeNotSatisfiable);
                    }

                    Response.StatusCode = StatusCodes.Status206PartialContent;
                    Response.Headers.Append("Content-Range", $"bytes {rangeStart}-{rangeEnd}/{fileLength}");
                    Response.Headers.Append("Accept-Ranges", "bytes");
                    Response.Headers.Append("Content-Type", fileType);
                    Response.Headers.Append("Content-Disposition", $"inline; filename=\"{VideoId}.{containerName}\"");

                    if (!fileWasFlaggedForDeletion)
                    {
                        _musicRepo.FlagFileForDeletion(endFileName);
                    }

                    return new FileStreamResult(new FileStream(endFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite), fileType)
                    {
                        EnableRangeProcessing = true
                    };
                }
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
                _operation.SetRequestBody(new
                {
                    ArtistName = ArtistName,
                    SongName = SongName,
                    VideoId = VideoId
                });
                using (_operation)
                {
                    string rootPath = _hostEnvironment.WebRootPath.TrimEnd('/').TrimEnd('\\');
                    string downloadFolder = (IsLinux ? $"{rootPath}/AudioFiles/" : $"{rootPath}\\AudioFiles\\");
                    string endFileName = "";
                    string fileType = "audio/mp4"; // MimeTypes.GetMimeType($"tmpFileName.mp4");
                    string fileHandle = $"{Guid.NewGuid().ToString().ToLower()}.m4a";
                    string fileHandleTemp = $"{Guid.NewGuid().ToString().ToLower()}";
                    string endTempExistingFile = (!string.IsNullOrWhiteSpace(VideoId) ? $"{downloadFolder}{VideoId}.m4a" : "");
                    string endTempFile = (!string.IsNullOrWhiteSpace(endTempExistingFile) ? endTempExistingFile : $"{downloadFolder}{fileHandleTemp}.m4a");
                    string fileName = "";
                    bool fromConvertedFile = false;

                    List<string> fileNameParts = new List<string>();

                    if (!string.IsNullOrWhiteSpace(ArtistName))
                    {
                        fileNameParts.Add(ArtistName.Trim());
                    }
                    if (!string.IsNullOrWhiteSpace(SongName))
                    {
                        fileNameParts.Add(SongName.Trim());
                    }
                    if (string.IsNullOrWhiteSpace(ArtistName) && string.IsNullOrWhiteSpace(SongName) && !string.IsNullOrWhiteSpace(VideoId))
                    {
                        fileNameParts.Add(VideoId.Trim());
                    }

                    string downloadFileName = $"{(Latinize(SanitizeFileName(string.Join(" - ", fileNameParts.ToArray())))).Trim()}";

                    if (!System.IO.File.Exists(endTempFile))
                    {
                        if (!System.IO.Directory.Exists(downloadFolder))
                        {
                            System.IO.Directory.CreateDirectory(downloadFolder);
                        }

                        await _musicRepo.DownloadMp4File($"https://www.youtube.com/watch?v={(VideoId ?? "").Trim()}", endTempFile);

                        //List<string> fileNameParts = new List<string>();
                        //string endFile = $"{downloadFolder}{fileHandle}";

                        //if (!string.IsNullOrWhiteSpace(ArtistName))
                        //{
                        //    fileNameParts.Add(ArtistName.Trim());
                        //}
                        //if (!string.IsNullOrWhiteSpace(SongName))
                        //{
                        //    fileNameParts.Add(SongName.Trim());
                        //}

                        //fileName = $"{(SanitizeFileName(string.Join(" - ", fileNameParts.ToArray()))).Trim()}";

                        //var youtube = new YoutubeExplode.YoutubeClient();
                        //var streamManifest = await youtube.Videos.Streams.GetManifestAsync($"https://www.youtube.com/watch?v={VideoId}");
                        //var allStreams = streamManifest.GetAudioOnlyStreams();

                        //var streamInfo = allStreams.GetWithHighestBitrate();
                        //var backupStreamInfo = streamInfo;

                        //if (!(streamInfo.Container.Name.ToLower() == "m4a" || streamInfo.Container.Name.ToLower() == "mp4"))
                        //{
                        //    streamInfo = streamManifest.GetAudioOnlyStreams()
                        //        .Where(s => s.Container == Container.Mp4)
                        //        .OrderByDescending(s => s.Bitrate)
                        //        .FirstOrDefault();

                        //    if (streamInfo != null)
                        //    {
                        //        backupStreamInfo = streamInfo;
                        //    }
                        //    else
                        //    {
                        //        fromConvertedFile = true;
                        //        fileName = $"{fileName}.mp4";
                        //        //endTempFile = $"{endTempFile}.{backupStreamInfo.Container.Name}";
                        //        fileHandleTemp = $"{fileHandleTemp}.{backupStreamInfo.Container.Name}";

                        //        ConvertToM4A(backupStreamInfo.Url, endTempFile);
                        //    }
                        //}

                        //if (!fromConvertedFile)
                        //{
                        //    fileName = $"{fileName}.{backupStreamInfo.Container.Name}";
                        //    //endTempFile = $"{endTempFile}.{backupStreamInfo.Container.Name}";
                        //    fileHandleTemp = $"{fileHandleTemp}.{backupStreamInfo.Container.Name}";

                        //    if (!System.IO.File.Exists(endTempFile))
                        //    {
                        //        await youtube.Videos.Streams.DownloadAsync(streamInfo, endTempFile);
                        //    }
                        //}
                    }

                    //await DownloadStreamAsync(streamInfo, endTempFile);
                    //ConvertToMp3(endTempFile, endFile);

                    //System.IO.File.Delete(endTempFile);

                    //await youtube.Videos.DownloadAsync($"https://www.youtube.com/watch?v={VideoId}", endFile);

                    Response.Headers.Append("Content-Disposition", $"inline; filename=\"{downloadFileName}.m4a\"");

                    _musicRepo.FlagFileForDeletion(endTempFile);

                    return PhysicalFile(endTempFile, fileType, enableRangeProcessing: false);

                    //MemoryStream memoryStream = new MemoryStream();
                    //using (FileStream fileStream = new FileStream(endTempFile, FileMode.Open, FileAccess.Read))
                    //{
                    //    fileStream.CopyTo(memoryStream);
                    //}

                    //FlagFileForDeletion(endTempFile);

                    //memoryStream.Position = 0;
                    //var returnItem = new FileStreamResult(memoryStream, fileType)
                    //{
                    //    FileDownloadName = fileName
                    //};

                    //Response.RegisterForDispose(memoryStream);

                    //return returnItem;
                }
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

        private static void ConvertToM4A(string inputUrl, string outputFilePath)
        {
            FFMpegArguments
                .FromUrlInput(new Uri(inputUrl))
                //.FromFileInput(inputFilePath)
                .OutputToFile(outputFilePath, true, options => options
                    .WithAudioCodec("aac")
                    .WithAudioBitrate(192)
                    .WithCustomArgument("-movflags faststart")
                    .WithCustomArgument("-strict -2")
                )
                .ProcessSynchronously();
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetUserPlaylists()
        {
            try
            {
                using (_operation)
                {
                    ArmaUser currentUser = _authControl.GetCurrentUser();

                    if (currentUser == null)
                    {
                        throw new Exception("Invalid request");
                    }

                    List<ArmaUserPlaylistDataItem> returnItem = _musicRepo.GetUserPlaylists(currentUser.UserId);

                    return new JsonResult(returnItem);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message.ToString());
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetSharedPlaylistToken(int PlaylistId)
        {
            try
            {
                _operation.SetRequestBody(new
                {
                    PlaylistId = PlaylistId
                });
                using (_operation)
                {
                    ArmaUser currentUser = _authControl.GetCurrentUser();

                    if (currentUser == null)
                    {
                        throw new Exception("Invalid request");
                    }

                    string returnItem = _musicRepo.GetSharedPlaylistToken(PlaylistId, currentUser.UserId);

                    return new JsonResult(new
                    {
                        token = returnItem
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message.ToString());
            }
        }

        [HttpGet]
        public IActionResult GetSharedPlaylist(string token)
        {
            try
            {
                _operation.SetRequestBody(new
                {
                    token = token
                });
                using (_operation)
                {
                    ArmaUser currentUser = _authControl.GetCurrentUser();

                    if (string.IsNullOrWhiteSpace(token))
                    {
                        return new JsonResult(null);
                    }

                    ArmaSharedPlaylistDataItem returnItem = _musicRepo.GetSharedPlaylist(token);

                    if (returnItem == null || returnItem.PlaylistData == null || returnItem.PlaylistData.Count == 0)
                    {
                        return new JsonResult(null);
                    }

                    var finalList = new
                    {
                        playlistId = -1,
                        playlistName = returnItem.PlaylistName,
                        playlistData = returnItem.PlaylistData.Select(sg =>
                        {
                            return new
                            {
                                tid = sg.Id,
                                artistName = sg.Artist,
                                songName = sg.Song,
                                videoId = sg.VideoId
                            };
                        })
                    };

                    return new JsonResult(finalList);
                }
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
                _operation.SetRequestBody(new
                {
                    PlaylistId = PlaylistId
                });
                using (_operation)
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
                _operation.SetRequestBody(new
                {
                    SongId = SongId
                });
                using (_operation)
                {
                    ArmaUser currentUser = _authControl.GetCurrentUser();

                    if (currentUser == null)
                    {
                        throw new Exception("Invalid request");
                    }

                    _musicRepo.DeleteSongFromPlaylist(SongId, currentUser.UserId);

                    return new JsonResult(Ok());
                }
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
                _operation.SetRequestBody(new
                {
                    PlaylistId = PlaylistId
                });
                using (_operation)
                {
                    ArmaUser currentUser = _authControl.GetCurrentUser();

                    if (currentUser == null)
                    {
                        throw new Exception("Invalid request");
                    }

                    _musicRepo.DeleteUserPlaylistAndData(PlaylistId, currentUser.UserId);

                    return new JsonResult(Ok());
                }
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
                _operation.SetRequestBody(value);
                using (_operation)
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
                _operation.SetRequestBody(value);
                using (_operation)
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
                using (_operation)
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
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message.ToString());
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetUrlByArtistSongName([FromBody] MusicUrlByArtistSongRequest value)
        {
            try
            {
                _operation.SetRequestBody(value);
                using (_operation)
                {
                    YTVideoIdsDataItem videoIds = await _musicRepo.Youtube_GetUrlByArtistNameSongName(value.artistName, value.songName);

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
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message.ToString());
            }
        }

        [HttpGet]
        public async Task<IActionResult> GeneralSearch(string SearchText)
        {
            try
            {
                _operation.SetRequestBody(new
                {
                    SearchText = SearchText
                });
                using (_operation)
                {
                    List<YTGeneralSearchDataItem> returnItem = await _musicRepo.Youtube_PerformGeneralSearch(SearchText);
                    //List<OdyseeSearchResult> odyseeResults = await _musicRepo.SearchOdyseeAsync(SearchText);

                    return new JsonResult(returnItem);
                }
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
                _operation.SetRequestBody(value);
                using (_operation)
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
