using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using arma_miner.Data;
using arma_miner.Models;
using arma_miner.Operations;
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
        private readonly IArmaArtistsOperations _armaArtistsOps;
        private readonly IArmaAlbumsOperations _armaAlbumsOps;
        public ArmaMinerService(
            IHostEnvironment hostEnvironment,
            IDapperHelper dapper,
            IConfiguration configuration,
            IArmaArtistsOperations armaArtistsOps,
            IArmaAlbumsOperations armaAlbumsOps
        )
        {
            _hostEnvironment = hostEnvironment;
            _dapper = dapper;
            _configuration = configuration;
            _armaArtistsOps = armaArtistsOps;
            _armaAlbumsOps = armaAlbumsOps;

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
                    bool artistErrors = _armaArtistsOps.ProcessArtistFile(siteVersion.ArtistsFileUrl, tempFilesDir, siteVersion.Version);

                    tempFilesDir = EmptyFilesFromTempFolder();
                    bool albumErrors = _armaAlbumsOps.ProcessAlbumsFile(siteVersion.AlbumsFileUrl, tempFilesDir, siteVersion.Version);

                    EmptyFilesFromTempFolder();

                    _dapper.ExecuteNonQuery("radioconn", "Operations_Sync_AddVersionToCompleted", new
                    {
                        version_number = siteVersion.Version,
                        errors_occurred = ((artistErrors || albumErrors) ? true : false)
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

            var file = Directory.EnumerateFiles(tempFiles, "*")
                .FirstOrDefault();

            while (file != null)
            {
                System.IO.File.Delete(file);

                file = Directory.EnumerateFiles(tempFiles, "*")
                    .FirstOrDefault();
            }

            return tempFiles;
        }
    }
}
