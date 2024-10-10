using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace armaradio.Repositories
{
    public class TeraboxUploader : ITeraboxUploader
    {
        private readonly HttpClient _httpClient;
        private const string BASE_URL = "https://www.terabox.com/api";
        private const string UPLOAD_URL = "https://c-jp.terabox.com/rest/2.0/pcs/superfile2";
        private readonly string _jsToken;
        
        public class PreCreateResponse
        {
            public List<int> Block_list { get; set; }
            public string Errmsg { get; set; }
            public int Errno { get; set; }
            public string Path { get; set; }
            public long Request_id { get; set; }
            public int Return_type { get; set; }
            public long Server_time { get; set; }
            public string Uploadid { get; set; }
        }

        public TeraboxUploader()
        {
            _httpClient = new HttpClient();
            _jsToken = "2974261B9FE27EA517A8BA943672964ADDA7CBECFD5143355B2011E5AE6E70B9785908EB3868CC4B232C3ED555A7D5ADBFD73DEA59C7CACFDADB20847DAE7D34";
        }

        public async Task<PreCreateResponse> PreCreateUpload(string filePath, long fileSize, DateTime localMtime)
        {
            var endpoint = $"{BASE_URL}/precreate";

            string blockList = CalculateFileMD5(filePath);

            var queryParams = new Dictionary<string, string>
        {
            {"app_id", "250528"},
            {"web", "1"},
            {"channel", "dubox"},
            {"clienttype", "0"},
            {"jsToken", _jsToken},
            {"logid", "55385400883491180031"}
        };

            var formData = new Dictionary<string, string>
        {
            {"path", filePath},
            {"autoinit", "1"},
            {"target_path", "/"},
            {"block_list", $"[\"{blockList}\"]"},
            {"size", fileSize.ToString()},
            {"file_limit_switch_v34", "true"},
            {"local_mtime", ((DateTimeOffset)localMtime).ToUnixTimeSeconds().ToString()}
        };

            var queryString = string.Join("&", queryParams.Select(x => $"{x.Key}={Uri.EscapeDataString(x.Value)}"));
            var requestUrl = $"{endpoint}?{queryString}";

            using var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);
            request.Content = new FormUrlEncodedContent(formData);

            var response = await _httpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<PreCreateResponse>(responseContent);
        }

        public async Task<string> UploadFile(string filePath, PreCreateResponse preCreateResponse)
        {
            var logid = Convert.ToBase64String(Encoding.UTF8.GetBytes(DateTime.Now.Ticks.ToString()));

            var queryParams = new Dictionary<string, string>
        {
            {"method", "upload"},
            {"app_id", "250528"},
            {"channel", "dubox"},
            {"clienttype", "0"},
            {"web", "1"},
            {"logid", logid},
            {"path", preCreateResponse.Path},
            {"uploadid", preCreateResponse.Uploadid},
            {"uploadsign", "0"},
            {"partseq", "0"}
        };

            var queryString = string.Join("&", queryParams.Select(x => $"{x.Key}={Uri.EscapeDataString(x.Value)}"));
            var requestUrl = $"{UPLOAD_URL}?{queryString}";

            using var multipartContent = new MultipartFormDataContent();
            using var fileStream = File.OpenRead(filePath);
            using var streamContent = new StreamContent(fileStream);

            multipartContent.Add(streamContent, "file", Path.GetFileName(filePath));

            using var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);
            request.Content = multipartContent;

            var response = await _httpClient.SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }

        private string CalculateFileMD5(string filePath)
        {
            using var md5 = MD5.Create();
            using var stream = File.OpenRead(filePath);
            var hash = md5.ComputeHash(stream);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }
    }
}
