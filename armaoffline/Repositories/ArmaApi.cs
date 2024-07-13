using armaoffline.Models;
using armaoffline.Services;
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
        public ArmaApi(GlobalState globalState)
        {
            _globalState = globalState;
        }

        public bool Singin(string Email, string Password)
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

                        nowSignedIn = true;
                    }
                }
            }

            return nowSignedIn;
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

        public void GetAudioFile(string VideoId)
        {
            if (!string.IsNullOrWhiteSpace(VideoId) && !CheckIfAudioFileExists($"{VideoId.Trim()}.mp3"))
            {
                byte[] fileBytes = null;
                ApiAudioDetailsDataItem audioDetails = GetAudioFileDetails(VideoId);

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
                    string filePath = Path.Combine(audioFolderPath, $"{VideoId.Trim()}.{(audioDetails != null ? audioDetails.FileExtension : "mp3")}");

                    if (!CheckIfAudioFileExists($"{VideoId.Trim()}.{(audioDetails != null ? audioDetails.FileExtension : "mp3")}"))
                    {
                        File.WriteAllBytesAsync(filePath, fileBytes).Wait();
                    }
                }
            }
        }

        public bool CheckIfAudioFileExists(string VideoId)
        {
            if (!string.IsNullOrWhiteSpace(VideoId))
            {
                string audioFolderPath = Path.Combine(FileSystem.AppDataDirectory, "audio");

                if (!Directory.Exists(audioFolderPath))
                {
                    Directory.CreateDirectory(audioFolderPath);
                }

                string filePath = Path.Combine(audioFolderPath, VideoId.Trim());

                return File.Exists(filePath);
            }

            return false;
        }
    }
}
