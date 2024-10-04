using armaradio.Repositories;
using Azure.Core;
using System.Net;
using System.Security.Claims;
using YoutubeExplode.Channels;

namespace armaradio.Operations
{
    public class ArmaUserOperation : IDisposable
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDapperHelper _dapper;
        private OperationLog _currentLog;
        private bool _disposed = false;
        private static readonly AsyncLocal<bool> _operationInProgress = new AsyncLocal<bool>();

        public ArmaUserOperation(
            IHttpContextAccessor httpContextAccessor,
            IDapperHelper dapper
        )
        {
            _httpContextAccessor = httpContextAccessor;
            _dapper = dapper;

            if (!_operationInProgress.Value)
            {
                _operationInProgress.Value = true;
                InitializeLog();
            }
        }

        private void InitializeLog()
        {
            var context = _httpContextAccessor.HttpContext;
            var user = context.User;
            var request = context.Request;
            var fullUrl = $"{request.Scheme}://{request.Host}{request.PathBase}{request.Path}{request.QueryString}";

            _currentLog = new OperationLog
            {
                Timestamp = DateTime.UtcNow,
                IpAddress = context.Connection.RemoteIpAddress?.ToString(),
                UserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                UserName = user.Identity?.Name,
                RequestPath = fullUrl,
                RequestMethod = context.Request.Method,
                UserAgent = context.Request.Headers["User-Agent"].ToString(),
                Referrer = context.Request.Headers["Referer"].ToString(),
                RequestHeaders = string.Join(";", context.Request.Headers.Select(h => $"{h.Key}={h.Value}")),
                QueryString = context.Request.QueryString.ToString()
            };
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _currentLog.Duration = (DateTime.UtcNow - _currentLog.Timestamp).TotalMilliseconds;

                _dapper.ExecuteNonQuery("radioconn", "Operations_LogUserActivity", new
                {
                    Timestamp = _currentLog.Timestamp,
                    IpAddress = _currentLog.IpAddress,
                    UserId = _currentLog.UserId,
                    UserName = _currentLog.UserName,
                    RequestPath = _currentLog.RequestPath,
                    RequestMethod = _currentLog.RequestMethod,
                    UserAgent = _currentLog.UserAgent,
                    Referrer = _currentLog.Referrer,
                    RequestHeaders = _currentLog.RequestHeaders,
                    QueryString = _currentLog.QueryString,
                    Duration = _currentLog.Duration
                });

                _disposed = true;
                _operationInProgress.Value = false;
            }
        }
    }

    public class OperationLog
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string IpAddress { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string RequestPath { get; set; }
        public string RequestMethod { get; set; }
        public string UserAgent { get; set; }
        public string Referrer { get; set; }
        public string RequestHeaders { get; set; }
        public string QueryString { get; set; }
        public double Duration { get; set; }
    }
}
