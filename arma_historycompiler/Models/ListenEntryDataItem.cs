using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace arma_historycompiler.Models
{
    public class ListenEntryDataItem
    {
        [Newtonsoft.Json.JsonProperty("user_id")]
        public int UserId { get; set; }

        [Newtonsoft.Json.JsonProperty("user_name")]
        public string UserName { get; set; }

        [Newtonsoft.Json.JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        [Newtonsoft.Json.JsonProperty("track_metadata")]
        public TrackMetadata TrackMetadata { get; set; }

        [Newtonsoft.Json.JsonProperty("recording_msid")]
        public string RecordingMsid { get; set; }
    }

    public class TrackMetadata
    {
        [Newtonsoft.Json.JsonProperty("track_name")]
        public string TrackName { get; set; }

        [Newtonsoft.Json.JsonProperty("artist_name")]
        public string ArtistName { get; set; }

        [Newtonsoft.Json.JsonProperty("release_name")]
        public string ReleaseName { get; set; }

        [Newtonsoft.Json.JsonProperty("additional_info")]
        public AdditionalInfo AdditionalInfo { get; set; }
    }

    public class AdditionalInfo
    {
        // Common fields across all entries
        [Newtonsoft.Json.JsonProperty("submission_client")]
        public string SubmissionClient { get; set; }

        // Optional fields that may or may not be present
        [Newtonsoft.Json.JsonProperty("lastfm_track_mbid")]
        public string LastfmTrackMbid { get; set; }

        [Newtonsoft.Json.JsonProperty("lastfm_artist_mbid")]
        public string LastfmArtistMbid { get; set; }

        [Newtonsoft.Json.JsonProperty("lastfm_release_mbid")]
        public string LastfmReleaseMbid { get; set; }

        [Newtonsoft.Json.JsonProperty("recording_msid")]
        public string RecordingMsid { get; set; }

        // Fields specific to certain entries
        [Newtonsoft.Json.JsonProperty("duration")]
        public string Duration { get; set; }

        [Newtonsoft.Json.JsonProperty("origin_url")]
        public string OriginUrl { get; set; }

        [Newtonsoft.Json.JsonProperty("music_service_name")]
        public string MusicServiceName { get; set; }

        [Newtonsoft.Json.JsonProperty("submission_client_version")]
        public string SubmissionClientVersion { get; set; }

        // Additional fields from the last entry
        [Newtonsoft.Json.JsonProperty("date")]
        public string Date { get; set; }

        [Newtonsoft.Json.JsonProperty("isrc")]
        public string Isrc { get; set; }

        [Newtonsoft.Json.JsonProperty("discnumber")]
        public string DiscNumber { get; set; }

        [Newtonsoft.Json.JsonProperty("totaldiscs")]
        public string TotalDiscs { get; set; }

        [Newtonsoft.Json.JsonProperty("track_mbid")]
        public string TrackMbid { get; set; }

        [Newtonsoft.Json.JsonProperty("albumartist")]
        public string AlbumArtist { get; set; }

        [Newtonsoft.Json.JsonProperty("duration_ms")]
        public string DurationMs { get; set; }

        [Newtonsoft.Json.JsonProperty("totaltracks")]
        public string TotalTracks { get; set; }

        [Newtonsoft.Json.JsonProperty("tracknumber")]
        public string TrackNumber { get; set; }

        [Newtonsoft.Json.JsonProperty("artist_mbids")]
        public string[] ArtistMbids { get; set; }

        [Newtonsoft.Json.JsonProperty("media_player")]
        public string MediaPlayer { get; set; }

        [Newtonsoft.Json.JsonProperty("release_mbid")]
        public string ReleaseMbid { get; set; }

        [Newtonsoft.Json.JsonProperty("recording_mbid")]
        public string RecordingMbid { get; set; }

        [Newtonsoft.Json.JsonProperty("release_group_mbid")]
        public string ReleaseGroupMbid { get; set; }

        [Newtonsoft.Json.JsonProperty("media_player_version")]
        public string MediaPlayerVersion { get; set; }
    }
}
