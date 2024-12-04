using armaoffline.Data;
using armaoffline.Models;
using armaoffline.Services;
using Dapper;
using Microsoft.JSInterop;
using Microsoft.Maui.Storage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace armaoffline.Repositories
{
    public class ArmaApi : IArmaApi
    {
        public static GlobalState _globalState;
        private readonly IDapperHelper _dapper;
        public ArmaApi(
            GlobalState globalState,
            IDapperHelper dapper
        )
        {
            _globalState = globalState;
            _dapper = dapper;
        }

        public bool Signin(string Email, string Password)
        {
            bool nowSignedIn = false;

            using (HttpClient client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(new
                {
                    UserName = Email,
                    Password = Password
                });

                string endPoing = "https://armarad.com/Api"; // $"{(Debugger.IsAttached ? "https://localhost:7001/Api" : "https://armarad.com/Api")}";
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = client.PostAsync($"{endPoing}/ExternalLogin", content).Result;

                if (response != null && response.IsSuccessStatusCode)
                {
                    var responseString = response.Content.ReadAsStringAsync().Result;
                    ApiSigninResponse returnItem = JsonConvert.DeserializeObject<ApiSigninResponse>(responseString);

                    if (returnItem != null && !string.IsNullOrWhiteSpace(returnItem.apiToken))
                    {
                        _globalState.appToken = returnItem.apiToken;

                        int userid = _dapper.GetFirstOrDefault<int>(@"
                            with existinguser as (
                                select 1 from armausers where username=@username
                            )
                            insert into armausers
                            (
                                username
                            )
                            select
                                @username
                            where not exists (
                                select 1 from existinguser
                            );

                            select
                                id
                            from armausers
                            where
                                username=@username;
                        ", new
                        {
                            username = (Email ?? "").Trim().ToLower()
                        });

                        _globalState.localUserId = userid;

                        RepopulateUserPlaylistAndSongs();

                        nowSignedIn = true;
                    }
                }
            }

            return nowSignedIn;
        }

        public void RepopulateUserPlaylistAndSongs()
        {
            using (var conn = _dapper.GetConnection())
            {
                _dapper.ExecuteNonQuery(conn, @"
                    delete from playlists
                    where
                        userid=@userid;

                    delete from usersongs
                    where
                        userid=@userid;
                    ", new
                    {
                        userid = _globalState.localUserId
                    });

                List<ArmaUserPlaylistDataItem> playlists = GetUserPlaylists() ?? new List<ArmaUserPlaylistDataItem>();

                foreach (var playlist in playlists)
                {
                    List<ArmaPlaylistDataItem> songs = GetPlaylistById(playlist.Id) ?? new List<ArmaPlaylistDataItem>();

                    int localPlaylistId = _dapper.GetFirstOrDefault<int>(conn, @"
                        insert into playlists
                        (
                            playlistid,
                            userid,
                            playlistname
                        )
                        values
                        (
                            @playlistid,
                            @userid,
                            @playlistname
                        );

                        select
                            id
                        from playlists
                        where
                            playlistid=@playlistid;
                        ", new
                        {
                            playlistid = playlist.Id,
                            userid = _globalState.localUserId,
                            playlistname = (playlist.PlaylistName ?? "")
                        });

                    bool allSongsAvailableOffLine = true;

                    foreach (var song in songs.Where(sg => !string.IsNullOrWhiteSpace(sg.VideoId)).ToList())
                    {
                        _dapper.ExecuteNonQuery(conn, @"
                            insert into usersongs
                            (
                                songid,
                                playlistid,
                                userid,
                                videoid,
                                songname,
                                artist
                            )
                            values
                            (
                                @songid,
                                @playlistid,
                                @userid,
                                @videoid,
                                @songname,
                                @artist
                            );
                        ", new
                            {
                                songid = song.Id,
                                playlistid = localPlaylistId,
                                userid = _globalState.localUserId,
                                videoid = song.VideoId,
                                songname = song.Song,
                                artist = song.Artist
                            });

                        if (!CheckIfAudioFileExists($"{song.VideoId}.m4a"))
                        {
                            allSongsAvailableOffLine = false;
                        }
                    }

                    if (allSongsAvailableOffLine)
                    {
                        MarkPlaylistSongsAsDownloaded(playlist.Id);
                    }
                }
            }
        }

        public List<ArmaUserPlaylistDataItem> GetUserPlaylists()
        {
            List<ArmaUserPlaylistDataItem> returnItem = new List<ArmaUserPlaylistDataItem>();

            using (HttpClient client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(new
                {
                });

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpUtility.UrlEncode(_globalState.appToken));

                string endPoing = "https://armarad.com/Api"; // $"{(Debugger.IsAttached ? "https://localhost:7001/Api" : "https://armarad.com/Api")}";
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = client.PostAsync($"{endPoing}/GetUserPlaylists", content).Result;

                if (response != null && response.IsSuccessStatusCode)
                {
                    var responseString = response.Content.ReadAsStringAsync().Result;
                    returnItem = JsonConvert.DeserializeObject<List<ArmaUserPlaylistDataItem>>(responseString);
                }
            }

            return returnItem;
        }

        public List<ArmaPlaylistDataItem> GetPlaylistById(int playlistId)
        {
            List<ArmaPlaylistDataItem> returnItem = new List<ArmaPlaylistDataItem>();

            using (HttpClient client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(new
                {
                    playlistId = playlistId
                });

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpUtility.UrlEncode(_globalState.appToken));

                string endPoing = "https://armarad.com/Api"; // $"{(Debugger.IsAttached ? "https://localhost:7001/Api" : "https://armarad.com/Api")}";
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = client.PostAsync($"{endPoing}/GetPlaylistById", content).Result;

                if (response != null && response.IsSuccessStatusCode)
                {
                    var responseString = response.Content.ReadAsStringAsync().Result;
                    returnItem = JsonConvert.DeserializeObject<List<ArmaPlaylistDataItem>>(responseString);
                }
            }

            returnItem = returnItem.Where(sg => !string.IsNullOrWhiteSpace(sg.VideoId)).ToList();

            return returnItem;
        }

        public List<ArmaPlaylistDataItem> Offline_GetPlaylistById(int playlistId)
        {
            List<ArmaPlaylistDataItem> returnItem = _dapper.GetList<ArmaPlaylistDataItem>(@"
                select
                    us.id as Id,
                    us.artist as Artist,
                    us.songname as Song,
                    us.videoid as VideoId
                from playlists pl
                inner join usersongs us
                    on us.playlistid = pl.id
                where
                    pl.playlistid = @playlistid
                    and pl.songsavailableoffline = 1;
                ",
                new
                {
                    playlistid = playlistId
                });

            return returnItem;
        }

        public List<ArmaUserPlaylistDataItem> Offline_GetAvailablePlaylists()
        {
            List<ArmaUserPlaylistDataItem> returnItem = _dapper.GetList<ArmaUserPlaylistDataItem>(@"
                select distinct
                    pl.playlistid as Id,
                    pl.playlistname as PlaylistName
                from playlists pl
                join usersongs us
                    on us.playlistid = pl.id
                where
                    pl.songsavailableoffline = 1;
                ");

            return returnItem;
        }

        public ApiAudioDetailsDataItem GetAudioFileDetails(string VideoId)
        {
            ApiAudioDetailsDataItem returnItem = null;

            if (!string.IsNullOrWhiteSpace(VideoId))
            {
                byte[] fileBytes = null;

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpUtility.UrlEncode(_globalState.appToken));

                    string endPoint = "https://armarad.com/Api"; // $"{(Debugger.IsAttached ? "https://localhost:7001/Api" : "https://armarad.com/Api")}";
                    var response = client.GetAsync($"{endPoint}/GetAudioFileDetails?VideoId={HttpUtility.UrlEncode(VideoId.Trim())}").Result;

                    if (response != null && response.IsSuccessStatusCode)
                    {
                        //ApiAudioDetailsDataItem
                        var responseString = response.Content.ReadAsStringAsync().Result;
                        returnItem = JsonConvert.DeserializeObject<ApiAudioDetailsDataItem>(responseString);
                    }
                }
            }

            return returnItem;
        }

        public void DownloadPlaylistSongsForOffline(int? PlaylistId)
        {
            if (PlaylistId.HasValue)
            {
                List<ArmaPlaylistDataItem> songs = GetPlaylistById(PlaylistId.Value);

                if (songs != null && songs.Count > 0)
                {
                    foreach (var song in songs)
                    {
                        GetAudioFile(song.VideoId);
                    }

                    MarkPlaylistSongsAsDownloaded(PlaylistId.Value);
                }
            }
        }

        public void GetAudioFile(string VideoId)
        {
            string fileName = _dapper.GetFirstOrDefault<string>(@"
                select
                    filename
                from localsongs
                where
                    videoid=@videoid;
            ",
            new
            {
                videoid = VideoId
            });

            if (string.IsNullOrWhiteSpace(fileName))
            {
                string newFileName = $"{VideoId}.m4a";

                if (CheckIfAudioFileExists(newFileName))
                {
                    _dapper.ExecuteNonQuery(@"
                        insert into localsongs
                        (
                            videoid,
                            filename
                        )
                        values
                        (
                            @videoid,
                            @filename
                        );
                    ",
                    new
                    {
                        videoid = VideoId.Trim(),
                        filename = newFileName
                    });

                    fileName = newFileName;
                }
            }

            if ((!string.IsNullOrWhiteSpace(VideoId) && string.IsNullOrWhiteSpace(fileName)) || (!string.IsNullOrWhiteSpace(VideoId) && !CheckIfAudioFileExists($"{VideoId}.m4a")))
            {
                byte[] fileBytes = null;
                //ApiAudioDetailsDataItem audioDetails = GetAudioFileDetails(VideoId);

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpUtility.UrlEncode(_globalState.appToken));

                    string endPoint = "https://armarad.com/Api"; // $"{(Debugger.IsAttached ? "https://localhost:7001/Api" : "https://armarad.com/Api")}";
                    var response = client.GetAsync($"{endPoint}/GetAudioFile?VideoId={HttpUtility.UrlEncode(VideoId.Trim())}").Result;

                    if (response != null && response.IsSuccessStatusCode)
                    {
                        fileBytes = response.Content.ReadAsByteArrayAsync().Result;
                    }
                }

                if (fileBytes != null)
                {
                    string audioFolderPath = Path.Combine(FileSystem.AppDataDirectory, "audio");
                    string fileNameFinal = $"{VideoId.Trim()}.m4a";
                    string filePath = Path.Combine(audioFolderPath, fileNameFinal);

                    if (!CheckIfAudioFileExists(fileNameFinal))
                    {
                        File.WriteAllBytesAsync(filePath, fileBytes).Wait();

                        _dapper.ExecuteNonQuery(@"
                            insert into localsongs
                            (
                                videoid,
                                filename
                            )
                            values
                            (
                                @videoid,
                                @filename
                            );
                        ",
                        new
                        {
                            videoid = VideoId.Trim(),
                            filename = fileNameFinal
                        });
                    }
                }
            }
        }

        public void MarkPlaylistSongsAsDownloaded(int PlaylistId)
        {
            _dapper.ExecuteNonQuery(@"
                update playlists
                set
                    songsavailableoffline = 1
                where
                    playlistid=@playlistid
            ",
            new
            {
                playlistid = PlaylistId
            });
        }

        public bool CheckIfAudioFileExists(string FileName)
        {
            if (!string.IsNullOrWhiteSpace(FileName))
            {
                string audioFolderPath = Path.Combine(FileSystem.AppDataDirectory, "audio");

                if (!Directory.Exists(audioFolderPath))
                {
                    Directory.CreateDirectory(audioFolderPath);
                }

                string filePath = Path.Combine(audioFolderPath, FileName.Trim());

                return File.Exists(filePath);
            }

            return false;
        }

        public string GetFileWithPath(string FileName)
        {
            string returnItem = "";

            if (!string.IsNullOrWhiteSpace(FileName))
            {
                string audioFolderPath = Path.Combine(FileSystem.AppDataDirectory, "audio");

                if (!Directory.Exists(audioFolderPath))
                {
                    Directory.CreateDirectory(audioFolderPath);
                }

                returnItem = Path.Combine(audioFolderPath, FileName.Trim());
            }

            return returnItem;
        }

        public bool CheckIfAudioFileExistsByVideoId(string VideoId)
        {
            string fileName = _dapper.GetFirstOrDefault<string>(@"
                select
                    filename
                from localsongs
                where
                    videoid=@videoid;
            ",
            new
            {
                videoid = VideoId
            });

            if (!string.IsNullOrWhiteSpace(fileName))
            {
                string audioFolderPath = Path.Combine(FileSystem.AppDataDirectory, "audio");

                if (!Directory.Exists(audioFolderPath))
                {
                    Directory.CreateDirectory(audioFolderPath);
                }

                string filePath = Path.Combine(audioFolderPath, fileName.Trim());

                return File.Exists(filePath);
            }

            return false;
        }
    }
}
