namespace armaradio.Models.Request
{
    public class ProxySocks4Request
    {
        public List<ProxySocks4DataItem> proxies { get; set; }
    }

    public class ProxySocks4DataItem
    {
        public bool alive { get; set; }
        public float? alive_since { get; set; }
        public string anonymity { get; set; }
        public float? average_timeout { get; set; }
        public float? first_seen { get; set; }
        public string ip { get; set; }
        public float? ip_data_last_update { get; set; }
        public float? last_seen { get; set; }
        public int? port { get; set; }
        public string protocol { get; set; }
        public string proxy { get; set; }
        public bool ssl { get; set; }
        public float? timeout { get; set; }
        public int? times_alive { get; set; }
        public int? times_dead { get; set; }
        public float? uptime { get; set; }
    }
}
