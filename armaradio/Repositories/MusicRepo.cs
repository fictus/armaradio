using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using armaradio.Models;
using armaradio.Models.BeatPort;
using armaradio.Models.Request;
using armaradio.Models.Response;
using armaradio.Models.Youtube;
using Dapper;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using YoutubeExplode.Common;

namespace armaradio.Repositories
{
    public class MusicRepo : IMusicRepo
    {
        private readonly IDapperHelper _dapper;
        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;
        public MusicRepo(
            IDapperHelper dapper,
            Microsoft.Extensions.Configuration.IConfiguration configuration
        )
        {
            _dapper = dapper;
            _configuration = configuration;
        }

        public List<ArmaArtistDataItem> Artist_FindArtists(string search)
        {
            return _dapper.GetList<ArmaArtistDataItem>("radioconn", "Arma_SearchArtists", new
            {
                artist_name = search
            });
        }

        public List<ArtistDataItem> Artist_GetArtistList(string search)
        {
            return _dapper.GetList<ArtistDataItem>("radioconn", "Artist_GetArtistList", new
            {
                search = search
            });
        }

        public ArmaArtistAlbumsResponse Albums_GetArtistsAlbums(int artistId)
        {
            using (var con = _dapper.GetConnection("radioconn"))
            {
                var dataSet = con.QueryMultiple("Arma_GetAlbumsForArtist", new
                {
                    artist_id = artistId
                }, commandType: CommandType.StoredProcedure);

                ArmaArtistAlbumsResponse returnItem = new ArmaArtistAlbumsResponse()
                {
                    Albums = dataSet.Read<ArmaAlbumDataItem>().ToList(),
                    Singles = dataSet.Read<ArmaAlbumDataItem>().ToList()
                };

                return returnItem;
            }
        }

        public List<ArmaAlbumSongDataItem> Albums_GetAlbumSongs(int artistId, int albumId)
        {
            return _dapper.GetList<ArmaAlbumSongDataItem>("radioconn", "Arma_GetSongsForAlbum", new
            {
                album_id = albumId,
                artist_id = artistId
            });
        }

        public List<ArmaRandomSongDataItem> Songs_GetRandomFromPlaylists(string userIdentity)
        {
            return _dapper.GetList<ArmaRandomSongDataItem>("radioconn", "Arma_GetRandomFromPlaylists", new
            {
                user_identity = userIdentity
            });
        }

        public string GetApiToken()
        {
            string returnItem = _dapper.GetFirstOrDefault<string>("radioconn", "Operations_GetCachedToken");

            if (string.IsNullOrWhiteSpace(returnItem))
            {
                ApiTokenDataItem apiToken = null;
                string clientId = _configuration.GetSection("ApplicationConfiguration:apiClientId").Value;
                string clientSecret = _configuration.GetSection("ApplicationConfiguration:apiClientSecret").Value;

                // Define the URL and form data
                string url = "https://accounts.spotify.com/api/token";
                var formData = new System.Collections.Generic.Dictionary<string, string>
                {
                    { "grant_type", "client_credentials" },
                    { "client_id", clientId },
                    { "client_secret", clientSecret }
                };

                using (HttpClient client = new HttpClient())
                {
                    var encodedFormData = new FormUrlEncodedContent(formData);

                    using (var response = client.PostAsync(url, encodedFormData).Result)
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string responseBody = response.Content.ReadAsStringAsync().Result;
                            apiToken = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiTokenDataItem>(responseBody);
                        }
                    }
                }

                if (apiToken != null && !string.IsNullOrWhiteSpace(apiToken.access_token))
                {
                    returnItem = apiToken.access_token;

                    _dapper.ExecuteNonQuery("radioconn", "Operations_SaveApiTokenToCache", new
                    {
                        token = returnItem
                    });
                }
            }

            return returnItem;
        }

        public ArtistPlaylistsResponse GetArtistPlaylists(string artistName, string songName)
        {
            ArtistPlaylistsResponse returnItem = null;
            string url = "";

            if (!string.IsNullOrWhiteSpace(songName))
            {
                url = $"https://api.spotify.com/v1/search?q={Uri.EscapeUriString($"artist:{artistName} track:{songName}")}&type={Uri.EscapeUriString("track,playlist")}&limit=5";
                //url = $"https://api.spotify.com/v1/search?q=remaster%20track%3A{Uri.EscapeUriString(songName.Trim())}&artist%3A{Uri.EscapeUriString(artistName.Trim())}&type=playlist%2Cartist&limit=5";
            }
            else
            {
                url = $"https://api.spotify.com/v1/search?q=artist%3A{Uri.EscapeUriString(artistName.Trim())}&type=playlist%2Cartist";
            }

            string accessToken = GetApiToken();

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

                using (var response = client.GetAsync(url).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = response.Content.ReadAsStringAsync().Result;
                        returnItem = Newtonsoft.Json.JsonConvert.DeserializeObject<ArtistPlaylistsResponse>(responseBody);
                    }
                }
            }

            return returnItem;
        }

        public RadioSessionSongsResponse GetRadioPlalistSongsFromArtist(string artistName)
        {
            RadioSessionSongsResponse returnItem = null;
            ArtistPlaylistsResponse playlists = GetArtistPlaylists(artistName, "");
            bool radioInstanceFound = false;

            if (playlists != null && playlists.playlists != null && playlists.playlists.items != null && playlists.playlists.items.Count > 0)
            {
                foreach (var list in playlists.playlists.items)
                {
                    if ((list.name ?? "").Trim().ToLower().EndsWith(" radio"))
                    {
                        string url = $"https://api.spotify.com/v1/playlists/{list.id}/tracks";
                        string accessToken = GetApiToken();

                        using (HttpClient client = new HttpClient())
                        {
                            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

                            using (var response = client.GetAsync(url).Result)
                            {
                                if (response.IsSuccessStatusCode)
                                {
                                    radioInstanceFound = true;

                                    string responseBody = response.Content.ReadAsStringAsync().Result;
                                    returnItem = Newtonsoft.Json.JsonConvert.DeserializeObject<RadioSessionSongsResponse>(responseBody);
                                }
                            }
                        }

                        break;
                    }
                }
            }

            if (!radioInstanceFound)
            {
                if (playlists != null && playlists.playlists != null && playlists.playlists.items != null && playlists.playlists.items.Count > 0)
                {
                    Random random = new Random();
                    int randomNumber = random.Next(0, playlists.playlists.items.Count);
                    string url = $"https://api.spotify.com/v1/playlists/{playlists.playlists.items[randomNumber].id}/tracks";
                    string accessToken = GetApiToken();

                    using (HttpClient client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

                        using (var response = client.GetAsync(url).Result)
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                string responseBody = response.Content.ReadAsStringAsync().Result;
                                returnItem = Newtonsoft.Json.JsonConvert.DeserializeObject<RadioSessionSongsResponse>(responseBody);
                            }
                        }
                    }
                }
            }

            return returnItem;
        }

        public RadioSessionRecommendedResponse GetRadioSessionRecommendedSongsFromArtist(string artistName, string songName)
        {
            RadioSessionRecommendedResponse returnItem = null;
            ArtistPlaylistsResponse playlists = GetArtistPlaylists(artistName, songName);
            string artistId = "";
            string songId = "";

            if (playlists != null && playlists.artists != null && playlists.artists.items != null && playlists.artists.items.Count > 0)
            {
                artistId = playlists.artists.items[0].id;
            }

            if (playlists.tracks != null && playlists.tracks.items != null && playlists.tracks.items.Count > 0)
            {
                songId = playlists.tracks.items[0].id;

                if (string.IsNullOrWhiteSpace(artistId))
                {
                    if (playlists.tracks.items[0].artists != null && playlists.tracks.items[0].artists.Count > 0)
                    {
                        artistId = playlists.tracks.items[0].artists[0].id;
                    }
                }
            }

            string url = "";

            if (!string.IsNullOrWhiteSpace(artistId) && !string.IsNullOrWhiteSpace(songId))
            {
                url = $"https://api.spotify.com/v1/recommendations?seed_artists={artistId}&seed_tracks={songId}";
            }
            else if (!string.IsNullOrWhiteSpace(artistId))
            {
                url = $"https://api.spotify.com/v1/recommendations?seed_artists={artistId}";
            }
            else if (!string.IsNullOrWhiteSpace(songId))
            {
                url = $"https://api.spotify.com/v1/recommendations?seed_tracks={songId}";
            }

            if (!string.IsNullOrWhiteSpace(url))
            {
                string accessToken = GetApiToken();

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

                    using (var response = client.GetAsync(url).Result)
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string responseBody = response.Content.ReadAsStringAsync().Result;
                            returnItem = Newtonsoft.Json.JsonConvert.DeserializeObject<RadioSessionRecommendedResponse>(responseBody);
                        }
                    }
                }
            }

            return returnItem;
        }

        public void FindSimilarSong(int artistId)
        {
            ArmaArtistSimpleDataItem artistInfo = _dapper.GetFirstOrDefault<ArmaArtistSimpleDataItem>("radioconn", "Arma_GetArtistById", new
            {
                artist_id = 6954 //artistId
            });

            string url = $"https://open.spotify.com/search/{Uri.EscapeUriString(artistInfo.Name)}/playlists";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.UserAgent = "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko; Google Page Speed Insights) Chrome/27.0.1453 Safari/537.36";
            var config = AngleSharp.Configuration.Default.WithDefaultLoader();
            var browsingContext = BrowsingContext.New(config);

            browsingContext.OpenAsync(url).Wait();

            //using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            //{
                //if (response.StatusCode == HttpStatusCode.OK)
                //{
                    //string htmlContent;
                    //using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    //{
                    //    htmlContent = sr.ReadToEnd();
                    //}

                    //if (!string.IsNullOrEmpty(htmlContent))
                    //{
                    //    var parser = new HtmlParser();
                    //    var document = parser.ParseDocument(htmlContent);

                        var chartHolder = browsingContext.Active.QuerySelector("#searchPage");

                        if (chartHolder != null)
                        {
                            var firstChildDiv = chartHolder.QuerySelectorAll("div[data-testid='grid-container']").FirstOrDefault();

                            if (firstChildDiv != null)
                            {
                                var nextLayerChild = firstChildDiv.QuerySelectorAll("div").FirstOrDefault();

                                if (nextLayerChild != null)
                                {
                                    var infiniteScrollDiv = nextLayerChild.QuerySelectorAll("div[data-testid='infinite-scroll-list']").FirstOrDefault();

                                    if (infiniteScrollDiv != null)
                                    {
                                        var resultsContainer = infiniteScrollDiv.QuerySelectorAll("div[data-testid='grid-container']").FirstOrDefault();

                                        if (resultsContainer != null)
                                        {
                                            var allDivs = resultsContainer.QuerySelectorAll("div[data-encore-id='card'][role='group']");

                                            if (allDivs != null)
                                            {
                                                foreach (var div in allDivs)
                                                {
                                                    try
                                                    {
                                                        var dataHolder = div.QuerySelector("ul:nth-child(1)").QuerySelector("li:nth-child(4)").QuerySelector("li:nth-child(1)");
                                                        string songName = (dataHolder.QuerySelector("h3").Text() ?? "").Trim();
                                                        string artistName = (dataHolder.QuerySelector("span").Text() ?? "").Trim();

                                                        //returnItem.Add(new TrackDataItem()
                                                        //{
                                                        //    artist_name = artistName,
                                                        //    track_name = songName,
                                                        //    tid = -1,
                                                        //    usability_score = 0
                                                        //});
                                                    }
                                                    catch (Exception ex)
                                                    {

                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    //}
                //}
            //}

            //// to implement from url: https://spotalike.com/en/songs-similar-to/luis-miguel-la-incondicional/3225079

            //string qry = @"
            //    let endPointUrl = ""https://api.spotalike.com/v1/playlists"";

            //    // Default options are marked with *
            //    let fetchBody = {
            //        method: ""POST"", // *GET, POST, PUT, DELETE, etc.
            //        mode: ""cors"", // no-cors, *cors, same-origin
            //        cache: ""no-cache"", // *default, no-cache, reload, force-cache, only-if-cached
            //        credentials: ""same-origin"", // include, *same-origin, omit
            //        headers: {
            //            ""Content-Type"": ""application/json"",
            //            ""Accept-Encoding"": ""gzip, deflate""
            //            // 'Content-Type': 'application/x-www-form-urlencoded',
            //        },
            //        redirect: ""follow"", // manual, *follow, error
            //        referrerPolicy: ""no-referrer"", // no-referrer, *no-referrer-when-downgrade, origin, origin-when-cross-origin, same-origin, strict-origin, strict-origin-when-cross-origin, unsafe-url
            //        body: JSON.stringify({
            //            artist: ""Luis Miguel"",
            //            mbid: ""6224ac4e-8c97-4005-9e45-6f10ad394404"",
            //            track: ""La Incondicional""
            //        }),
            //    };

            //    fetch(endPointUrl, fetchBody)
            //        .then(function (response) {
            //            if (response.status >= 500) {
            //                return response.text();
            //            } else {
            //                return response.json().catch(function () {
            //                    return {};
            //                });
            //            }
            //        })
            //        .then(function (jsonData) {
            //            console.log(jsonData);
            //            //d.resolve(jsonData);
            //        })
            //        .catch(function (error) {
            //            console.log(error.message);
            //            //d.resolve({ error: true, errorMsg: error.message });
            //        });
            //";
        }

        public List<TrackDataItem> Tracks_GetTop50Songs()
        {
            return _dapper.GetList<TrackDataItem>("radioconn", "Tracks_GetTop50UserFavorites");
        }

        public List<TrackDataItem> GetCurrentTop100()
        {
            List<TrackDataItem> returnItem = new List<TrackDataItem>();
            int? key_id = _dapper.GetFirstOrDefault<int?>("radioconn", "Top100_GetDailyKey");

            if (key_id.HasValue)
            {
                returnItem = _dapper.GetList<TrackDataItem>("radioconn", "Top100_GetTodaysTopSongs", new
                {
                    key_id = key_id.Value
                });
            }
            else
            {
                key_id = _dapper.GetFirstOrDefault<int?>("radioconn", "Top100_GenerateDailyKey");

                if (key_id.HasValue)
                {
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

                                using (var con = _dapper.GetConnection("radioconn"))
                                {
                                    int rankId = 0;
                                    foreach (var div in allDivs)
                                    {
                                        rankId++;

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
                                                usability_score = rankId
                                            });

                                            _dapper.ExecuteNonQuery(con, "Top100_InsertSongData", new
                                            {
                                                key_id = key_id.Value,
                                                rank = rankId,
                                                artist = artistName,
                                                song = songName
                                            });
                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                    }
                                }
                            }
                        }
                    }

                    _dapper.ExecuteNonQuery("radioconn", "Top100_BackupOldSongs", new
                    {
                        key_id = key_id.Value
                    });
                }
            }

            return returnItem;
        }

        public List<TrackDataItem> GetCurrentTop40DanceSingles()
        {
            List<TrackDataItem> returnItem = new List<TrackDataItem>();
            DateTime dateNow = DateTime.Now;

            string url = $"https://www.officialcharts.com/charts/dance-singles-chart/{dateNow.ToString("yyyyMMdd")}";

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
                        string pattern = @"id=""__NUXT_DATA__"" data-ssr=""true"">(.*?)</script>";
                        Match match = Regex.Match(htmlContent, pattern);

                        if (match.Success)
                        {
                            string currentVideoId = "";
                            string jsonString = match.Groups[1].Value;
                            //jsonString = jsonString.Replace("'", "\"");
                            List<object> jsonObject = Newtonsoft.Json.JsonConvert.DeserializeObject<List<object>>(jsonString);
                            List<object> cleanList = new List<object>();
                            bool startAdding = false;

                            cleanList.Add("player-3242342-0");

                            foreach (var item in jsonObject)
                            {
                                if (!startAdding)
                                {
                                    if ((item ?? "").ToString() == "track-info")
                                    {
                                        startAdding = true;
                                        continue;
                                    }
                                }
                                else
                                {
                                    if (item != null)
                                    {
                                        string currentItem = item.ToString().Trim();

                                        if (!currentItem.StartsWith("{") && !currentItem.EndsWith("}"))
                                        {
                                            cleanList.Add(item);
                                        }
                                    }
                                }
                            }

                            var textinfo = new CultureInfo("en-US", false).TextInfo;

                            for (int i= 0; i < cleanList.Count; i++)
                            {
                                if (cleanList[i].ToString().StartsWith("player-") && returnItem.Count < 40)
                                {
                                    returnItem.Add(new TrackDataItem()
                                    {
                                        artist_name = textinfo.ToTitleCase(cleanList[i + 4].ToString().ToLower()),
                                        track_name = textinfo.ToTitleCase(cleanList[i + 2].ToString().ToLower()),
                                        tid = -1,
                                        usability_score = 0
                                    });
                                }
                                //if (i == 0)
                                //{
                                //    returnItem.Add(new TrackDataItem()
                                //    {
                                //        artist_name = cleanList[i + 5].ToString(),
                                //        track_name = cleanList[i + 3].ToString(),
                                //        tid = -1,
                                //        usability_score = 0
                                //    });
                                //}
                                //else
                                //{

                                //}
                            }
                        }



                        //var parser = new HtmlParser();
                        //var document = parser.ParseDocument(htmlContent);

                        //var chartHolder = document.QuerySelector("div.chart-content");

                        //var allDivs = chartHolder.QuerySelectorAll("div.chart-item");

                        //foreach (var div in allDivs)
                        //{
                        //    try
                        //    {
                        //        var dataHolder = div.QuerySelector("div.description");
                        //        string songName = (dataHolder.QuerySelector("a.chart-name").Text() ?? "").Trim();
                        //        string artistName = (dataHolder.QuerySelector("a.chart-artist").Text() ?? "").Trim();

                        //        returnItem.Add(new TrackDataItem()
                        //        {
                        //            artist_name = artistName,
                        //            track_name = songName,
                        //            tid = -1,
                        //            usability_score = 0
                        //        });
                        //    }
                        //    catch (Exception ex)
                        //    {

                        //    }
                        //}
                    }
                }
            }

            return returnItem;
        }

        public List<TrackDataItem> GetCurrentTranceTop100()
        {
            List<TrackDataItem> returnItem = new List<TrackDataItem>();

            string url = "https://www.beatport.com/genre/trance-main-floor/7/top-100";

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
                        string pattern = @"id=""__NEXT_DATA__"" type=""application/json"">(.*?)</script>";
                        Match match = Regex.Match(htmlContent, pattern);

                        if (match.Success)
                        {
                            string jsonString = match.Groups[1].Value;
                            //jsonString = jsonString.Replace("'", "\"");
                            TranceTopSearchDataItem jsonObject = Newtonsoft.Json.JsonConvert.DeserializeObject<TranceTopSearchDataItem>(jsonString);

                            if (jsonObject.props.pageProps.dehydratedState.queries.Count > 0)
                            {
                                foreach (var content in jsonObject.props.pageProps.dehydratedState.queries.First().state.data.results)
                                {
                                    List<string> artists = new List<string>();

                                    foreach (var artist in content.artists)
                                    {
                                        artists.Add(artist.name);
                                    }

                                    string artistName = string.Join(", ", artists.ToArray());
                                    string songName = $"{content.name}{(!string.IsNullOrWhiteSpace(content.mix_name) ? $" ({content.mix_name})" : "")}";

                                    returnItem.Add(new TrackDataItem()
                                    {
                                        artist_name = artistName,
                                        track_name = songName,
                                        tid = -1,
                                        usability_score = 0
                                    });
                                }
                            }
                        }
                    }
                }
            }

            return returnItem;
        }

        public List<TrackDataItem> GetCurrentTranceHype100()
        {
            List<TrackDataItem> returnItem = new List<TrackDataItem>();

            string url = "https://www.beatport.com/genre/trance-main-floor/7/hype-100";

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
                        string pattern = @"id=""__NEXT_DATA__"" type=""application/json"">(.*?)</script>";
                        Match match = Regex.Match(htmlContent, pattern);

                        if (match.Success)
                        {
                            string jsonString = match.Groups[1].Value;
                            //jsonString = jsonString.Replace("'", "\"");
                            TranceTopSearchDataItem jsonObject = Newtonsoft.Json.JsonConvert.DeserializeObject<TranceTopSearchDataItem>(jsonString);

                            if (jsonObject.props.pageProps.dehydratedState.queries.Count > 0)
                            {
                                foreach (var content in jsonObject.props.pageProps.dehydratedState.queries.First().state.data.results)
                                {
                                    List<string> artists = new List<string>();

                                    foreach (var artist in content.artists)
                                    {
                                        artists.Add(artist.name);
                                    }

                                    string artistName = string.Join(", ", artists.ToArray());
                                    string songName = $"{content.name}{(!string.IsNullOrWhiteSpace(content.mix_name) ? $" ({content.mix_name})" : "")}";

                                    returnItem.Add(new TrackDataItem()
                                    {
                                        artist_name = artistName,
                                        track_name = songName,
                                        tid = -1,
                                        usability_score = 0
                                    });
                                }
                            }
                        }
                    }
                }
            }

            return returnItem;
        }

        public YTVideoIdsDataItem Youtube_GetUrlByArtistNameSongName(string artistName, string songName)
        {
            YTVideoIdsDataItem returnItem = null;
            List<string> alternateIds = new List<string>();

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
                                            if (string.IsNullOrWhiteSpace(currentVideoId))
                                            {
                                                currentVideoId = subContent.videoRenderer.videoId;
                                            }
                                            else
                                            {
                                                alternateIds.Add(subContent.videoRenderer.videoId);
                                            }
                                            //currentVideoTitle = content.itemSectionRenderer.contents[0].videoRenderer.title.runs[0].text;
                                        }
                                    }
                                }
                            }

                            if (!string.IsNullOrWhiteSpace(currentVideoId))
                            {
                                returnItem = new YTVideoIdsDataItem()
                                {
                                    VideoId = currentVideoId,
                                    AlternateIds = alternateIds
                                };
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

                if (returnItem.Count == 0)
                {
                    returnItem = DuckDuckGo_PerformGeneralSearch(searchText);
                }
            }

            return returnItem;
        }

        // implement Creative Commons search from DuckDuckGo
        // https://duckduckgo.com/?t=h_&q=depeche+mode+somebody&iax=videos&ia=videos&iaf=videoLicense%3AcreativeCommon
        public List<YTGeneralSearchDataItem> DuckDuckGo_PerformGeneralSearch(string searchText)
        {
            List<YTGeneralSearchDataItem> returnItem = new List<YTGeneralSearchDataItem>();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                string userInput = Uri.EscapeDataString(searchText).Replace("%20", "+");

                string url = $"https://duckduckgo.com/?t=h_&q={userInput}&iax=videos&ia=videos&iaf=videoLicense%3AcreativeCommon";

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

                            var chartHolder = document.QuerySelector("#links");
                            var allDivs = chartHolder.QuerySelectorAll("div.result.web-result");

                            foreach (var div in allDivs)
                            {
                                try
                                {
                                    var videoAlink = (div.QuerySelector("h2")).QuerySelector("a");
                                    string videoUrl = videoAlink.GetAttribute("href");

                                    if ((videoUrl ?? "").Contains("youtube"))
                                    {
                                        string videoId = videoUrl.Split('=').Last();

                                        if (!(returnItem.Any(v => v.videoId == videoId)))
                                        {
                                            string nameText = videoAlink.Text().Trim();
                                            nameText = (nameText.Replace("youtube", "", StringComparison.OrdinalIgnoreCase)).Trim().TrimEnd('-').TrimEnd('|').Trim();
                                            string artistName = (nameText.Split('-').FirstOrDefault() ?? "").Trim();
                                            string songName = (nameText.Contains("-") ? nameText.Substring(nameText.IndexOf("-") + 1).Trim() : "");

                                            returnItem.Add(new YTGeneralSearchDataItem()
                                            {
                                                videoId = videoId,
                                                artistName = artistName,
                                                songName = songName,
                                                thumbNail = null
                                            });
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {

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
