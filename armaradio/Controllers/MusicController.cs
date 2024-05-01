using armaradio.Models;
using armaradio.Models.ArmaAuth;
using armaradio.Models.Request;
using armaradio.Models.Response;
using armaradio.Models.Youtube;
using armaradio.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp;
using System.Collections.Generic;

namespace armaradio.Controllers
{
    public class MusicController : Controller
    {
        private readonly IMusicRepo _musicRepo;
        private readonly IArmaAuth _authControl;
        //private IPage _page;
        public MusicController(
            IMusicRepo musicRepo,
            IArmaAuth authControl
            //IPage page
        )
        {
            _musicRepo = musicRepo;
            _authControl = authControl;
            //_page = page;
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

                RadioSessionSongsResponse songs = _musicRepo.GetRadioSessionSongsFromArtist(value.ArtistName);

                //var response = await _page.EvaluateFunctionAsync(@"(artistName, artistId, songName) => { 
                //    return new Promise((resolve, reject) => {
                //        let fetchBody = {
                //            method: ""POST"", // *GET, POST, PUT, DELETE, etc.
                //            mode: ""cors"", // no-cors, *cors, same-origin
                //            cache: ""no-cache"", // *default, no-cache, reload, force-cache, only-if-cached
                //            credentials: ""same-origin"", // include, *same-origin, omit
                //            headers: {
                //                ""Content-Type"": ""application/json"",
                //                ""Accept-Encoding"": ""gzip, deflate""
                //                // 'Content-Type': 'application/x-www-form-urlencoded',
                //            },
                //            redirect: ""follow"", // manual, *follow, error
                //            referrerPolicy: ""no-referrer"", // no-referrer, *no-referrer-when-downgrade, origin, origin-when-cross-origin, same-origin, strict-origin, strict-origin-when-cross-origin, unsafe-url
                //            body: JSON.stringify({
                //                artist: artistName,
                //                mbid: artistId,
                //                track: songName
                //            })
                //        };

                //        fetch(""https://api.spotalike.com/v1/playlists"", fetchBody)
                //            .then(function (response) {
                //                if (response.status >= 500) {
                //                    resolve(response.text());
                //                } else {
                //                    return response.json().catch(function () {
                //                        resolve(""{}"");
                //                    });
                //                }
                //            })
                //            .then(function (jsonData) {
                //                resolve(JSON.stringify(jsonData));
                //            })
                //            .catch(function (error) {
                //                resolve(error.message);
                //            });
                //    });

                //}", (value.ArtistName ?? ""), (value.MBId ?? ""), (value.SongName ?? ""));

                //while (response == null)
                //{
                //    Thread.Sleep(5);
                //}

                //SongsAlikeResponse responseItem = Newtonsoft.Json.JsonConvert.DeserializeObject<SongsAlikeResponse>(response.ToObject<string>());

                //return new JsonResult(responseItem);

                return new JsonResult(Ok());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

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
    }
}
