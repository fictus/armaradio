using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace armaradio_ops.Operations
{
    public class SpotifyFindSongsAlike
    {
        public void FindSimilarSong(int artistId)
        {
            //ArmaArtistSimpleDataItem artistInfo = _dapper.GetFirstOrDefault<ArmaArtistSimpleDataItem>("radioconn", "Arma_GetArtistById", new
            //{
            //    artist_id = 6954 //artistId
            //});

            string url = $"https://open.spotify.com/search/{Uri.EscapeUriString("Luis Miguel")}/playlists";

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

                        var chartHolder = document.QuerySelector("#searchPage");

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
                    }
                }
            }

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
