using AngleSharp.Html.Parser;
using AngleSharp.Dom;
using armaradio_ops.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace armaradio_ops.Operations
{
    public class AlbumsProcessor
    {
        public void ProcessAlbums(int? artistId = null)
        {
            List<AlbumArtistDataItem> artists = armaradio_ops.Data._SqlHelper.GetList<AlbumArtistDataItem>("", "Operations_GetArtistsFromRawList");

            if (artistId.HasValue)
            {
                artists = artists.Where(ar => ar.Id >= artistId.Value).ToList();
            }

            foreach (var artist in artists)
            {
                List<AlbumRawDataItem> data = GetAlbumsFromAlbumUrl(artist);

                SaveAlbums(data);
            }
        }

        private List<AlbumRawDataItem> GetAlbumsFromAlbumUrl(AlbumArtistDataItem artist)
        {
            List<AlbumRawDataItem> returnItem = new List<AlbumRawDataItem>();

            if (!string.IsNullOrWhiteSpace(artist.AlbumsUrl))
            {
                string url = artist.AlbumsUrl;

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

                                if (name.ToLower() == "albums discography:" || name.ToLower() == "singles discography:")
                                {
                                    bool isSingles = name.ToLower() == "singles discography:";
                                    var tblMain = href.ParentElement.NextElementSibling; //.Closest("table");

                                    if (tblMain != null)
                                    {
                                        var firstMainTr = tblMain.QuerySelectorAll("tr").Eq(0);

                                        if (firstMainTr != null)
                                        {
                                            var albumsTbl = firstMainTr.QuerySelector("table");

                                            if (albumsTbl != null)
                                            {
                                                var allTrs = albumsTbl.QuerySelectorAll("tr");

                                                foreach (var tr in allTrs)
                                                {
                                                    var allTds = tr.QuerySelectorAll("td");
                                                    var firstTd = allTds.Eq(0);

                                                    if (firstTd != null)
                                                    {
                                                        string trPosition = firstTd.Text().Trim();

                                                        if (trPosition != "#")
                                                        {
                                                            var titleHolder = allTds.Eq(1).QuerySelector("b");

                                                            allTds.Eq(1).QuerySelector("b").Remove();

                                                            string albumTitle = (titleHolder != null ? titleHolder.Text().Trim() : "");
                                                            string albumDetails = allTds.Eq(1).Text().Trim();
                                                            string releaseDate = allTds.Eq(2).Text().Trim();
                                                            string label = allTds.Eq(3).Text().Trim();

                                                            if (albumDetails == albumTitle)
                                                            {
                                                                albumDetails = "";
                                                            }

                                                            if (string.IsNullOrWhiteSpace(albumTitle) && !string.IsNullOrWhiteSpace(albumDetails))
                                                            {
                                                                albumTitle = albumDetails;
                                                            }

                                                            returnItem.Add(new AlbumRawDataItem()
                                                            {
                                                                ArtistId = artist.Id,
                                                                AlbumName = albumTitle,
                                                                AlbumDetails = albumDetails,
                                                                ReleaseDate = releaseDate,
                                                                Label = label,
                                                                DBDesignator = artist.DBDesignator,
                                                                IsSingle = isSingles
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
                    }
                }
            }

            return returnItem;
        }

        private void SaveAlbums(List<AlbumRawDataItem> albums)
        {
            using (var con = armaradio_ops.Data._SqlHelper.GetConnection(""))
            {
                foreach (var album in albums)
                {
                    armaradio_ops.Data._SqlHelper.ExecuteNonQuery(con, "Operations_InsertRawAlbum", new
                    {
                        ArtistId = album.ArtistId,
                        DBDesignator = album.DBDesignator,
                        AlbumName = album.AlbumName,
                        AlbumDetails = album.AlbumDetails,
                        ReleaseDate = album.ReleaseDate,
                        Label = album.Label,
                        IsSingle = album.IsSingle
                    });
                }
            }
        }
    }
}
