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
    public class ArmaAlbumsOperations : IArmaAlbumsOperations
    {
        private readonly bool IsLinux = false;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly IDapperHelper _dapper;
        public ArmaAlbumsOperations(
            IHostEnvironment hostEnvironment,
            IDapperHelper dapper
        )
        {
            _hostEnvironment = hostEnvironment;
            _dapper = dapper;

            IsLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        }

        public bool ProcessAlbumsFile(string Url, string tempFilesDir, string queueKey, bool fistTimeProcess)
        {
            bool completedWithErrors = false;
            //string artistFile = $"{tempFilesDir}release.tar.xz";

            //using (WebClient webClient = new WebClient())
            //{
            //    webClient.Headers.Add("Accept: text/html, application/xhtml+xml, */*");
            //    webClient.Headers.Add("User-Agent: Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)");
            //    webClient.DownloadFile(new Uri(Url), artistFile);
            //}

            //if (File.Exists(artistFile))
            //{
            //    using (var fileStream = File.OpenRead(artistFile))
            //    using (IReader reader = ReaderFactory.Open(fileStream))
            //    {
            //        while (reader.MoveToNextEntry())
            //        {
            //            if (reader.Entry.Key.EndsWith("release"))
            //            {
            //                reader.WriteEntryToDirectory(tempFilesDir, new SharpCompress.Common.ExtractionOptions()
            //                {
            //                    ExtractFullPath = true,
            //                    Overwrite = true
            //                });
            //            }
            //        }
            //    }
            //}

            string albumFileFull = (IsLinux ? $"{tempFilesDir}mbdump/release" : $"{tempFilesDir}mbdump\\release");

            if (fistTimeProcess)
            {
                completedWithErrors = FirstTimeProcess(albumFileFull, queueKey);
            }
            else
            {
                completedWithErrors = AppendedProcess(albumFileFull, queueKey);
            }

            return completedWithErrors;
        }

        private bool FirstTimeProcess(string albumFileFull, string queueKey)
        {
            bool completedWithErrors = false;
            bool albumExists = false;
            int newAlbumsCount = 0;

            using (System.IO.Stream fs = new FileStream(albumFileFull, FileMode.Open, FileAccess.Read))
            using (StreamReader streamReader = new StreamReader(fs, System.Text.Encoding.UTF8))
            {
                while (!streamReader.EndOfStream)
                {
                    string result = streamReader.ReadLine();

                    if (!string.IsNullOrWhiteSpace(result))
                    {
                        try
                        {
                            MBAlbumParseDataItem albumItem = Newtonsoft.Json.JsonConvert.DeserializeObject<MBAlbumParseDataItem>(result);

                            if (albumItem != null && albumItem.artistcredit != null && albumItem.artistcredit.Count > 0 && albumItem.media != null && albumItem.media.Count > 0)
                            {
                                albumExists = _dapper.GetFirstOrDefault<bool>("radioconn", "Operations_CheckIfMBAlbumIdExists", new
                                {
                                    mb_albumid = albumItem.id,
                                    mb_artistid = albumItem.artistcredit[0].artist.id
                                });

                                if (!albumExists)
                                {
                                    SaveMBAlbum(albumItem);
                                    newAlbumsCount++;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            completedWithErrors = true;

                            _dapper.ExecuteNonQuery("radioconn", "Operations_Sync_LogError", new
                            {
                                queue_key = queueKey,
                                error_parent = "AlbumsOperation",
                                error_message = ex.Message.ToString(),
                                json_source = (result ?? "")
                            });
                        }
                    }
                }
            }

            return completedWithErrors;
        }

        private bool AppendedProcess(string albumFileFull, string queueKey)
        {
            bool completedWithErrors = false;
            bool albumExists = false;
            int newAlbumsCount = 0;

            using (FileStream fss = new FileStream(albumFileFull, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader fs = new StreamReader(fss, System.Text.Encoding.UTF8))
            {
                // Start reading from the end of the file
                fs.BaseStream.Seek(0, SeekOrigin.End);

                // Read the file stream backward
                long position = fs.BaseStream.Position;
                byte[] buffer = new byte[1024];
                StringBuilder sb = new StringBuilder();

                while (!albumExists && position > 0)
                {
                    fs.BaseStream.Seek(-Math.Min(position, buffer.Length), SeekOrigin.Current);

                    int bytesRead = fs.BaseStream.Read(buffer, 0, buffer.Length);
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
                                MBAlbumParseDataItem albumItem = Newtonsoft.Json.JsonConvert.DeserializeObject<MBAlbumParseDataItem>(line);

                                if (albumItem != null && albumItem.artistcredit != null && albumItem.artistcredit.Count > 0 && albumItem.media != null && albumItem.media.Count > 0)
                                {
                                    albumExists = _dapper.GetFirstOrDefault<bool>("radioconn", "Operations_CheckIfMBAlbumIdExists", new
                                    {
                                        mb_albumid = albumItem.id,
                                        mb_artistid = albumItem.artistcredit[0].artist.id
                                    });

                                    if (!albumExists)
                                    {
                                        SaveMBAlbum(albumItem);
                                        newAlbumsCount++;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                completedWithErrors = true;

                                _dapper.ExecuteNonQuery("radioconn", "Operations_Sync_LogError", new
                                {
                                    queue_key = queueKey,
                                    error_parent = "AlbumsOperation",
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
                    fs.BaseStream.Seek(-bytesRead, SeekOrigin.Current);
                }

                // Process the first line if any
                if (position <= 0 && sb.Length > 0)
                {
                    string line = sb.ToString();

                    try
                    {
                        MBAlbumParseDataItem albumItem = Newtonsoft.Json.JsonConvert.DeserializeObject<MBAlbumParseDataItem>(line);

                        if (albumItem != null && albumItem.artistcredit != null && albumItem.artistcredit.Count > 0 && albumItem.media != null && albumItem.media.Count > 0)
                        {
                            albumExists = _dapper.GetFirstOrDefault<bool>("radioconn", "Operations_CheckIfMBAlbumIdExists", new
                            {
                                mb_albumid = albumItem.id,
                                mb_artistid = albumItem.artistcredit[0].artist.id
                            });

                            if (!albumExists)
                            {
                                SaveMBAlbum(albumItem);
                                newAlbumsCount++;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        completedWithErrors = true;

                        _dapper.ExecuteNonQuery("radioconn", "Operations_Sync_LogError", new
                        {
                            queue_key = queueKey,
                            error_parent = "AlbumsOperation",
                            error_message = ex.Message.ToString(),
                            json_source = (line ?? "")
                        });
                    }
                }
            }

            int finalCount = newAlbumsCount;

            return completedWithErrors;
        }

        private void SaveMBAlbum(MBAlbumParseDataItem albumItem)
        {
            if (albumItem != null && albumItem.artistcredit != null && albumItem.artistcredit.Count > 0 && albumItem.media != null && albumItem.media.Count > 0)
            {
                MBNewAlbumInsertDataItem newId = _dapper.GetFirstOrDefault<MBNewAlbumInsertDataItem>("radioconn", "Operations_MBInsertAlbumForArtist", new
                {
                    mb_artistid = albumItem.artistcredit[0].artist.id,
                    mb_albumid = albumItem.id,
                    album_title = albumItem.title,
                    status = albumItem.status,
                    primary_type = (albumItem.releasegroup != null ? (albumItem.releasegroup.primarytype ?? "") : ""),
                    first_release_date = (albumItem.releasegroup != null ? (albumItem.releasegroup.firstreleasedate ?? "") : "")
                });

                if (newId != null && newId.new_id.HasValue && !string.IsNullOrWhiteSpace(newId.db_source))
                {
                    int cdNumber = 0;
                    foreach (var cd in albumItem.media)
                    {
                        cdNumber++;

                        if (cd != null && cd.tracks != null && cd.tracks.Count > 0)
                        {
                            int songPosition = 0;

                            foreach (var track in cd.tracks)
                            {
                                songPosition++;

                                if (track != null)
                                {
                                    _dapper.ExecuteNonQuery("radioconn", "Operations_MBInsertAlbumSong", new
                                    {
                                        db_source = newId.db_source,
                                        mb_songid = track.id,
                                        album_id = newId.new_id.Value,
                                        cd_number = cdNumber,
                                        song_number = (track.position.HasValue ? track.position.Value : songPosition),
                                        song_title = track.title
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
