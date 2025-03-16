using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using armaradio.Models;
using armaradio.Models.BeatPort;
using armaradio.Models.Request;
using armaradio.Models.Response;
using armaradio.Models.Youtube;
using Dapper;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using YoutubeDLSharp.Options;
using YoutubeDLSharp;
using YoutubeExplode.Common;
using YoutubeExplode.Videos;
using System.Diagnostics;
using FFMpegCore.Enums;
using System.Runtime.InteropServices;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Identity.Client;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Net.Http.Json;
using System.Reflection;
using System.IO.Compression;

namespace armaradio.Repositories
{
    public class MusicRepo : IMusicRepo
    {
        private readonly IDapperHelper _dapper;
        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;
        private readonly ArmaYoutubeDownloader _armaYTDownloader;
        public MusicRepo(
            IDapperHelper dapper,
            Microsoft.Extensions.Configuration.IConfiguration configuration,
            ArmaYoutubeDownloader armaYTDownloader
        )
        {
            _dapper = dapper;
            _configuration = configuration;
            _armaYTDownloader = armaYTDownloader;
        }

        public Guid? GetSiteApiToken()
        {
            return _dapper.GetFirstOrDefault<Guid?>("radioconn", "Arma_SiteGetApiToken");
        }

        public bool UseSiteApiToken(Guid token)
        {
            return _dapper.GetFirstOrDefault<bool>("radioconn", "Arma_SiteUseApiToken", new
            {
                token = token
            });
        }

        public List<ArmaArtistDataItem> Artist_FindArtists(string search)
        {
            return _dapper.GetList<ArmaArtistDataItem>("radioconn", "Arma_SearchArtists", new
            {
                artist_name = search
            });
        }

        public ArmaArtistDataItem Artist_GetArtistByMBid(string artist_mbid)
        {
            return _dapper.GetFirstOrDefault<ArmaArtistDataItem>("radioconn", "Arma_GetArtistByMBid", new
            {
                artist_mbid = artist_mbid
            });
        }

        public List<ArmaArtistDataItem> Artist_FindArtistsInternal(string search)
        {
            return _dapper.GetList<ArmaArtistDataItem>("radioconn", "Arma_SearchArtistsInternal", new
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
        public byte[] CompressJsonString(string data)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(data);

            var memoryStream = new MemoryStream();
            using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
            {
                gZipStream.Write(buffer, 0, buffer.Length);
            }

            return memoryStream.ToArray();
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
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
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
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
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
                            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
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
                        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
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

        public List<string> GetGenresByArtist(string artistName)
        {
            List<string> returnItem = new List<string>();
            ArtistPlaylistsResponse playlists = GetArtistPlaylists(artistName, "");

            if (playlists != null && playlists.artists != null && playlists.artists.items != null && playlists.artists.items.Count > 0)
            {
                returnItem = playlists.artists.items[0].genres ?? new List<string>();
            }

            return returnItem;
        }

        public List<ArmaRecommendationDataItem> GetRadioSessionRecommendedSongsFromArtist(string artistName, string songName)
        {
            List<ArmaRecommendationDataItem> returnItem = new List<ArmaRecommendationDataItem>();

            ArmaArtistDataItem currentArtist = (Artist_FindArtistsInternal(artistName) ?? new List<ArmaArtistDataItem>()).FirstOrDefault();

            if (currentArtist != null)
            {
                returnItem = _dapper.GetList<ArmaRecommendationDataItem>("recommendations", "arma_get_suggestions_by_renge_related_artists", new
                {
                    artist_mbid = currentArtist.Artist_MBId,
                    only_by_genre = true
                }) ?? new List<ArmaRecommendationDataItem>();

                if (returnItem.Count == 0)
                {
                    List<ArmaApiSimilarArtistIdDataItem> likeArtists = SiteApiGetSimilarArtistIds(currentArtist.Artist_MBId);

                    foreach (var simArtist in likeArtists)
                    {
                        returnItem = _dapper.GetList<ArmaRecommendationDataItem>("recommendations", "arma_get_suggestions_by_renge_related_artists", new
                        {
                            artist_mbid = simArtist.artist_mbid,
                            only_by_genre = true,
                            use_spotify_genres = true
                        }) ?? new List<ArmaRecommendationDataItem>();

                        if (returnItem.Count > 0)
                        {
                            break;
                        }
                    }
                }

                List<string> relatedArtists = new List<string>();

                if (returnItem.Count == 0)
                {
                    relatedArtists = GetSimilarArtistNames(artistName) ?? new List<string>();
                    relatedArtists = relatedArtists.OrderBy(an => Guid.NewGuid()).ToList();

                    for (int i = 0; i < relatedArtists.Count; i++)
                    {
                        currentArtist = (Artist_FindArtistsInternal(relatedArtists[i]) ?? new List<ArmaArtistDataItem>()).FirstOrDefault();

                        if (currentArtist != null)
                        {
                            returnItem = _dapper.GetList<ArmaRecommendationDataItem>("recommendations", "arma_get_suggestions_by_renge_related_artists", new
                            {
                                artist_mbid = currentArtist.Artist_MBId,
                                only_by_genre = true
                            }) ?? new List<ArmaRecommendationDataItem>();
                        }

                        if (returnItem.Count > 0)
                        {
                            break;
                        }
                    }
                }

                if (returnItem.Count == 0)
                {
                    returnItem = _dapper.GetList<ArmaRecommendationDataItem>("recommendations", "arma_get_suggestions_by_renge_related_artists", new
                    {
                        artist_mbid = currentArtist.Artist_MBId
                    }) ?? new List<ArmaRecommendationDataItem>();
                }

                if (returnItem.Count == 0)
                {
                    for (int i = 0; i < relatedArtists.Count; i++)
                    {
                        currentArtist = (Artist_FindArtistsInternal(relatedArtists[i]) ?? new List<ArmaArtistDataItem>()).FirstOrDefault();

                        if (currentArtist != null)
                        {
                            returnItem = _dapper.GetList<ArmaRecommendationDataItem>("recommendations", "arma_get_suggestions_by_renge_related_artists", new
                            {
                                artist_mbid = currentArtist.Artist_MBId
                            }) ?? new List<ArmaRecommendationDataItem>();
                        }

                        if (returnItem.Count > 0)
                        {
                            break;
                        }
                    }
                }

                if (returnItem.Count == 0)
                {
                    for (int i = 0; i < relatedArtists.Count; i++)
                    {
                        currentArtist = (Artist_FindArtistsInternal(relatedArtists[i]) ?? new List<ArmaArtistDataItem>()).FirstOrDefault();

                        if (currentArtist != null)
                        {
                            returnItem = _dapper.GetList<ArmaRecommendationDataItem>("recommendations", "arma_get_suggestions_by_artist", new
                            {
                                artist_mbid = currentArtist.Artist_MBId
                            }) ?? new List<ArmaRecommendationDataItem>();
                        }

                        if (returnItem.Count > 0)
                        {
                            break;
                        }
                    }
                }
            }

            return returnItem;
        }

        public List<ArmaApiSimilarArtistIdDataItem> SiteApiGetSimilarArtistIds(string artistid)
        {
            List<ArmaApiSimilarArtistIdDataItem> returnItem = new List<ArmaApiSimilarArtistIdDataItem>();
            ArmaArtistDataItem queryArtist = Artist_GetArtistByMBid(artistid);
            List<string> relatedArtists = GetSimilarArtistNames(queryArtist.ArtistName) ?? new List<string>();
            relatedArtists = relatedArtists.OrderBy(an => Guid.NewGuid()).ToList();

            for (int i = 0; i < relatedArtists.Count; i++)
            {
                ArmaArtistDataItem currentArtist = (Artist_FindArtistsInternal(relatedArtists[i]) ?? new List<ArmaArtistDataItem>()).FirstOrDefault();

                if (currentArtist != null)
                {
                    returnItem.Add(new ArmaApiSimilarArtistIdDataItem()
                    {
                        artist_name = currentArtist.ArtistName,
                        artist_mbid = currentArtist.Artist_MBId
                    });

                    if (i >= 20)
                    {
                        break;
                    }
                }
            }

            return returnItem;
        }

        public List<string> GetSimilarArtistNames(string artistName)
        {
            List<string> returnItem = new List<string>();
            string url = $"https://www.music-map.com/{Uri.EscapeUriString(artistName)}";

            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.UserAgent = "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko; Google Page Speed Insights) Chrome/27.0.1453 Safari/537.36";
            var config = AngleSharp.Configuration.Default.WithDefaultLoader();
            var browsingContext = BrowsingContext.New(config);

            browsingContext.OpenAsync(url).Wait();

            var namestHolder = browsingContext.Active.QuerySelector("#gnodMap");

            if (namestHolder != null)
            {
                var allNamesLinks = namestHolder.QuerySelectorAll("a");

                if (allNamesLinks != null)
                {
                    for (int i = 0; i < allNamesLinks.Count(); i++)
                    {
                        if (i != 0)
                        {
                            var link = allNamesLinks[i];

                            try
                            {
                                returnItem.Add(link.Text().Trim());
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

        public RadioSessionRecommendedResponse GetRadioSessionRecommendedSongsFromArtist_OLD(string artistName, string songName)
        {
            RadioSessionRecommendedResponse returnItem = null;
            ArtistPlaylistsResponse playlists = GetArtistPlaylists(artistName, songName);
            string artistId = "";
            string songId = "";

            if (playlists != null && playlists.artists != null && playlists.artists.items != null && playlists.artists.items.Count > 0)
            {
                artistId = playlists.artists.items[0].id;
            }

            //if (string.IsNullOrWhiteSpace(artistId))
            //{
            //    if (playlists != null && playlists.tracks != null && playlists.tracks.items != null && playlists.tracks.items.Count > 0)
            //    {
            //        if (playlists.tracks.items[0].artists != null && playlists.tracks.items[0].artists.Count > 0)
            //        {
            //            artistId = playlists.tracks.items[0].artists[0].id;
            //        }
            //    }
            //}

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
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
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

            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
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

        public async Task<ArmaAISongsResponse> GetAIRecommendedSongs(string artistName, string songName, int limit = 10)
        {
            ArmaAISongsResponse returnItem = new ArmaAISongsResponse()
            {
                Rerun = true,
                Songs = new List<ArmaAISongDataItem>()
            };
            ArmaAIRecommendationResponse responseItem = null;

            string artistSong = $"{((artistName ?? "").Trim())}{(!string.IsNullOrWhiteSpace(artistName) && !string.IsNullOrWhiteSpace(songName) ? " " : "")}{((songName ?? "").Trim())}";
            string promptText = $"pretend you are a dataserver. recommend {limit} songs, from various artists, similar to '{artistSong}'. return data in this format 'artist | song\\n'. don't add other wording or explanations";
            string url = "http://163.172.72.32:5000/api/v1/generate";

            using (var client = new HttpClient()
            {
                Timeout = TimeSpan.FromMinutes(5)
            })
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer 17791853-d396-496c-bd3f-0c5bc51ae27d");

                var payload = new
                {
                    prompt = promptText
                    //temperature = "1"
                };

                var content = new StringContent(
                    Newtonsoft.Json.JsonConvert.SerializeObject(payload),
                    Encoding.UTF8,
                    "application/json");

                using (var response = await client.PostAsync(url, content))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        responseItem = Newtonsoft.Json.JsonConvert.DeserializeObject<ArmaAIRecommendationResponse>(responseBody);
                    }

                    if (responseItem != null && responseItem.results != null && responseItem.results.Count > 0)
                    {
                        List<string> tempSongs = new List<string>();

                        foreach (var item in responseItem.results)
                        {
                            List<string> responseParts = (item.text ?? "").Trim().Split('\n', StringSplitOptions.RemoveEmptyEntries).ToList();

                            foreach (var part in responseParts)
                            {
                                string currentPart = part.Trim();

                                if (currentPart.Contains("|"))
                                {
                                    string songToAdd = currentPart.Replace("\\n", "").Trim('\\').Trim('.').Trim();

                                    if (!(tempSongs.Any(sg => sg.ToLower() == songToAdd.ToLower())))
                                    {
                                        List<string> songParts = songToAdd.Split('|', StringSplitOptions.RemoveEmptyEntries).ToList();

                                        if (songParts.Count > 1 && !(string.IsNullOrWhiteSpace(songParts.First()) && string.IsNullOrWhiteSpace(songParts.Last())))
                                        {
                                            string firstPart = (songParts.First()).Trim();
                                            string lastPart = songParts.Last().Trim();

                                            if (lastPart.Contains("  "))
                                            {
                                                lastPart = (lastPart.Replace("  ", "|").Split('|')).First().Trim();
                                            }

                                            if ((firstPart.ToLower() != "" && lastPart.ToLower() != "") && (firstPart.ToLower() != "artist" && lastPart.ToLower() != "song"))
                                            {
                                                returnItem.Songs.Add(new ArmaAISongDataItem()
                                                {
                                                    artistName = firstPart,
                                                    songName = lastPart
                                                });

                                                tempSongs.Add(songToAdd);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (returnItem.Songs.Count >= 6)
                        {
                            returnItem.Rerun = false;
                        }
                        else
                        {
                            returnItem.Songs = new List<ArmaAISongDataItem>();
                        }
                    }
                }
            }

            return returnItem;
        }

        public List<TrackDataItem> Tracks_GetTop50Songs()
        {
            return _dapper.GetList<TrackDataItem>("radioconn", "Tracks_GetTop50UserFavorites");
        }

        public Guid? Tracks_CacheTop100LastFMTrending()
        {
            return _dapper.GetFirstOrDefault<Guid?>("recommendations", "arma_get_top_played_songs", null, 120);
        }

        public List<TrackDataItem> Tracks_GetTop100LastFMTrending(Guid requestId)
        {
            return _dapper.GetList<TrackDataItem>("radioconn", "Arma_GetTopLastFMTrendingTracks", new
            {
                request_id = requestId
            });
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
                                            tid = key_id.Value,
                                            usability_score = rankId
                                        });
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                }
                            }
                        }
                    }

                    Task.Delay(TimeSpan.FromSeconds(1))
                        .ContinueWith(_ => {
                            using (var con = _dapper.GetConnection("radioconn"))
                            {
                                int keyId = returnItem[0].tid;

                                foreach (var song in returnItem)
                                {
                                    _dapper.ExecuteNonQuery(con, "Top100_InsertSongData", new
                                    {
                                        key_id = keyId,
                                        rank = song.usability_score,
                                        artist = song.artist_name,
                                        song = song.track_name
                                    });
                                }

                                _dapper.ExecuteNonQuery(con, "Top100_BackupOldSongs", new
                                {
                                    key_id = keyId
                                });
                            }
                        });                    
                }
            }

            return returnItem;
        }

        public List<TrackDataItem> GetCurrentLatinTop50()
        {
            List<TrackDataItem> returnItem = new List<TrackDataItem>();

            string url = "https://www.billboard.com/charts/latin-songs/";

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

                        var allDivs = document.QuerySelectorAll("div.o-chart-results-list-row-container");

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
                                    tid = 0,
                                    usability_score = rankId
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

        public List<TrackDataItem> GetCurrentTopDanceElectronic()
        {
            List<TrackDataItem> returnItem = new List<TrackDataItem>();

            string url = "https://www.billboard.com/charts/dance-electronic-songs/";

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

                        var allDivs = document.QuerySelectorAll("div.o-chart-results-list-row-container");

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
                                    tid = 0,
                                    usability_score = rankId
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

        public List<TrackDataItem> GetCurrentTopRockAlternative()
        {
            List<TrackDataItem> returnItem = new List<TrackDataItem>();

            string url = "https://www.billboard.com/charts/rock-songs/";

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

                        var allDivs = document.QuerySelectorAll("div.o-chart-results-list-row-container");

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
                                    tid = 0,
                                    usability_score = rankId
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

        public List<TrackDataItem> GetTopEmergingArtists()
        {
            List<TrackDataItem> returnItem = new List<TrackDataItem>();

            string url = "https://www.billboard.com/charts/emerging-artists/";

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

                        var allDivs = document.QuerySelectorAll("div.o-chart-results-list-row-container");

                        int rankId = 0;

                        foreach (var div in allDivs)
                        {
                            rankId++;

                            try
                            {
                                var dataHolder = div.QuerySelector("ul:nth-child(1)").QuerySelector("li:nth-child(4)").QuerySelector("li:nth-child(1)");
                                string artistName = (dataHolder.QuerySelector("h3").Text() ?? "").Trim();
                                string songName = (dataHolder.QuerySelector("span") != null ? dataHolder.QuerySelector("span").Text() ?? "" : "").Trim();

                                returnItem.Add(new TrackDataItem()
                                {
                                    artist_name = artistName,
                                    track_name = songName,
                                    tid = 0,
                                    usability_score = rankId
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

        public List<TrackDataItem> GetCurrentTopCountrySongs()
        {
            List<TrackDataItem> returnItem = new List<TrackDataItem>();

            string url = "https://www.billboard.com/charts/country-songs/";

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

                        var allDivs = document.QuerySelectorAll("div.o-chart-results-list-row-container");

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
                                    tid = 0,
                                    usability_score = rankId
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

        public List<TrackDataItem> GetCurrentTopRegionalMexicanoSongs()
        {
            List<TrackDataItem> returnItem = new List<TrackDataItem>();

            string url = "https://www.billboard.com/charts/latin-regional-mexican-airplay/";

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

                        var allDivs = document.QuerySelectorAll("div.o-chart-results-list-row-container");

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
                                    tid = 0,
                                    usability_score = rankId
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
        public List<TrackDataItem> GetTopUserRankedArtists5stars()
        {
            List<TrackDataItem> returnItem = _dapper.GetList<TrackDataItem>("radioconn", "Arma_GetTopUserRankedArtists5range");

            return returnItem;
        }

        public List<TrackDataItem> GetTopUserRankedArtists4stars()
        {
            List<TrackDataItem> returnItem = _dapper.GetList<TrackDataItem>("radioconn", "Arma_GetTopUserRankedArtists4range");

            return returnItem;
        }


        public List<ProxySocks4DataItem> GetSocks4ProxyList()
        {
            ProxySocks4Request requestItem = null;
            List<ProxySocks4DataItem> returnItem = new List<ProxySocks4DataItem>();

            string url = $"https://api.proxyscrape.com/v4/free-proxy-list/get?request=get_proxies&skip=0&proxy_format=protocolipport&format=json&limit=25&protocol=socks4";

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
                        requestItem = Newtonsoft.Json.JsonConvert.DeserializeObject<ProxySocks4Request>(htmlContent);

                        if (requestItem != null && requestItem.proxies != null && requestItem.proxies.Count > 0)
                        {
                            returnItem = requestItem.proxies;
                        }
                    }
                }
            }

            return returnItem;
        }

        public List<AdaptiveFormatDataItem> GetAudioStreams(string VideoId)
        {
            string fullVideoUrl = $"https://www.youtube.com/watch?v={(VideoId ?? "").Trim()}";
            List<AdaptiveFormatDataItem> returnItem = new List<AdaptiveFormatDataItem>();


            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(fullVideoUrl);
            request.UserAgent = "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko; Google Page Speed Insights) Chrome/119.0.0.0 Safari/537.36";

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
                        var match = Regex.Match(htmlContent, @"ytInitialPlayerResponse\s*=\s*(\{.+?\});");
                        if (match.Success)
                        {
                            var jsonStr = match.Groups[1].Value;
                            AudioStreamsRequest streamRequest = Newtonsoft.Json.JsonConvert.DeserializeObject<AudioStreamsRequest>(jsonStr);

                            if (streamRequest != null && streamRequest.streamingData != null && streamRequest.streamingData.adaptiveFormats != null && streamRequest.streamingData.adaptiveFormats.Count > 0)
                            {
                                returnItem = streamRequest.streamingData.adaptiveFormats.Where(st => (st.mimeType ?? "").ToLower().Contains("audio")).ToList();

                                foreach (var stream in returnItem)
                                {
                                    List<string> mimeParts = stream.mimeType.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList();

                                    stream.mimeTypeSimple = mimeParts[0].Trim();
                                    stream.containerName = stream.mimeTypeSimple.Split('/').Last();
                                    stream.codec = (mimeParts.Count > 1 ? (mimeParts[1].Trim().Split('=').Last().Trim().TrimStart('"').TrimEnd('"')) : "");
                                    stream.streamUrl = DecodeSignatureCipher(stream.signatureCipher);
                                }
                            }
                        }                        
                    }
                }
            }

            return returnItem;

            //List<ProxySocks4DataItem> proxies = GetSocks4ProxyList();

            //foreach (var proxy in proxies)
            //{
            //    try
            //    {
            //var proxyUri = new Uri(proxy.proxy);

            //var httpClientHandler = new SocketsHttpHandler
            //{
            //    Proxy = new WebProxy(proxyUri),
            //    UseProxy = true,
            //    ConnectTimeout = TimeSpan.FromSeconds(5)
            //};

            //using (var client = new HttpClient(httpClientHandler))
            //{
            //            var response = client.GetStringAsync(fullVideoUrl).Result;

            //            var match = Regex.Match(response, @"ytInitialPlayerResponse\s*=\s*(\{.+?\});");
            //            if (!match.Success)
            //            {
            //                Console.WriteLine("Failed to find ytInitialPlayerResponse");
            //                return;
            //            }

            //            var jsonStr = match.Groups[1].Value;
            //            var json = JObject.Parse(jsonStr);

            //            var streamingData = json["streamingData"];
            //            if (streamingData == null)
            //            {
            //                Console.WriteLine("Failed to find streaming data");
            //                return;
            //            }

            //            var adaptiveFormats = streamingData["adaptiveFormats"] as JArray;
            //            if (adaptiveFormats == null)
            //            {
            //                Console.WriteLine("Failed to find adaptive formats");
            //                return;
            //            }

            //            foreach (var format in adaptiveFormats)
            //            {
            //                var mimeType = format["mimeType"].ToString();
            //                if (mimeType.StartsWith("audio/"))
            //                {
            //                    var url = format["url"].ToString();
            //                    var bitrate = format["bitrate"].ToString();
            //                    Console.WriteLine($"Audio stream: {mimeType}, Bitrate: {bitrate}");
            //                    Console.WriteLine($"URL: {url}\n");
            //                }
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {

            //    }
            //}
        }

        private string DecodeSignatureCipher(string signatureCipher)
        {
            var components = HttpUtility.ParseQueryString(signatureCipher);
            var encodedUrl = components["url"];
            var decodedUrl = HttpUtility.UrlDecode(encodedUrl);
            var signature = components["s"];

            return $"{decodedUrl}&sig={signature}";
        }

        public async Task DownloadMp4File(string url, string endFileName)
        {
            await _armaYTDownloader.DownloadAudioFileAsync(url, endFileName);

            //bool isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

            //var youtubeDl = new YoutubeDL
            //{
            //    YoutubeDLPath = (isLinux ? "/usr/bin/yt-dlp" : "C:\\YTDL\\yt-dlp.exe"),
            //    FFmpegPath = (isLinux ? "/usr/bin/ffmpeg" : "C:\\ffmpeg\\ffmpeg.exe")
            //};

            //var options = new OptionSet
            //{
            //    Format = "bestaudio[ext=m4a]/bestaudio", // Prefer m4a, but fall back to best audio if not available
            //    Output = endFileName,
            //    ExtractAudio = true,
            //    AudioFormat = AudioConversionFormat.M4a,
            //    NoPlaylist = true,
            //    NoCheckCertificates = true,
            //    NoWarnings = true,
            //    //HlsPreferFfmpeg = true,
            //    //PostprocessorArgs = "-strict -2",
            //    Downloader = "native",
            //    //DownloaderArgs = "native:buffer_size=16k"
            //};

            ////var options = new OptionSet
            ////{
            ////    Format = "bestaudio",          // Download best available audio
            ////    Output = endFileName, //$"{outputDirectory}/%(title)s.%(ext)s",  // Specify output file format
            ////    ExtractAudio = true,           // Extract audio only
            ////    AudioFormat = AudioConversionFormat.M4a,
            ////    PostprocessorArgs = "'ba[ext=m4a]' --no-check-certificates --no-warnings -strict -2"
            ////};


            //var result = youtubeDl.RunAudioDownload(
            //        url,
            //        AudioConversionFormat.M4a,
            //        progress: null,
            //        output: null,
            //        overrideOptions: options
            //    ).Result;

            //if (!result.Success)
            //{
            //    throw new Exception((result.ErrorOutput != null && result.ErrorOutput.Length > 0 ? string.Join("; ", result.ErrorOutput) : "An error occurred"));
            //}

            //List<ProxySocks4DataItem> proxies = GetSocks4ProxyList();

            //foreach (var proxy in proxies)
            //{
            //    try
            //    {
            //        var proxyUri = new Uri(proxy.proxy);

            //        var httpClientHandler = new SocketsHttpHandler
            //        {
            //            Proxy = new WebProxy(proxyUri),
            //            UseProxy = true,
            //            ConnectTimeout = TimeSpan.FromSeconds(5)
            //        };

            //        using (var httpClient = new HttpClient(httpClientHandler))
            //        {
            //            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
            //            using (var response = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result)
            //            {
            //                if (response.IsSuccessStatusCode)
            //                {
            //                    using (var fileStream = new FileStream(endFileName, FileMode.Create, FileAccess.Write, FileShare.None))
            //                    {
            //                        using (var downloadStream = response.Content.ReadAsStreamAsync().Result)
            //                        {
            //                            downloadStream.CopyToAsync(fileStream).Wait();

            //                            break;
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {

            //    }
            //}




            //using (var httpClient = new HttpClient())
            //{
            //    try
            //    {
            //        httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
            //        using (var response = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result)
            //        {
            //            if (response.IsSuccessStatusCode)
            //            {
            //                using (var fileStream = new FileStream(endFileName, FileMode.Create, FileAccess.Write, FileShare.None))
            //                {
            //                    using (var downloadStream = response.Content.ReadAsStreamAsync().Result)
            //                    {
            //                        downloadStream.CopyToAsync(fileStream).Wait();
            //                    }
            //                }
            //            }
            //        }
            //    }
            //    catch (HttpRequestException e)
            //    {
            //        Console.WriteLine($"Error downloading video: {e.Message}");
            //    }
            //    catch (IOException e)
            //    {
            //        Console.WriteLine($"Error saving video: {e.Message}");
            //    }
            //}
        }

        public void FlagFileForDeletion(string FullDirFIleName)
        {
            Task.Delay(TimeSpan.FromHours(1.5))
                .ContinueWith(_ =>
                {
                    if (System.IO.File.Exists(FullDirFIleName))
                    {
                        System.IO.File.Delete(FullDirFIleName);
                    }
                });
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

        public string GetSharedPlaylistToken(int PlaylistId, string UserId)
        {
            return _dapper.GetList<string>("radioconn", "ArmaPlayList_GetSharedPlaylistToken", new
            {
                playlist_id = PlaylistId,
                user_id = UserId
            }).FirstOrDefault();
        }

        public ArmaSharedPlaylistDataItem GetSharedPlaylist(string Token)
        {
            ArmaSharedPlaylistDataItem returnItem = null;

            using (var con = _dapper.GetConnection("radioconn"))
            {
                var qParams = new DynamicParameters();
                qParams.Add("@token", Token);

                var currentDataset = con.QueryMultiple("ArmaPlayList_GetSharedPlaylistByToken",
                    qParams,
                    commandType: CommandType.StoredProcedure);

                returnItem = currentDataset.Read<ArmaSharedPlaylistDataItem>().FirstOrDefault();

                if (returnItem != null)
                {
                    returnItem.PlaylistData = currentDataset.Read<ArmaPlaylistDataItem>().ToList() ?? new List<ArmaPlaylistDataItem>();
                }
            }

            return returnItem;
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
