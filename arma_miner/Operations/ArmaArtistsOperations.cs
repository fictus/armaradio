using AngleSharp;
using arma_miner.Data;
using arma_miner.Models;
using Microsoft.Data.SqlClient;
using SharpCompress.Readers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace arma_miner.Operations
{
    public class ArmaArtistsOperations : IArmaArtistsOperations
    {
        private DataTable dataTable = new DataTable();
        private DataTable dataTableGenres = new DataTable();
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

        public async Task<bool> ProcessArtistFile(string Url, string tempFilesDir, string queueKey, bool fistTimeProcess)
        {
            _dapper.ExecuteNonQuery("radioconn", "Operations_Sync_ArtistTaskMarkAsStarted", new
            {
                version_number = queueKey
            });

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
            //string artistFileFull = "C:\\Users\\19039\\Downloads\\savedfromerror.json";

            if (fistTimeProcess)
            {
                completedWithErrors = FirstTimeProcess(artistFileFull, queueKey);
                //completedWithErrors = FirstTimeProcessBulkInsert(artistFileFull, queueKey);
            }
            else
            {
                completedWithErrors = AppendedProcess(artistFileFull, queueKey);
            }

            _dapper.ExecuteNonQuery("radioconn", "Operations_Sync_ArtistTaskMarkAsCompleted", new
            {
                version_number = queueKey
            });

            return completedWithErrors;
        }

        private bool FirstTimeProcess(string artistFileFull, string queueKey)
        {
            bool completedWithErrors = false;
            bool artistExists = false;
            int newArtistsCount = 0;

            using (System.IO.Stream fs = new FileStream(artistFileFull, FileMode.Open, FileAccess.Read))
            using (StreamReader streamReader = new StreamReader(fs, System.Text.Encoding.UTF8))
            {
                while (!streamReader.EndOfStream)
                {
                    string result = streamReader.ReadLine();

                    if (!string.IsNullOrWhiteSpace(result))
                    {
                        try
                        {
                            MBArtistParseDataItem artistItem = Newtonsoft.Json.JsonConvert.DeserializeObject<MBArtistParseDataItem>(result);



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
                                json_source = (result ?? "")
                            });
                        }
                    }
                }
            }

            return completedWithErrors;
        }

        private bool AppendedProcess(string artistFileFull, string queueKey)
        {
            bool completedWithErrors = false;
            bool artistExists = false;
            int newArtistsCount = 0;

            using (FileStream fss = new FileStream(artistFileFull, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader fs = new StreamReader(fss, System.Text.Encoding.UTF8))
            {
                // Start reading from the end of the file
                fs.BaseStream.Seek(0, SeekOrigin.End);

                // Read the file stream backward
                long position = fs.BaseStream.Position;
                byte[] buffer = new byte[1024];
                StringBuilder sb = new StringBuilder();

                while (!artistExists && position > 0)
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
                    fs.BaseStream.Seek(-bytesRead, SeekOrigin.Current);
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

        private bool FirstTimeProcessBulkInsert(string artistFileFull, string queueKey)
        {
            bool completedWithErrors = false;
            bool artistExists = false;
            int newArtistsCount = 0;
            List<string> errorSB = new List<string>();

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("MBId", typeof(string));
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("SortName", typeof(string));
            dataTable.Columns.Add("NameFlat", typeof(string));
            dataTable.Columns.Add("NameFlatReverse", typeof(string));
            dataTable.Columns.Add("NameSearch", typeof(string));
            dataTable.Columns.Add("NameSearchReverse", typeof(string));
            dataTable.Columns.Add("Country", typeof(string));
            dataTable.Columns.Add("Type", typeof(string));
            dataTable.Columns.Add("TypeId", typeof(string));
            dataTable.Columns.Add("RatingValue", typeof(string));
            dataTable.Columns.Add("RatingVotes", typeof(string));
            dataTable.Columns.Add("LifeSpanBegin", typeof(string));
            dataTable.Columns.Add("LifeSpanEnd", typeof(string));
            dataTable.Columns.Add("LifeSpanEnded", typeof(bool));
            dataTable.Columns.Add("DBSource", typeof(string));
            dataTable.Columns.Add("AddedDateTime", typeof(DateTime));


            DataTable dataTableGenres = new DataTable();
            dataTableGenres.Columns.Add("MBId", typeof(string));
            dataTableGenres.Columns.Add("ArtistMBId", typeof(string));
            dataTableGenres.Columns.Add("GenreName", typeof(string));
            dataTableGenres.Columns.Add("Count", typeof(string));
            dataTableGenres.Columns.Add("Disambiguation", typeof(string));

            using (System.IO.Stream fs = new FileStream(artistFileFull, FileMode.Open, FileAccess.Read))
            using (StreamReader streamReader = new StreamReader(fs, System.Text.Encoding.UTF8))
            using (var conn = _dapper.GetConnection("radioconn"))
            using (var conArtists = _dapper.GetConnection("radioconnArtists"))
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conArtists))
            using (SqlBulkCopy bulkCopyGenres = new SqlBulkCopy(conArtists))
            {
                bulkCopy.DestinationTableName = "ArmaArtistsMainStaging";
                bulkCopy.BatchSize = 1500;
                bulkCopy.BulkCopyTimeout = 0;

                bulkCopy.ColumnMappings.Add("MBId", "MBId");
                bulkCopy.ColumnMappings.Add("Name", "Name");
                bulkCopy.ColumnMappings.Add("SortName", "SortName");
                bulkCopy.ColumnMappings.Add("NameFlat", "NameFlat");
                bulkCopy.ColumnMappings.Add("NameFlatReverse", "NameFlatReverse");
                bulkCopy.ColumnMappings.Add("NameSearch", "NameSearch");
                bulkCopy.ColumnMappings.Add("NameSearchReverse", "NameSearchReverse");
                bulkCopy.ColumnMappings.Add("Country", "Country");
                bulkCopy.ColumnMappings.Add("Type", "Type");
                bulkCopy.ColumnMappings.Add("TypeId", "TypeId");
                bulkCopy.ColumnMappings.Add("RatingValue", "RatingValue");
                bulkCopy.ColumnMappings.Add("RatingVotes", "RatingVotes");
                bulkCopy.ColumnMappings.Add("LifeSpanBegin", "LifeSpanBegin");
                bulkCopy.ColumnMappings.Add("LifeSpanEnd", "LifeSpanEnd");
                bulkCopy.ColumnMappings.Add("LifeSpanEnded", "LifeSpanEnded");
                bulkCopy.ColumnMappings.Add("DBSource", "DBSource");
                bulkCopy.ColumnMappings.Add("AddedDateTime", "AddedDateTime");

                bulkCopyGenres.DestinationTableName = "ArmaGenresStaged";
                bulkCopyGenres.BatchSize = 1500;
                bulkCopyGenres.BulkCopyTimeout = 0;

                bulkCopyGenres.ColumnMappings.Add("MBId", "MBId");
                bulkCopyGenres.ColumnMappings.Add("ArtistMBId", "ArtistMBId");
                bulkCopyGenres.ColumnMappings.Add("GenreName", "GenreName");
                bulkCopyGenres.ColumnMappings.Add("Count", "Count");
                bulkCopyGenres.ColumnMappings.Add("Disambiguation", "Disambiguation");

                while (!streamReader.EndOfStream)
                {
                    string result = streamReader.ReadLine();

                    if (!string.IsNullOrWhiteSpace(result))
                    {
                        errorSB.Add(result);

                        try
                        {
                            MBArtistParseDataItem artistItem = Newtonsoft.Json.JsonConvert.DeserializeObject<MBArtistParseDataItem>(result);

                            if (artistItem != null)
                            {
                                artistExists = _dapper.GetFirstOrDefault<bool>(conn, "Operations_CheckIfMBArtistIdExists", new
                                {
                                    mb_artistid = artistItem.id
                                });

                                if (!artistExists)
                                {
                                    string nameFlat = Latinize(artistItem.sortname);
                                    string dbSource = nameFlat.Trim()[0].ToString().ToLower();
                                    string nameFlatReverse = new string(nameFlat.Reverse().ToArray());
                                    string nameSearch = nameFlat.Trim();

                                    if (Int32.TryParse(dbSource, out int _tmpDBSource))
                                    {
                                        dbSource = "num";
                                    }
                                    else
                                    {
                                        char cDbSource = Convert.ToChar(dbSource);

                                        if (!((cDbSource >= 'a' && cDbSource <= 'z') || (cDbSource >= 'A' && cDbSource <= 'Z')))
                                        {
                                            dbSource = "symb";
                                        }
                                    }

                                    if (nameFlat.Contains(','))
                                    {
                                        List<string> stringParts = nameFlat.Trim().Split(',').ToList();
                                        List<string> finalParts = new List<string>();

                                        for (int i = 0; i < stringParts.Count - 1; i++)
                                        {
                                            finalParts.Add(stringParts[i].Trim());
                                        }

                                        nameSearch = stringParts.Last().Trim() + " " + string.Join(", ", finalParts.ToArray());
                                    }

                                    string nameSearchReverse = new string(nameSearch.Reverse().ToArray());

                                    DataRow newRow = dataTable.NewRow();
                                    newRow["MBId"] = artistItem.id;
                                    newRow["Name"] = artistItem.name;
                                    newRow["SortName"] = artistItem.sortname;
                                    newRow["NameFlat"] = nameFlat;
                                    newRow["NameFlatReverse"] = nameFlatReverse;
                                    newRow["NameSearch"] = nameSearch;
                                    newRow["NameSearchReverse"] = nameSearchReverse;
                                    newRow["Country"] = artistItem.country;
                                    newRow["Type"] = artistItem.type;
                                    newRow["TypeId"] = artistItem.typeid;
                                    newRow["RatingValue"] = artistItem.rating != null ? (artistItem.rating.value.HasValue ? artistItem.rating.value.ToString() : DBNull.Value) : DBNull.Value;
                                    newRow["RatingVotes"] = artistItem.rating != null ? (artistItem.rating.votescount.HasValue ? artistItem.rating.votescount.Value.ToString() : DBNull.Value) : DBNull.Value;
                                    newRow["LifeSpanBegin"] = artistItem.lifespan != null ? artistItem.lifespan.begin : "";
                                    newRow["LifeSpanEnd"] = artistItem.lifespan != null ? artistItem.lifespan.end : "";
                                    newRow["LifeSpanEnded"] = artistItem.lifespan != null ? artistItem.lifespan.ended : false;
                                    newRow["DBSource"] = dbSource;
                                    newRow["AddedDateTime"] = DateTime.Now;

                                    dataTable.Rows.Add(newRow);

                                    if (dataTable.Rows.Count >= bulkCopy.BatchSize)
                                    {
                                        bulkCopy.WriteToServer(dataTable);
                                        dataTable.Clear();
                                        errorSB.Clear();
                                    }

                                    foreach (var genre in artistItem.genres)
                                    {
                                        DataRow newRowGenre = dataTableGenres.NewRow();
                                        newRowGenre["MBId"] = genre.id;
                                        newRowGenre["ArtistMBId"] = artistItem.id;
                                        newRowGenre["GenreName"] = genre.name;
                                        newRowGenre["Count"] = (genre.count.HasValue ? genre.count.Value.ToString() : DBNull.Value);
                                        newRowGenre["Disambiguation"] = genre.disambiguation;

                                        dataTableGenres.Rows.Add(newRowGenre);
                                    }

                                    if (dataTableGenres.Rows.Count >= bulkCopyGenres.BatchSize)
                                    {
                                        bulkCopyGenres.WriteToServer(dataTableGenres);
                                        dataTableGenres.Clear();
                                    }

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
                                json_source = string.Join("\n", errorSB.ToArray())
                            });

                            dataTable.Clear();
                            errorSB.Clear();
                        }
                    }
                }

                if (dataTable.Rows.Count > 0)
                {
                    try
                    {
                        bulkCopy.WriteToServer(dataTable);
                        dataTable.Clear();
                    }
                    catch (Exception ex)
                    {
                        completedWithErrors = true;

                        _dapper.ExecuteNonQuery("radioconn", "Operations_Sync_LogError", new
                        {
                            queue_key = queueKey,
                            error_parent = "ArtistOperation",
                            error_message = ex.Message.ToString(),
                            json_source = string.Join("\n", errorSB.ToArray())
                        });

                        dataTable.Clear();
                        errorSB.Clear();
                    }
                }

                if (dataTableGenres.Rows.Count > 0)
                {
                    bulkCopyGenres.WriteToServer(dataTableGenres);
                    dataTableGenres.Clear();
                }
            }

            return completedWithErrors;
        }

        private string Latinize(string Input)
        {
            Encoding latinizeEncoding = Encoding.GetEncoding("ISO-8859-8");
            var strBytes = latinizeEncoding.GetBytes(Input);

            return latinizeEncoding.GetString(strBytes);
        }
    }
}
