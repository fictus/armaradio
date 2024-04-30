using armaradioSongsAlike.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using PuppeteerSharp;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;

namespace armaradioSongsAlike.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IPage _page;

        public HomeController(
            ILogger<HomeController> logger,
            IPage page
        )
        {
            _logger = logger;
            _page = page;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetSongsLike([FromBody] SongAlikeRequest value)
        {
            try
            {
                if (value == null)
                {
                    throw new Exception("Invalid Request");
                }

                var response = await _page.EvaluateFunctionAsync(@"(artistName, artistId, songName) => { 
                    return new Promise((resolve, reject) => {
                        let fetchBody = {
                            method: ""POST"", // *GET, POST, PUT, DELETE, etc.
                            mode: ""cors"", // no-cors, *cors, same-origin
                            cache: ""no-cache"", // *default, no-cache, reload, force-cache, only-if-cached
                            credentials: ""same-origin"", // include, *same-origin, omit
                            headers: {
                                ""Content-Type"": ""application/json"",
                                ""Accept-Encoding"": ""gzip, deflate""
                                // 'Content-Type': 'application/x-www-form-urlencoded',
                            },
                            redirect: ""follow"", // manual, *follow, error
                            referrerPolicy: ""no-referrer"", // no-referrer, *no-referrer-when-downgrade, origin, origin-when-cross-origin, same-origin, strict-origin, strict-origin-when-cross-origin, unsafe-url
                            body: JSON.stringify({
                                artist: artistName,
                                mbid: artistId,
                                track: songName
                            }),
                        };

                        fetch(""https://api.spotalike.com/v1/playlists"", fetchBody)
                            .then(function (response) {
                                if (response.status >= 500) {
                                    return response.text();
                                } else {
                                    return response.json().catch(function () {
                                        resolve(""{}"");
                                    });
                                }
                            })
                            .then(function (jsonData) {
                                resolve(JSON.stringify(jsonData));
                                //d.resolve(jsonData);
                            })
                            .catch(function (error) {
                                resolve(error.message);
                                //d.resolve({ error: true, errorMsg: error.message });
                            });
                    });
                
                }", "Luis Miguel", "6224ac4e-8c97-4005-9e45-6f10ad394404", "La Incondicional");

                while (response == null)
                {
                    Thread.Sleep(5);
                }

                var result = response;

                SongsAlikeResponse responseItem = Newtonsoft.Json.JsonConvert.DeserializeObject<SongsAlikeResponse>(result.ToObject<string>()); //result.Value<JObject>().ToObject<SongsAlikeResponse>();
                //SongsAlikeResponse responseItem =
                    //Newtonsoft.Json.JsonConvert.DeserializeObject<SongsAlikeResponse>(result.Value<string>) //result.Value<JObject>().ToObject<SongsAlikeResponse>();

                return new JsonResult(responseItem);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
