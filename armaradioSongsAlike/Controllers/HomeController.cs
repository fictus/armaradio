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
        public IActionResult GetSongsLike([FromBody] SongAlikeRequest value)
        {
            try
            {
                if (value == null)
                {
                    throw new Exception("Invalid Request");
                }

                string hrePlaylistfUrl = "";
                string newUrl = $"https://open.spotify.com/search/{Uri.EscapeUriString(value.ArtistName)}/playlists";
                _page.GoToAsync(newUrl).Wait();

                _page.WaitForSelectorAsync("#searchPage").Wait();

                //var mainResultsPane = _page.QuerySelectorAsync("main[aria-label='Spotify - Search']").Result;

                //while (mainResultsPane == null)
                //{
                //    mainResultsPane = _page.QuerySelectorAsync("main[aria-label='Spotify - Search']").Result;
                //}

                var chartHolder = _page.QuerySelectorAsync("#searchPage").Result;

                if (chartHolder != null)
                {
                    _page.WaitForSelectorAsync("div[data-testid='grid-container']").Wait();
                    var firstChildDiv = chartHolder.QuerySelectorAllAsync("div[data-testid='grid-container']").Result.FirstOrDefault();

                    if (firstChildDiv != null)
                    {
                        var nextLayerChild = firstChildDiv.QuerySelectorAllAsync("div").Result.FirstOrDefault();

                        if (nextLayerChild != null)
                        {
                            _page.WaitForSelectorAsync("div[data-testid='infinite-scroll-list']").Wait();
                            var infiniteScrollDiv = nextLayerChild.QuerySelectorAllAsync("div[data-testid='infinite-scroll-list']").Result.FirstOrDefault();

                            if (infiniteScrollDiv != null)
                            {
                                var resultsContainer = infiniteScrollDiv.QuerySelectorAllAsync("div[data-testid='grid-container']").Result.FirstOrDefault();

                                if (resultsContainer != null)
                                {
                                    _page.WaitForSelectorAsync("div[data-encore-id='card'][role='group']").Wait();
                                    var allDivs = resultsContainer.QuerySelectorAllAsync("div[data-encore-id='card'][role='group']").Result;

                                    if (allDivs != null)
                                    {
                                        
                                        foreach (var div in allDivs)
                                        {
                                            try
                                            {
                                                var alink = div.QuerySelectorAsync("a").Result;

                                                if (alink != null)
                                                {
                                                    string linkText = _page.EvaluateFunctionAsync<string>("(element) => element.textContent", alink).Result;
                                                    
                                                    if (linkText.ToLower().EndsWith(" radio"))
                                                    {
                                                        hrePlaylistfUrl = _page.EvaluateFunctionAsync<string>("(element) => element.getAttribute('href')", alink).Result;

                                                        break;
                                                    }
                                                }

                                                //var dataHolder = div.QuerySelector("ul:nth-child(1)").QuerySelector("li:nth-child(4)").QuerySelector("li:nth-child(1)");
                                                //string songName = (dataHolder.QuerySelector("h3").Text() ?? "").Trim();
                                                //string artistName = (dataHolder.QuerySelector("span").Text() ?? "").Trim();

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

                if (!string.IsNullOrWhiteSpace(hrePlaylistfUrl))
                {
                    _page.GoToAsync($"https://open.spotify.com{hrePlaylistfUrl}").Wait();

                    _page.WaitForSelectorAsync("section[data-testid='playlist-page']").Wait();

                    var gridHolder = _page.QuerySelectorAsync("div[role='grid'][data-testid='playlist-tracklist']").Result;


                    _page.WaitForSelectorAsync("div[role='presentation']").Wait();
                    var rowsHolder = gridHolder.QuerySelectorAsync("div[role='presentation']").Result;

                    var allRows = rowsHolder.QuerySelectorAllAsync("div[role='row']").Result;

                    foreach (var row in allRows)
                    {
                        var currentRow = row;
                    }
                }

                //var response = await _page.EvaluateFunctionAsync(@"(artistName) => { 
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
                //            }),
                //        };

                //        fetch(""https://api.spotalike.com/v1/playlists"", fetchBody)
                //            .then(function (response) {
                //                if (response.status >= 500) {
                //                    return response.text();
                //                } else {
                //                    return response.json().catch(function () {
                //                        resolve(""{}"");
                //                    });
                //                }
                //            })
                //            .then(function (jsonData) {
                //                resolve(JSON.stringify(jsonData));
                //                //d.resolve(jsonData);
                //            })
                //            .catch(function (error) {
                //                resolve(error.message);
                //                //d.resolve({ error: true, errorMsg: error.message });
                //            });
                //    });
                
                //}", "Luis Miguel", "6224ac4e-8c97-4005-9e45-6f10ad394404", "La Incondicional");

                //while (response == null)
                //{
                //    Thread.Sleep(5);
                //}

                //var result = response;

                //SongsAlikeResponse responseItem = Newtonsoft.Json.JsonConvert.DeserializeObject<SongsAlikeResponse>(result.ToObject<string>()); //result.Value<JObject>().ToObject<SongsAlikeResponse>();
                //SongsAlikeResponse responseItem =
                    //Newtonsoft.Json.JsonConvert.DeserializeObject<SongsAlikeResponse>(result.Value<string>) //result.Value<JObject>().ToObject<SongsAlikeResponse>();

                return new JsonResult(Ok());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
