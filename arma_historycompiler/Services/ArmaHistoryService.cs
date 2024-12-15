using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using arma_historycompiler.Data;
using arma_historycompiler.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Client;
using SharpCompress.Readers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace arma_historycompiler.Services
{
    public class ArmaHistoryService : IArmaHistoryService
    {
        private readonly bool IsLinux = false;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly IDapperHelper _dapper;
        private readonly IConfiguration _configuration;
        public ArmaHistoryService(
            IHostEnvironment hostEnvironment,
            IDapperHelper dapper,
            IConfiguration configuration
        )
        {
            _hostEnvironment = hostEnvironment;
            _dapper = dapper;
            _configuration = configuration;
        }

        public async Task RunUpdateRoutine()
        {
            VersionDataItem siteVersion = GetLatestVersion();

            if (siteVersion != null)
            {
                //MBSyncQueueDataItem versionHasBeenProcessed = _dapper.GetFirstOrDefault<MBSyncQueueDataItem>("radioconn", "Operations_Sync_CheckIfVersionHasBeenProcessed", new
                //{
                //    version_number = siteVersion.Version
                //});

                MBSyncQueueDataItem versionHasBeenProcessed = new MBSyncQueueDataItem()
                {
                    HasBeenProcessed = false
                };

                if (versionHasBeenProcessed != null && !versionHasBeenProcessed.HasBeenProcessed)
                {
                    //_dapper.ExecuteNonQuery("radioconn", "Operations_Sync_AddVersionToStaging", new
                    //{
                    //    version_number = siteVersion.Version
                    //});

                    string tempFilesDir = EmptyFilesFromTempFolder();

                    DownloadHistoryFile(siteVersion.FileUrl, tempFilesDir, siteVersion.Version);

                    //bool artistErrors = _armaArtistsOps.ProcessArtistFile(siteVersion.ArtistsFileUrl, tempFilesDir, siteVersion.Version, versionHasBeenProcessed.FirstTimeProcess);

                    var listenFiles = Directory.EnumerateFiles(tempFilesDir, "*.listens", SearchOption.AllDirectories).ToList();
                    string listenFile = (listenFiles != null && listenFiles.Count > 0 ? listenFiles[0] : "");
                    List<ArtistDataItem> allArtists = _dapper.GetList<ArtistDataItem>("armaradio", "Operations_GetAllMBArtistIdsWithDBSource");

                    using (var conn = _dapper.GetConnection("radioconn"))
                    {
                        ProcessFile(conn, listenFile, allArtists);
                    }

                    EmptyFilesFromTempFolder();

                    //_dapper.ExecuteNonQuery("radioconn", "Operations_Sync_AddVersionToCompleted", new
                    //{
                    //    version_number = siteVersion.Version,
                    //    errors_occurred = ((artistErrors || albumErrors) ? true : false)
                    //});
                }
            }
        }

        public async Task RunQueueList()
        {
            List<QueueDataItem> queueItems = _dapper.GetList<QueueDataItem>("radioconn", "queue_get_pending_list");
            List<ArtistDataItem> allArtists = _dapper.GetList<ArtistDataItem>("armaradio", "Operations_GetAllMBArtistIdsWithDBSource");

            if (queueItems != null && queueItems.Count > 0)
            {
                using (var conn = _dapper.GetConnection("radioconn"))
                {
                    foreach (var queueItem in queueItems)
                    {
                        try
                        {
                            _dapper.ExecuteNonQuery("radioconn", "queue_set_list_started", new
                            {
                                id = queueItem.id
                            });

                            string tempFolder = EmptyFilesFromTempFolder();

                            DownloadHistoryFile(queueItem.file_url, tempFolder, "");

                            var listenFiles = Directory.EnumerateFiles(tempFolder, "*.listens", SearchOption.AllDirectories).ToList();
                            string listenFile = (listenFiles != null && listenFiles.Count > 0 ? listenFiles[0] : "");

                            ProcessFile(conn, listenFile, allArtists);

                            EmptyFilesFromTempFolder();

                            _dapper.ExecuteNonQuery("radioconn", "queue_set_list_completed", new
                            {
                                id = queueItem.id
                            });
                        }
                        catch (Exception ex)
                        {
                            _dapper.ExecuteNonQuery("radioconn", "queue_set_list_error_message", new
                            {
                                id = queueItem.id,
                                error = ex.Message.ToString()
                            });
                        }
                    }
                }
            }
        }

        private bool ProcessFile(SqlConnection conn, string FileWithPath, List<ArtistDataItem> allArtists)
        {
            bool returnItem = false;
            //List<ArtistDataItem> allArtists = _dapper.GetList<ArtistDataItem>("armaradio", "Operations_GetAllMBArtistIdsWithDBSource");

            using (FileStream fss = new FileStream(FileWithPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader fs = new StreamReader(fss, System.Text.Encoding.UTF8))
            {
                while (!fs.EndOfStream)
                {
                    string line = fs.ReadLine();
                    ListenEntryDataItem currentItem = Newtonsoft.Json.JsonConvert.DeserializeObject<ListenEntryDataItem>(line);

                    if (currentItem != null)
                    {
                        string artistId = currentItem.TrackMetadata?.AdditionalInfo?.LastfmArtistMbid;

                        if (!string.IsNullOrWhiteSpace(artistId))
                        {
                            //ArtistDataItem currentArtist = _dapper.GetFirstOrDefault<ArtistDataItem>("armaradio", "Arma_GetArtistByMBId", new
                            //{
                            //    artist_mbid = artistId
                            //}); 
                            ArtistDataItem currentArtist = allArtists.Where(ar => ar.MBId == artistId).FirstOrDefault();

                            if (currentArtist != null)
                            {
                                string songId = currentItem.TrackMetadata?.AdditionalInfo?.LastfmTrackMbid;
                                string songName = currentItem.TrackMetadata?.TrackName;
                                double time_stamp = currentItem.Timestamp;
                                int artistCount = 1;
                                int songCount = 0;

                                DateTime dateTimeHeard = DateTimeOffset.FromUnixTimeSeconds((long)time_stamp).DateTime;

                                if (!string.IsNullOrWhiteSpace(songId))
                                {
                                    songCount = 1;
                                }

                                _dapper.ExecuteNonQuery(conn, "history_insert_user_history_tally", new
                                {
                                    userid = currentItem.UserId,
                                    artist_mbid = artistId,
                                    song_mbid = songId,
                                    song_name = songName,
                                    db_source = currentArtist.DBSource,
                                    artist_count = artistCount,
                                    song_count = songCount,
                                    lasttime_heard = dateTimeHeard
                                });
                            }
                        }
                    }

                    var otherItem = currentItem;
                }
            }

            return returnItem;
        }

        private VersionDataItem GetLatestVersion()
        {
            VersionDataItem returnItem = null;
            string url = "https://data.metabrainz.org/pub/musicbrainz/listenbrainz/incremental/";
            string latestUrl = "";
            string version = "";

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

                        latestUrl = allLinks != null ? allLinks.Last().GetAttribute("href") : "";
                        version = allLinks != null ? allLinks.Last().Text() : "";
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(latestUrl))
            {
                returnItem = new VersionDataItem()
                {
                    Version = version
                };

                request = (HttpWebRequest)WebRequest.Create($"{url}{latestUrl}");
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

                            IHtmlCollection<IElement> allLinks = document.QuerySelectorAll("a");

                            foreach (var link in allLinks)
                            {
                                if (!link.GetAttribute("href").Contains(".."))
                                {
                                    returnItem.FileUrl = $"{url}{version}/{(link.GetAttribute("href"))}";

                                    break;
                                }
                            }
                        }
                    }
                }
            }

            return returnItem;
        }

        private async void DownloadHistoryFile(string Url, string tempFilesDir, string queueKey)
        {
            //_dapper.ExecuteNonQuery("radioconn", "Operations_Sync_ArtistTaskMarkAsStarted", new
            //{
            //    version_number = queueKey
            //});

            string artistFile = $"{tempFilesDir}historyfile.tar.xz";

            using (WebClient webClient = new WebClient())
            {
                webClient.Headers.Add("Accept: text/html, application/xhtml+xml, */*");
                webClient.Headers.Add("User-Agent: Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)");
                webClient.DownloadFile(new Uri(Url), artistFile);
            }

            if (File.Exists(artistFile))
            {
                using (var fileStream = File.OpenRead(artistFile))
                using (IReader reader = ReaderFactory.Open(fileStream))
                {
                    while (reader.MoveToNextEntry())
                    {
                        if (reader.Entry.Key.EndsWith(".listens"))
                        {
                            reader.WriteEntryToDirectory(tempFilesDir, new SharpCompress.Common.ExtractionOptions()
                            {
                                ExtractFullPath = true,
                                Overwrite = true
                            });
                        }
                    }
                }
            }

            if (System.IO.File.Exists($"{tempFilesDir}historyfile.tar.xz"))
            {
                System.IO.File.Delete($"{tempFilesDir}historyfile.tar.xz");
            }
        }

        private string EmptyFilesFromTempFolder()
        {
            string rootPath = _hostEnvironment.ContentRootPath.TrimEnd('/').TrimEnd('\\');
            string tempFiles = (IsLinux ? $"{rootPath}/tempFiles/" : $"{rootPath}\\tempFiles\\");

            if (!System.IO.Directory.Exists(tempFiles))
            {
                System.IO.Directory.CreateDirectory(tempFiles);
            }

            var file = Directory.EnumerateFiles(tempFiles, "*")
                .FirstOrDefault();

            while (file != null)
            {
                System.IO.File.Delete(file);

                file = Directory.EnumerateFiles(tempFiles, "*")
                    .FirstOrDefault();
            }

            foreach (var dir in Directory.EnumerateDirectories(tempFiles))
            {
                System.IO.Directory.Delete(dir, true);
            }

            return tempFiles;
        }
    }
}
