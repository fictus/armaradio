using armaradio_ops.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace armaradio_ops.Operations
{
    public class MBAlbumProcessor
    {
        public void SplitLargeJsonFile()
        {
            string basePath = "C:\\Users\\19039\\Downloads\\musicbrainzJson\\release_parts\\";

            if (!System.IO.Directory.Exists(basePath))
            {
                System.IO.Directory.CreateDirectory(basePath);
            }
            int fileName = 0;
            using (System.IO.Stream fs = new FileStream("C:\\Users\\19039\\Downloads\\musicbrainzJson\\release.json", FileMode.Open, FileAccess.Read))
            using (StreamReader streamReader = new StreamReader(fs, Encoding.GetEncoding("iso-8859-1")))
            {
                while (!streamReader.EndOfStream)
                {
                    string result = streamReader.ReadLine();

                    using (StreamWriter writetext = new StreamWriter(basePath + $"{fileName}.json"))
                    {
                        writetext.WriteLine(result);
                    }

                    fileName++;
                }
            }
        }

        public void ProcessAlbumsFromJsonFile()
        {
            var file = Directory.EnumerateFiles("C:\\Users\\19039\\Downloads\\musicbrainzJson\\release_parts\\", "*.json")
                .FirstOrDefault();

            while (file != null)
            {
                using (System.IO.Stream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                using (StreamReader streamReader = new StreamReader(fs, Encoding.GetEncoding("iso-8859-1")))
                {
                    while (!streamReader.EndOfStream)
                    {
                        string result = streamReader.ReadLine();

                        if (!string.IsNullOrWhiteSpace(result))
                        {
                            MBAlbumParseDataItem albumItem = Newtonsoft.Json.JsonConvert.DeserializeObject<MBAlbumParseDataItem>(result);

                            if (albumItem != null)
                            {
                                SaveMBAlbums(albumItem);
                            }
                        }
                    }
                }

                System.IO.File.Delete(file);

                file = Directory.EnumerateFiles("C:\\Users\\19039\\Downloads\\musicbrainzJson\\release_parts\\", "*.json")
                    .FirstOrDefault();
            }

            
        }

        private void SaveMBAlbums(MBAlbumParseDataItem albumItem)
        {
            if (albumItem != null && albumItem.artistcredit != null && albumItem.artistcredit.Count > 0 && albumItem.media != null && albumItem.media.Count > 0)
            {
                MBNewAlbumInsertDataItem newId = Data._SqlHelper.GetFirstOrDefault<MBNewAlbumInsertDataItem>("", "Operations_MBInsertAlbumForArtist", new
                {
                    mb_artistid = albumItem.artistcredit[0].artist.id,
                    mb_albumid = albumItem.id,
                    album_title = albumItem.title
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
                                    Data._SqlHelper.ExecuteNonQuery("", "Operations_MBInsertAlbumSong", new
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
