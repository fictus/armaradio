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
    }
}
