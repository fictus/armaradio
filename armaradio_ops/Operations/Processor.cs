using AngleSharp.Html.Parser;
using armaradio_ops.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using AngleSharp.Dom;

namespace armaradio_ops.Operations
{
    public class Processor
    {
        public void StartParsingArtistNames()
        {
            string dbDesignator = "num";
            ArtistParseRequest artists = GetArtistNamesWithLinks($"http://www.umdmusic.com/default.asp?Lang=English&Letter=1&View=I", dbDesignator);
            //http://www.umdmusic.com/default.asp?Lang=English&Letter=A&View=I

            if (artists != null)
            {
                SaveArtistNamesToSql(artists);
            }

            while (artists != null && !string.IsNullOrWhiteSpace(artists.NextPageUrl))
            {
                artists = GetArtistNamesWithLinks(artists.NextPageUrl, dbDesignator);
                SaveArtistNamesToSql(artists);
            }
        }

        private static ArtistParseRequest GetArtistNamesWithLinks(string pageUrl, string dbDesignator)
        {
            if (string.IsNullOrWhiteSpace(pageUrl))
            {
                return null;
            }

            ArtistParseRequest returnItem = null;
            string url = pageUrl;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.UserAgent = "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko; Google Page Speed Insights) Chrome/27.0.1453 Safari/537.36";

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string htmlContent;
                    using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("iso-8859-1")))
                    {
                        htmlContent = sr.ReadToEnd();
                    }

                    if (!string.IsNullOrEmpty(htmlContent))
                    {
                        var parser = new HtmlParser();
                        var document = parser.ParseDocument(htmlContent);

                        var allPageHrefs = document.QuerySelectorAll("a");

                        foreach (var href in allPageHrefs)
                        {
                            string name = href.Text().Trim();

                            if ((dbDesignator != "symb" ? (name == "Next Page") : (name == "$400 SUITS")))
                            {
                                var tblMain = href.Closest("table");

                                if (tblMain != null)
                                {
                                    returnItem = new ArtistParseRequest()
                                    {
                                        artists = new List<ArtistRawDataItem>(),
                                        NextPageUrl = (dbDesignator != "symb" ? $"http://www.umdmusic.com/{href.GetAttribute("href")}" : "")
                                    };

                                    var allHrefs = tblMain.QuerySelectorAll("a");

                                    foreach (var alink in allHrefs)
                                    {
                                        string artistName = alink.Text().Trim();

                                        if (artistName != "")
                                        {
                                            if (dbDesignator == "symb" || !(artistName == "Previous Page" || artistName == "Next Page"))
                                            {
                                                if (!returnItem.artists.Any(ar => ar.Name == artistName && ar.AlbumsUrl == $"http://www.umdmusic.com/{alink.GetAttribute("href")}"))
                                                {
                                                    returnItem.artists.Add(new ArtistRawDataItem()
                                                    {
                                                        Name = artistName,
                                                        AlbumsUrl = $"http://www.umdmusic.com/{alink.GetAttribute("href")}",
                                                        DBDesignator = dbDesignator
                                                    });
                                                }
                                            }
                                        }
                                    }
                                }

                                break;
                            }
                        }

                        if (returnItem == null)
                        {
                            foreach (var href in allPageHrefs)
                            {
                                string name = href.Text().Trim();

                                if (name == "Previous Page")
                                {
                                    var tblMain = href.Closest("table");

                                    if (tblMain != null)
                                    {
                                        returnItem = new ArtistParseRequest()
                                        {
                                            artists = new List<ArtistRawDataItem>(),
                                            NextPageUrl = ""
                                        };

                                        var allHrefs = tblMain.QuerySelectorAll("a");

                                        foreach (var alink in allHrefs)
                                        {
                                            string artistName = alink.Text().Trim();

                                            if (artistName != "")
                                            {
                                                if (!(artistName == "Previous Page" || artistName == "Next Page"))
                                                {
                                                    if (!returnItem.artists.Any(ar => ar.Name == artistName && ar.AlbumsUrl == $"http://www.umdmusic.com/{alink.GetAttribute("href")}"))
                                                    {
                                                        returnItem.artists.Add(new ArtistRawDataItem()
                                                        {
                                                            Name = artistName,
                                                            AlbumsUrl = $"http://www.umdmusic.com/{alink.GetAttribute("href")}",
                                                            DBDesignator = dbDesignator
                                                        });
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    break;
                                }
                            }
                        }
                    }
                }
            }

            return returnItem;
        }

        private void SaveArtistNamesToSql(ArtistParseRequest data)
        {
            if (data != null && data.artists != null && data.artists.Count > 0)
            {
                using (var con = armaradio_ops.Data._SqlHelper.GetConnection(""))
                {
                    foreach (var artist in data.artists)
                    {
                        con.Execute("Operations_InsertArtistToRawList", new
                        {
                            ArtistName = artist.Name,
                            AlbumsUrl = artist.AlbumsUrl,
                            DBDesignator = artist.DBDesignator
                        }, commandType: System.Data.CommandType.StoredProcedure);
                    }
                }
            }
        }
    }
}
