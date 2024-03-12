namespace armaradio.Tools
{
    public interface IArmaWebRequest
    {
        T CallEndPointViaPost<T>(string EndPoint, object RequestItem, Dictionary<string, string> RequestHeaders = null);
    }
}
