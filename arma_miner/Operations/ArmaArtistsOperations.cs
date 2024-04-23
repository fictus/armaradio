using AngleSharp;
using arma_miner.Data;
using arma_miner.Models;
using SharpCompress.Readers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace arma_miner.Operations
{
    public class ArmaArtistsOperations : IArmaArtistsOperations
    {
        private readonly bool IsLinux = false;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly IDapperHelper _dapper;
        public ArmaArtistsOperations(
            IHostEnvironment hostEnvironment,
            IDapperHelper dapper
        )
        {
            _hostEnvironment = hostEnvironment;
            _dapper = dapper;

            IsLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        }

        public bool ProcessArtistFile(string Url, string tempFilesDir, string queueKey)
        {
            bool completedWithErrors = false;
            string artistFile = $"{tempFilesDir}artist.tar.xz";

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
                        if (reader.Entry.Key.EndsWith("artist"))
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

                            try
                            { 
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
                            catch (Exception ex)
                            {
                                completedWithErrors = true;

                                _dapper.ExecuteNonQuery("radioconn", "Operations_Sync_LogError", new
                                {
                                    queue_key = queueKey,
                                    error_parent = "ArtistOperation",
                                    error_message = ex.Message.ToString(),
                                    json_source = (line ?? "")
                                });
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

                    try
                    {
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
                    catch (Exception ex)
                    {
                        completedWithErrors = true;

                        _dapper.ExecuteNonQuery("radioconn", "Operations_Sync_LogError", new
                        {
                            queue_key = queueKey,
                            error_parent = "ArtistOperation",
                            error_message = ex.Message.ToString(),
                            json_source = (line ?? "")
                        });
                    }
                }
            }

            int finalCount = newArtistsCount;

            return completedWithErrors;
        }

        private void SaveMBArtist(MBArtistParseDataItem artistItem)
        {
            if (artistItem != null)
            {
                int? newId = _dapper.GetFirstOrDefault<int?>("radioconn", "Operations_MBInsertArtistWithGenres", new
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
                        _dapper.ExecuteNonQuery("radioconn", "Operations_MBInsertArtistGenre", new
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
