using arma_genregenerator.Data;
using arma_genregenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace arma_genregenerator.Services
{
    public class ArmaGenresService : IArmaGenresService
    {
        private readonly IDapperHelper _dapper;
        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;

        public ArmaGenresService(
            IDapperHelper dapper,
            Microsoft.Extensions.Configuration.IConfiguration configuration
        )
        {
            _dapper = dapper;
            _configuration = configuration;
        }

        public async Task PopulateArtistGenres()
        {
            List<ArtistDataItem> artistsWitNoGenres = _dapper.GetList<ArtistDataItem>("armaradio", "Genre_GetArtistsRequiringGenres") ?? new List<ArtistDataItem>();

            using (var con = _dapper.GetConnection("armaradio"))
            {
                foreach (var artist in artistsWitNoGenres)
                {
                    try
                    {
                        List<string> genres = GetGenresByArtist(artist.Name) ?? new List<string>();

                        if (genres.Count > 0)
                        {
                            foreach (var genre in genres)
                            {
                                int genreId = _dapper.GetFirstOrDefault<int>(con, "Genre_InsertGenre", new
                                {
                                    genre_name = genre
                                });

                                _dapper.ExecuteNonQuery(con, "Genre_InsertArtistGenre", new
                                {
                                    artist_mbid = artist.artist_mbid,
                                    genre_id = genreId
                                });
                            }
                        }

                        await Task.Delay(1000);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

            await Task.Delay(1);
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

        public string GetApiToken()
        {
            string returnItem = _dapper.GetFirstOrDefault<string>("armaradio", "Operations_GetCachedToken");

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

                    _dapper.ExecuteNonQuery("armaradio", "Operations_SaveApiTokenToCache", new
                    {
                        token = returnItem
                    });
                }
            }

            return returnItem;
        }
    }
}
