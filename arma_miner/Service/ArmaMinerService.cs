using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using arma_miner.Data;
using arma_miner.Models;
using Microsoft.Extensions.Hosting;
using SharpCompress.Common;
using SharpCompress.Compressors.Xz;
using SharpCompress.Readers;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace arma_miner.Service
{
    public class ArmaMinerService : IArmaMinerService
    {
        private readonly bool IsLinux = false;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly IDapperHelper _dapper;
        private readonly IConfiguration _configuration;
        public ArmaMinerService(
            IHostEnvironment hostEnvironment,
            IDapperHelper dapper,
            IConfiguration configuration
        )
        {
            _hostEnvironment = hostEnvironment;
            _dapper = dapper;
            _configuration = configuration;

            IsLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        }

        public void RunUpdateRoutine()
        {
            VersionDataItem siteVersion = GetLatestVersion();

            if (siteVersion != null)
            {
                bool versionHasBeenProcessed = _dapper.GetFirstOrDefault<bool>("radioconn", "Operations_Sync_CheckIfVersionHasBeenProcessed", new
                {
                    version_number = siteVersion.Version
                });

                if (!versionHasBeenProcessed)
                {
                    _dapper.ExecuteNonQuery("radioconn", "Operations_Sync_AddVersionToStaging", new
                    {
                        version_number = siteVersion.Version
                    });

                    string tempFilesDir = EmptyFilesFromTempFolder();
                    ProcessArtistFile(siteVersion.ArtistsFileUrl, tempFilesDir);




                    _dapper.ExecuteNonQuery("radioconn", "Operations_Sync_AddVersionToCompleted", new
                    {
                        version_number = siteVersion.Version,
                        errors_occurred = false
                    });
                }
            }
        }

        private VersionDataItem GetLatestVersion()
        {
            VersionDataItem returnItem = null;
            string url = "https://data.metabrainz.org/pub/musicbrainz/data/json-dumps/";

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

                        var allLinks = document.QuerySelectorAll("a");

                        foreach (var link in allLinks)
                        {
                            try
                            {
                                if ((link.Text() ?? "").Trim().StartsWith("latest-is-"))
                                {
                                    string version = link.Text().Trim().Replace("latest-is-", "");

                                    returnItem = new VersionDataItem()
                                    {
                                        Version = version,
                                        ArtistsFileUrl = $"https://data.metabrainz.org/pub/musicbrainz/data/json-dumps/{version}/artist.tar.xz",
                                        AlbumsFileUrl = $"https://data.metabrainz.org/pub/musicbrainz/data/json-dumps/{version}/release.tar.xz"
                                    };

                                    break;
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                }
            }

            return returnItem;
        }

        private string EmptyFilesFromTempFolder()
        {
            string rootPath = _hostEnvironment.ContentRootPath.TrimEnd('/').TrimEnd('\\');
            string tempFiles = (IsLinux ? $"{rootPath}/tempFiles/" : $"{rootPath}\\tempFiles\\");

            if (!System.IO.Directory.Exists(tempFiles))
            {
               System.IO.Directory.CreateDirectory(tempFiles);
            }

            //var file = Directory.EnumerateFiles(tempFiles, "*")
            //    .FirstOrDefault();

            //while (file != null)
            //{
            //    System.IO.File.Delete(file);

            //    file = Directory.EnumerateFiles(tempFiles, "*")
            //        .FirstOrDefault();
            //}

            return tempFiles;
        }

        private void ProcessArtistFile(string Url, string tempFilesDir)
        {
            //Url = "https://data.metabrainz.org/pub/musicbrainz/data/json-dumps/20240420-001001/series.tar.xz";
            string artistFile = $"{tempFilesDir}artist.tar.xz";

            // using (WebClient webClient = new WebClient())
            // {
            //    webClient.Headers.Add("Accept: text/html, application/xhtml+xml, */*");
            //    webClient.Headers.Add("User-Agent: Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)");
            //    webClient.DownloadFile(new Uri(Url), artistFile);
            // }

            // if (File.Exists(artistFile))
            // {
            //    using (var fileStream  = File.OpenRead(artistFile))
            //    using (IReader reader = ReaderFactory.Open(fileStream))
            //    {
            //        while (reader.MoveToNextEntry())
            //        {
            //            if (reader.Entry.Key.EndsWith("artist"))
            //            {
            //                reader.WriteEntryToDirectory(tempFilesDir, new SharpCompress.Common.ExtractionOptions()
            //                {
            //                    ExtractFullPath = true,
            //                    Overwrite = true
            //                });
            //            }
            //        }
            //    }                
            // }

            string artistFileFull = (IsLinux ? $"{tempFilesDir}mbdump/artist" : $"{tempFilesDir}mbdump\\artist");
            bool artistExists = false;
            int newArtistsCount = 0;

            using (FileStream fs = new FileStream(artistFileFull, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                // Start reading from the end of the file
                fs.Seek(0, SeekOrigin.End);

                // Read the file stream backward
                long position = fs.Position;
                byte[] buffer = new byte[1024];
                StringBuilder sb = new StringBuilder();

                while (!artistExists && position > 0)
                {
                    fs.Seek(-Math.Min(position, buffer.Length), SeekOrigin.Current);

                    int bytesRead = fs.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0)
                        break;

                    for (int i = bytesRead - 1; i >= 0; i--)
                    {
                        if (buffer[i] == '\n')
                        {
                            // Process the line
                            string line = sb.ToString();

                            MBArtistParseDataItem artistItem = Newtonsoft.Json.JsonConvert.DeserializeObject<MBArtistParseDataItem>(line);

                            if (artistItem != null)
                            {
                                artistExists = _dapper.GetFirstOrDefault<bool>("radioconn", "Operations_CheckIfMBArtistIdExists", new
                                {
                                    mb_artistid = artistItem.id
                                });

                                if (!artistExists)
                                {
                                    SaveMBArtist(artistItem);
                                    newArtistsCount++;
                                }
                            }

                            sb.Clear();
                        }
                        else
                        {
                            sb.Insert(0, (char)buffer[i]);
                        }
                    }

                    position -= bytesRead;
                    fs.Seek(-bytesRead, SeekOrigin.Current);
                }

                // Process the first line if any
                if (position <= 0 && sb.Length > 0)
                {
                    string line = sb.ToString();

                    MBArtistParseDataItem artistItem = Newtonsoft.Json.JsonConvert.DeserializeObject<MBArtistParseDataItem>(line);

                    if (artistItem != null)
                    {
                        artistExists = _dapper.GetFirstOrDefault<bool>("radioconn", "Operations_CheckIfMBArtistIdExists", new
                        {
                            mb_artistid = artistItem.id
                        });

                        if (!artistExists)
                        {
                            SaveMBArtist(artistItem);
                            newArtistsCount++;
                        }
                    }
                }
            }

            int finalCount = newArtistsCount;
        }

        private void SaveMBArtist(MBArtistParseDataItem artistItem)
        {
            if (artistItem != null)
            {
                using (var con = _dapper.GetConnection("radioconn"))
                {
                    int? newId = _dapper.GetFirstOrDefault<int?>(con, "Operations_MBInsertArtistWithGenres", new
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
                            _dapper.ExecuteNonQuery(con, "Operations_MBInsertArtistGenre", new
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
    }
}
