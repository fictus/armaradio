namespace armaradio.Tools
{
    public class ArmaWebRequest : IArmaWebRequest
    {
        public ArmaWebRequest()
        {

        }

        public T CallEndPointViaPost<T>(string EndPoint, object RequestItem, Dictionary<string, string> RequestHeaders = null)
        {
            T returnItem;

            System.Net.ServicePointManager.SecurityProtocol |= System.Net.SecurityProtocolType.Tls12;
            using (var stringContent = new System.Net.Http.StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(RequestItem ?? ""), System.Text.Encoding.UTF8, "application/json"))
            using (var client = new System.Net.Http.HttpClient())
            {
                if (RequestHeaders != null && RequestHeaders.Count > 0)
                {
                    foreach (var header in RequestHeaders)
                    {
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }

                var postTask = client.PostAsync(EndPoint, stringContent);
                postTask.Wait();

                if (postTask.Result.IsSuccessStatusCode)
                {
                    var readTask = postTask.Result.Content.ReadAsStringAsync();
                    readTask.Wait();

                    returnItem = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(readTask.Result);
                }
                else
                {
                    throw new Exception(postTask.Result.ReasonPhrase);
                }
            }

            return returnItem;
        }
    }
}
