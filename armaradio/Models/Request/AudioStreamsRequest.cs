namespace armaradio.Models.Request
{
    public class AudioStreamsRequest
    {
        public StreamingDataDataItem streamingData { get; set; }
    }

    public class StreamingDataDataItem
    {
        public List<AdaptiveFormatDataItem> adaptiveFormats { get; set; }
    }

    public class AdaptiveFormatDataItem
    {
        public int? itag { get; set; }
        public string mimeType { get; set; }
        public string mimeTypeSimple { get; set; }
        public string codec { get; set; }
        public int? bitrate { get; set; }
        public string contentLength { get; set; }
        public string quality { get; set; }
        public string qualityLabel { get; set; }
        public int? averageBitrate { get; set; }
        public string approxDurationMs { get; set; }
        public string signatureCipher { get; set; }
        public string streamUrl { get; set; }
        public string containerName { get; set; }
    }
}
