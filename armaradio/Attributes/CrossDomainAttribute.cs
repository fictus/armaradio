using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace armaradio.Attributes
{
    public class CrossDomainAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            IPAddress remoteIp = context.HttpContext.Connection.RemoteIpAddress;

            if (!IsLocalIPAddress(remoteIp))
            {
                string authDomainHeader = context.HttpContext.Request.Headers["referrerstate"];

                if (string.IsNullOrWhiteSpace(authDomainHeader) || authDomainHeader != "jdf8a80fdgoiady98f87ghdufa087jdpfoa98")
                {
                    context.Result = new ContentResult()
                    {
                        Content = "Request is from a different domain."
                    };

                    return;
                }
            }

            //string currentUrl = $"{context.HttpContext.Request.Scheme}://{context.HttpContext.Request.Host}{context.HttpContext.Request.Path}";
            //string referrerUrl = context.HttpContext.Request.Headers["Referer"].ToString();
            //string authDomainHeader = context.HttpContext.Request.Headers["referrerstate"];

            //if (!string.IsNullOrEmpty(referrerUrl) && !IsDomainAllowed(currentUrl, referrerUrl, authDomainHeader))
            //{
            //    context.Result = new ContentResult()
            //    {
            //        Content = "Request is from a different domain."
            //    };

            //    return;
            //}

            base.OnActionExecuting(context);
        }

        public bool IsLocalIPAddress(IPAddress ipAddress)
        {
            if (IPAddress.IsLoopback(ipAddress))
                return true;

            byte[] ipBytes = ipAddress.GetAddressBytes();
            if (ipBytes.Length == 4)
            {
                if (ipBytes[0] == 10 ||
                    (ipBytes[0] == 172 && (ipBytes[1] >= 16 && ipBytes[1] <= 31)) ||
                    (ipBytes[0] == 192 && ipBytes[1] == 168))
                {
                    return true;
                }
            }
            else if (ipBytes.Length == 16)
            {
                if (ipBytes[0] == 0xFE && (ipBytes[1] == 0x80 || ipBytes[1] == 0xC0))
                    return true;
            }

            return false;
        }

        private bool IsDomainAllowed(string url1, string url2, string authDomainHeader)
        {
            bool isAllowed = false;
            Uri uri1 = new Uri(url1);
            Uri uri2 = new Uri(url2);

            isAllowed = uri1.Host.Equals(uri2.Host, StringComparison.OrdinalIgnoreCase);

            if (!isAllowed)
            {
                isAllowed = (!string.IsNullOrWhiteSpace(authDomainHeader) && authDomainHeader == "jdf8a80fdgoiady98f87ghdufa087jdpfoa98");
            }

            return isAllowed;
        }
    }
}
