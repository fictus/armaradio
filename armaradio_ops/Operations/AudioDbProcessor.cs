using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using armaradio_ops.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace armaradio_ops.Operations
{
    public class AudioDbProcessor
    {
        //public void ProcessArtists(int? artistId = null)
        //{
        //    List<AudioDbArtistDataItem> artists = new List<AudioDbArtistDataItem>();

        //    for (int i = (artistId.HasValue ? artistId.Value : 0); i <= Int32.MaxValue; i++)
        //    {
        //        //i = 112024;
        //        HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"https://www.theaudiodb.com/api/v1/json/523532/artist.php?i={i}");
        //        request.UserAgent = "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko; Google Page Speed Insights) Chrome/27.0.1453 Safari/537.36";

        //        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        //        {
        //            if (response.StatusCode == HttpStatusCode.OK)
        //            {
        //                string htmlContent;
        //                using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("iso-8859-1")))
        //                {
        //                    htmlContent = sr.ReadToEnd();
        //                }

        //                if (!string.IsNullOrEmpty(htmlContent))
        //                {
        //                    AudioDbArtistRequest foundArtist = Newtonsoft.Json.JsonConvert.DeserializeObject<AudioDbArtistRequest>(htmlContent);

        //                    if (foundArtist != null && foundArtist.artists != null && foundArtist.artists.Count > 0)
        //                    {
        //                        SaveArtists(foundArtist.artists);
        //                        artists.AddRange(foundArtist.artists);
        //                    }
        //                }
        //            }
        //        }

        //        System.Threading.Thread.Sleep(500);
        //    }


        //}

        public void SaveArtistsFromJsonFile()
        {
            using (System.IO.Stream fs = new FileStream("C:\\Users\\19039\\Downloads\\musicbrainzJson\\artist.json", FileMode.Open, FileAccess.Read))
            using (StreamReader streamReader = new StreamReader(fs))
            {
                while (!streamReader.EndOfStream)
                {
                    string result = streamReader.ReadLine();

                    if (!string.IsNullOrWhiteSpace(result))
                    {
                        MBArtistParseDataItem artistItem = Newtonsoft.Json.JsonConvert.DeserializeObject<MBArtistParseDataItem>(result);

                        if (artistItem != null)
                        {
                            //string artistName = artistItem.name.Trim();
                            //string firstLetter = (artistName.Length > 0 ? artistName[0].ToString() : "").ToLower();

                            //if (!string.IsNullOrWhiteSpace(firstLetter))
                            //{
                            //    if (Int32.TryParse(firstLetter, out int _isNumb))
                            //    {
                            //        artistItem.dbsource = "num";
                            //    }
                            //    else
                            //    {
                            //        if (IsBasicLetter(Convert.ToChar(firstLetter)))
                            //        {
                            //            artistItem.dbsource = firstLetter;
                            //        }
                            //        else
                            //        {
                            //            artistItem.dbsource = "symb";
                            //        }
                            //    }
                            //}

                            SaveMBArtist(artistItem);
                        }
                    }
                }
            }
        }

        private void SaveMBArtist(MBArtistParseDataItem artistItem)
        {
            if (artistItem != null)
            {
                using (var con = armaradio_ops.Data._SqlHelper.GetConnection(""))
                {
                    int? newId = Data._SqlHelper.GetFirstOrDefault<int?>(con, "Operations_MBInsertArtistWithGenres", new
                    {
                        mb_id = artistItem.id,
                        name = artistItem.name,
                        sort_name = artistItem.sortname,
                        country = artistItem.country,
                        type = artistItem.type,
                        type_id = artistItem.typeid,
                        rating_value = artistItem.rating?.value,
                        rating_votes = artistItem.rating?.votescount,
                        lifespan_begin = artistItem.lifespan?.begin,
                        lifespan_end = artistItem.lifespan?.end,
                        lifespan_ended = artistItem.lifespan?.ended
                    });

                    if (newId.HasValue && artistItem.genres != null && artistItem.genres.Count > 0)
                    {
                        foreach (var genre in artistItem.genres)
                        {
                            Data._SqlHelper.ExecuteNonQuery(con, "Operations_MBInsertArtistGenre", new
                            {
                                artist_id = newId.Value,
                                mb_id = genre.id,
                                genre_name = genre.name,
                                count = genre.count,
                                disambiguation = genre.disambiguation
                            });
                        }
                    }
                }
            }
        }

        private void SaveArtists(List<AudioDbArtistDataItem> artists)
        {
            using (var con = armaradio_ops.Data._SqlHelper.GetConnection(""))
            {
                foreach (var artist in artists)
                {
                    armaradio_ops.Data._SqlHelper.ExecuteNonQuery(con, "AudioDb_InsertArtist", new
                    {
                        idArtist = artist.idArtist,
                        strArtist = artist.strArtist,
                        strArtistStripped = artist.strArtistStripped,
                        strArtistAlternate = artist.strArtistAlternate,
                        strLabel = artist.strLabel,
                        idLabel = artist.idLabel,
                        intFormedYear = artist.intFormedYear,
                        intDiedYear = artist.intDiedYear,
                        strStyle = artist.strStyle,
                        strGenre = artist.strGenre,
                        strMood = artist.strMood,
                        strGender = artist.strGender,
                        strCountry = artist.strCountry,
                        strCountryCode = artist.strCountryCode,
                        strArtistThumb = artist.strArtistThumb,
                        strArtistLogo = artist.strArtistLogo,
                        strArtistCutout = artist.strArtistCutout,
                        strArtistClearart = artist.strArtistClearart,
                        strArtistWideThumb = artist.strArtistWideThumb,
                        strArtistFanart = artist.strArtistFanart,
                        strArtistFanart2 = artist.strArtistFanart2,
                        strArtistFanart3 = artist.strArtistFanart3,
                        strArtistFanart4 = artist.strArtistFanart4,
                        strArtistBanner = artist.strArtistBanner,
                        strMusicBrainzID = artist.strMusicBrainzID,
                        intCharted = artist.intCharted
                    });
                }
            }
        }

        private bool IsBasicLetter(char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
        }
    }
}
