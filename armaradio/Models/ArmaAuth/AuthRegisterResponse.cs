namespace armaradio.Models.ArmaAuth
{
    public class AuthRegisterResponse
    {
        public string type { get; set; }
        public string title { get; set; }
        public int status { get; set; }
        public string detail { get; set; }
        public string instance { get; set; }
        public AuthRegisterErrors errors { get; set; }
        public string additionalProp1 { get; set; }
        public string additionalProp2 { get; set; }
        public string additionalProp3 { get; set; }
    }

    public class AuthRegisterErrors
    {
        public List<string> additionalProp1 { get; set; }
        public List<string> additionalProp2 { get; set; }
        public List<string> additionalProp3 { get; set; }
    }
}
