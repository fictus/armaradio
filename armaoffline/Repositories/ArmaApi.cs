using armaoffline.Models;
using armaoffline.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = client.PostAsync("https://armarad.com/Api/ExternalLogin", content).Result;

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

                var response = client.GetAsync("https://armarad.com/Api/GetUserPlaylists").Result;

                if (response != null && response.IsSuccessStatusCode)
                {
                    var responseString = response.Content.ReadAsStringAsync().Result;
                    returnItem = JsonConvert.DeserializeObject<List<ArmaUserPlaylistDataItem>>(responseString);
                }
            }

            return returnItem;
        }
    }
}
