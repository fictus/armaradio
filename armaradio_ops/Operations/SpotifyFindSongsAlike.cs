using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using Newtonsoft.Json;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace armaradio_ops.Operations
{
    public class SpotifyFindSongsAlike
    {

        private IBrowser _browser;

        public async void FindSimilarSong(int artistId)
        {
            string url = "https://localhost:7195/Home/GetSongsLike";

            using (var client = new HttpClient())
            {
                // Serialize the data object to JSON
                var json = JsonConvert.SerializeObject(new
                {
                    ArtistName = "Luis Miguel",
                    MBId = "6224ac4e-8c97-4005-9e45-6f10ad394404",
                    SongName = "La Incondicional"
                });

                // Create the HTTP content from the JSON string
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Send the POST request
                var response = client.PostAsync(url, content).Result;

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content
                    var responseContent = response.Content.ReadAsStringAsync();
                    responseContent.Wait();
                    string contents = responseContent.Result;
                    Console.WriteLine(contents);
                }
                else
                {
                    Console.WriteLine($"Request failed with status code: {response.StatusCode}");
                }
            }

            ////ArmaArtistSimpleDataItem artistInfo = _dapper.GetFirstOrDefault<ArmaArtistSimpleDataItem>("radioconn", "Arma_GetArtistById", new
            ////{
            ////    artist_id = 6954 //artistId
            ////});

            //string url = $"https://spotalike.com/en";

            //var browserFetcher = new BrowserFetcher();
            //browserFetcher.DownloadAsync().Wait();

            //_browser = Puppeteer.LaunchAsync(new LaunchOptions {
            //    Headless = true
            //}).Result;
            //var page = _browser.NewPageAsync().Result;
            //page.GoToAsync(url).Wait();
            ////page.WaitForNavigationAsync().Wait();

            //var result = page.EvaluateFunctionAsync(@"(artistName, artistId, songName) => { 
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

            //}", "Luis Miguel", "6224ac4e-8c97-4005-9e45-6f10ad394404", "La Incondicional").Result;
            //Console.WriteLine(result);


            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //request.UserAgent = "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko; Google Page Speed Insights) Chrome/27.0.1453 Safari/537.36";
            //var config = AngleSharp.Configuration.Default.WithDefaultLoader();
            //var browsingContext = BrowsingContext.New(config);

            //browsingContext.OpenAsync(url).Wait();


            //IHtmlScriptElement script = browsingContext.Active.CreateElement<IHtmlScriptElement>();

            //script.Text = @"
            //    (function () {
            //        console.log('Hello, World!');
            //        var txtSearch = document.getElementById('search');

            //        txtSearch.setAttribute('data-results', 'hello workd');
            //    })();
            //";

            //browsingContext.Active.QuerySelector("body").AppendChild(script);

            //var formOb = browsingContext.Active.QuerySelector<IHtmlFormElement>("form");

            //var searchText = browsingContext.Active.QuerySelector("#search");

            //formOb.QuerySelector("input").SetAttribute("value", "Luis Miguel");

            ////object result = browsingContext.Active.EvaluateScriptAsync("");

            //formOb.Submitted += async (sender, eventArgs) =>
            //{
            //    // eventArgs.Data contains the FormDataSet with the submitted form data
            //    // eventArgs.Response contains the response from the server
            //    string some = "";

            //    // You can access the AJAX response data using eventArgs.Response
            //    //string responseHtml = await eventArgs.Response.ReadHtmlAsync();
            //    //Console.WriteLine(responseHtml);

            //    // Or you can access the raw response bytes using eventArgs.Response.Content
            //    //byte[] responseBytes = await eventArgs.Response.Content.ReadAsByteArrayAsync();
            //    // Process the response bytes as needed
            //};

            //formOb.SubmitAsync().Wait();


            //var chartHolder = browsingContext.Active.Body.InnerHtml;
            //var xs = chartHolder;

            //if (chartHolder != null)
            //{
            //    var firstChildDiv = chartHolder.QuerySelectorAll("div[data-testid='grid-container']").FirstOrDefault();

            //    if (firstChildDiv != null)
            //    {
            //        var nextLayerChild = firstChildDiv.QuerySelectorAll("div").FirstOrDefault();

            //        if (nextLayerChild != null)
            //        {
            //            var infiniteScrollDiv = nextLayerChild.QuerySelectorAll("div[data-testid='infinite-scroll-list']").FirstOrDefault();

            //            if (infiniteScrollDiv != null)
            //            {
            //                var resultsContainer = infiniteScrollDiv.QuerySelectorAll("div[data-testid='grid-container']").FirstOrDefault();

            //                if (resultsContainer != null)
            //                {
            //                    var allDivs = resultsContainer.QuerySelectorAll("div[data-encore-id='card'][role='group']");

            //                    if (allDivs != null)
            //                    {
            //                        foreach (var div in allDivs)
            //                        {
            //                            try
            //                            {
            //                                var dataHolder = div.QuerySelector("ul:nth-child(1)").QuerySelector("li:nth-child(4)").QuerySelector("li:nth-child(1)");
            //                                string songName = (dataHolder.QuerySelector("h3").Text() ?? "").Trim();
            //                                string artistName = (dataHolder.QuerySelector("span").Text() ?? "").Trim();

            //                                //returnItem.Add(new TrackDataItem()
            //                                //{
            //                                //    artist_name = artistName,
            //                                //    track_name = songName,
            //                                //    tid = -1,
            //                                //    usability_score = 0
            //                                //});
            //                            }
            //                            catch (Exception ex)
            //                            {

            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
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
    }
}
