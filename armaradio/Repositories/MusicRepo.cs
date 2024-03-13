using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using armaradio.Models;
using armaradio.Models.Youtube;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;

namespace armaradio.Repositories
{
    public class MusicRepo : IMusicRepo
    {
        private readonly IDapperHelper _dapper;
        public MusicRepo(
            IDapperHelper dapper  
        )
        {
            _dapper = dapper;
        }

        public List<ArtistDataItem> Artist_GetArtistList(string search)
        {
            return _dapper.GetList<ArtistDataItem>("radioconn", "Artist_GetArtistList", new
            {
                search = search
            });
        }

        public List<TrackDataItem> Tracks_GetTop50Songs()
        {
            return _dapper.GetList<TrackDataItem>("radioconn", "Tracks_GetTop50UserFavorites");
        }

        public List<TrackDataItem> GetCurrentTop100()
        {
            List<TrackDataItem> returnItem = new List<TrackDataItem>();

            string url = "https://www.billboard.com/charts/hot-100/";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.UserAgent = "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko; Google Page Speed Insights) Chrome/27.0.1453 Safari/537.36";

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string htmlContent;
                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    {
                        htmlContent = sr.ReadToEnd();
                    }

                    if (!string.IsNullOrEmpty(htmlContent))
                    {
                        var parser = new HtmlParser();
                        var document = parser.ParseDocument(htmlContent);

                        var chartHolder = document.QuerySelector("div.chart-results-list");

                        var allDivs = chartHolder.QuerySelectorAll("div.o-chart-results-list-row-container");

                        foreach (var div in allDivs)
                        {
                            try
                            {
                                var dataHolder = div.QuerySelector("ul:nth-child(1)").QuerySelector("li:nth-child(4)").QuerySelector("li:nth-child(1)");
                                string songName = (dataHolder.QuerySelector("h3").Text() ?? "").Trim();
                                string artistName = (dataHolder.QuerySelector("span").Text() ?? "").Trim();

                                returnItem.Add(new TrackDataItem()
                                {
                                    artist_name = artistName,
                                    track_name = songName,
                                    tid = -1,
                                    usability_score = 0
                                });
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                }
            }

            return returnItem;
        }

        public string Youtube_GetUrlByArtistNameSongName(string artistName, string songName)
        {
            string returnItem = "";

            string userInput = HttpUtility.UrlEncode($"{artistName} - {songName}");

            string url = $"https://www.youtube.com/results?search_query={userInput}&sp=EgIQAQ%253D%253D";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.UserAgent = "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko; Google Page Speed Insights) Chrome/27.0.1453 Safari/537.36";

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string htmlContent;
                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    {
                        htmlContent = sr.ReadToEnd();
                    }

                    if (!string.IsNullOrEmpty(htmlContent))
                    {
                        string pattern = @"var ytInitialData = (.*?);\s*</script>";
                        Match match = Regex.Match(htmlContent, pattern);

                        if (match.Success)
                        {
                            string currentVideoId = "";
                            string jsonString = match.Groups[1].Value;
                            //jsonString = jsonString.Replace("'", "\"");
                            YTArtistSongNameResponse jsonObject = Newtonsoft.Json.JsonConvert.DeserializeObject<YTArtistSongNameResponse>(jsonString);

                            foreach (var content in jsonObject.contents.twoColumnSearchResultsRenderer.primaryContents.sectionListRenderer.contents)
                            {
                                if (content.itemSectionRenderer != null && content.itemSectionRenderer.contents != null)
                                {
                                    foreach (var subContent in content.itemSectionRenderer.contents)
                                    {
                                        if (subContent.videoRenderer != null)
                                        {
                                            currentVideoId = subContent.videoRenderer.videoId;
                                            //currentVideoTitle = content.itemSectionRenderer.contents[0].videoRenderer.title.runs[0].text;
                                            break;
                                        }
                                    }
                                }

                                if (!string.IsNullOrWhiteSpace(currentVideoId))
                                {
                                    break;
                                }
                            }

                            if (!string.IsNullOrWhiteSpace(currentVideoId))
                            {
                                returnItem = currentVideoId;
                            }
                        }
                    }
                }
            }

            return returnItem;
        }

        public List<YTGeneralSearchDataItem> Youtube_PerformGeneralSearch(string searchText)
        {
            List<YTGeneralSearchDataItem> returnItem = new List<YTGeneralSearchDataItem>();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                string userInput = HttpUtility.UrlEncode(searchText);

                string url = $"https://www.youtube.com/results?search_query={userInput}&sp=EgIQAQ%253D%253D";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.UserAgent = "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko; Google Page Speed Insights) Chrome/27.0.1453 Safari/537.36";

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        string htmlContent;
                        using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                        {
                            htmlContent = sr.ReadToEnd();
                        }

                        if (!string.IsNullOrEmpty(htmlContent))
                        {
                            string pattern = @"var ytInitialData = (.*?);\s*</script>";
                            Match match = Regex.Match(htmlContent, pattern);

                            if (match.Success)
                            {
                                string jsonString = match.Groups[1].Value;
                                //jsonString = jsonString.Replace("'", "\"");
                                YTArtistSongNameResponse jsonObject = Newtonsoft.Json.JsonConvert.DeserializeObject<YTArtistSongNameResponse>(jsonString);

                                foreach (var content in jsonObject.contents.twoColumnSearchResultsRenderer.primaryContents.sectionListRenderer.contents)
                                {
                                    if (content.itemSectionRenderer != null && content.itemSectionRenderer.contents != null)
                                    {
                                        foreach (var subContent in content.itemSectionRenderer.contents)
                                        {
                                            if (subContent.videoRenderer != null && !string.IsNullOrWhiteSpace(subContent.videoRenderer.videoId))
                                            {
                                                string artistSong = subContent.videoRenderer.title?.runs?.Count > 0 ? (subContent.videoRenderer.title.runs[0].text ?? "") : "";
                                                string artistName = (artistSong.Split('-').First() ?? "").Trim();
                                                string songName = (artistSong.Contains("-") ? artistSong.Substring(artistSong.IndexOf("-") + 1).Trim() : "");
                                                Thumbnails thumbNail = subContent.videoRenderer.thumbnail?.thumbnails?.Count > 0 ? subContent.videoRenderer.thumbnail.thumbnails[0] : null;

                                                returnItem.Add(new YTGeneralSearchDataItem()
                                                {
                                                    videoId = subContent.videoRenderer.videoId,
                                                    artistName = artistName,
                                                    songName = songName,
                                                    thumbNail = thumbNail
                                                });
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return returnItem;
        }

        public bool CheckIfPlaylistExists(string PlaylistName, string UserId)
        {
            return _dapper.GetFirstOrDefault<bool>("radioconn", "ArmaPlayList_CheckIfPlaylistExists", new
            {
                playlist_name = PlaylistName,
                user_id = UserId
            });
        }

        public int? InsertPlaylistName(
            string PlaylistName,
            string UserId
        )
        {
            return _dapper.GetFirstOrDefault<int?>("radioconn", "ArmaPlayList_InsertPlaylist", new
            {
                playlist_name = PlaylistName,
                user_id = UserId
            });
        }

        public void InsertSongToPlaylist(
            int PlaylistId,
            string Artist,
            string Song
        )
        {
            _dapper.ExecuteNonQuery("radioconn", "ArmaPlayList_InsertSongToPlaylist", new
            {
                PlaylistId = PlaylistId,
                Artist = Artist,
                Song = Song
            });
        }

        public List<ArmaPlaylistDataItem> GetPlaylistByName(
            string PlaylistName,
            string UserId
        )
        {
            return _dapper.GetList<ArmaPlaylistDataItem>("radioconn", "ArmaPlayList_GetPlaylistByName", new
            {
                playlist_name = PlaylistName,
                user_id = UserId
            });
        }

        public List<ArmaPlaylistDataItem> GetPlaylistById(int PlaylistId, string UserId)
        {
            return _dapper.GetList<ArmaPlaylistDataItem>("radioconn", "ArmaPlayList_GetPlaylistById", new
            {
                playlist_id = PlaylistId,
                user_id = UserId
            });
        }

        public List<ArmaUserPlaylistDataItem> GetUserPlaylists(string UserId)
        {
            return _dapper.GetList<ArmaUserPlaylistDataItem>("radioconn", "ArmaPlayList_GetUserPlaylists", new
            {
                user_id = UserId
            });
        }

        public void DeleteSongFromPlaylist(int SongId, string UserId)
        {
            _dapper.ExecuteNonQuery("radioconn", "ArmaPlayList_DeleteSongFromPlaylist", new
            {
                song_id = SongId,
                user_id = UserId
            });
        }

        public void DeleteUserPlaylistAndData(int PlaylistId, string UserId)
        {
            _dapper.ExecuteNonQuery("radioconn", "ArmaPlayList_DeleteUserPlaylistAndData", new
            {
                playlist_id = PlaylistId,
                user_id = UserId
            });
        }

        public void AddSongToPlaylist(int PlaylistId, string Artist, string Song, string VideoId)
        {
            _dapper.ExecuteNonQuery("radioconn", "ArmaPlayList_AddSongToPlaylist", new
            {
                playlist_id = PlaylistId,
                artist = Artist,
                song = Song,
                video_id = VideoId
            });
        }
    }
}
