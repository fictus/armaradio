using armaradio.Models.ArmaAuth;
using armaradio.Tools;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace armaradio.Repositories
{
    public class ArmaAuth : IArmaAuth
    {
        private readonly IHttpContextAccessor _context;
        private readonly IArmaWebRequest _webRequest;
        public ArmaAuth(
            IHttpContextAccessor context,
            IArmaWebRequest webRequest
        )
        {
            _context = context;
            _webRequest = webRequest;
        }

        public bool UserIsLoggedIn()
        {
            return _context.HttpContext.User?.Identity?.IsAuthenticated ?? false;
        }
                
        public ArmaUser GetCurrentUser()
        {
            ArmaUser returnItem = null;

            if ((_context.HttpContext.User?.Identity?.IsAuthenticated ?? false))
            {
                returnItem = new ArmaUser()
                {
                    UserId = _context.HttpContext.User.Claims.Where(cl => cl.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value,
                    UserName = _context.HttpContext.User.Identity.Name
                };

                if (string.IsNullOrWhiteSpace(returnItem.UserId))
                {
                    returnItem = null;
                }
            }

            return returnItem;
        }

        public AuthLoginResponse Login(string email, string password)
        {
            string urlEndpoint = (Debugger.IsAttached ? "https://localhost:7001" : "https://armarad.com");

            AuthLoginResponse returnItem = _webRequest.CallEndPointViaPost<AuthLoginResponse>($"{urlEndpoint}/login?useCookies=true", new
            {
                email = email,
                password = password
            }); //https://localhost:7001/login?useCookies=true

            return returnItem ?? new AuthLoginResponse();
        }

        public AuthRegisterResponse Register(string email, string password)
        {
            string urlEndpoint = (Debugger.IsAttached ? "https://localhost:7001" : "https://armarad.com");

            Dictionary<string, string> requestHeaders = new Dictionary<string, string>()
            {
                { "accept", "*/*" }
            };

            AuthRegisterResponse returnItem = _webRequest.CallEndPointViaPost<AuthRegisterResponse>($"{urlEndpoint}/register", new
            {
                email = email,
                password = password
            }, requestHeaders); //https://localhost:7001/login?useCookies=true

            return returnItem ?? new AuthRegisterResponse();
        }

        public bool IsValidEmailAddress(string Email)
        {
            //http://www.regular-expressions.info/email.html
            //return Regex.IsMatch("" + sEmail, @"^(?i)[a-z0-9#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+(?:aero|asia|biz|cat|com|coop|edu|gov|info|int|jobs|mil|mobi|museum|name|net|org|pro|tel|travel|xxx|ac|ad|ae|af|ag|ai|al|am|an|ao|aq|ar|as|at|au|aw|ax|az|ba|bb|bd|be|bf|bg|bh|bi|bj|bm|bn|bo|br|bs|bt|bv|bw|by|bz|ca|cc|cd|cf|cg|ch|ci|ck|cl|cm|cn|co|cr|cs|cu|cv|cx|cy|cz|de|dj|dk|dm|do|dz|ec|ee|eg|er|es|et|eu|fi|fj|fk|fm|fo|fr|ga|gb|gd|ge|gf|gg|gh|gi|gl|gm|gn|gp|gq|gr|gs|gt|gu|gw|gy|hk|hm|hn|hr|ht|hu|id|ieNorthernIreland|il|im|in|io|iq|ir|is|it|je|jm|jo|jp|ke|kg|kh|ki|km|kn|kp|kr|kw|ky|kz|la|lb|lc|li|lk|lr|ls|lt|lu|lv|ly|ma|mc|md|me|mg|mh|mk|ml|mm|mn|mo|mp|mq|mr|ms|mt|mu|mv|mw|mx|my|mz|na|nc|ne|nf|ng|ni|nl|no|np|nr|nu|nz|om|pa|pe|pf|pg|ph|pk|pl|pm|pn|pr|ps|pt|pw|py|qa|re|ro|rs|ru|rw|sa|sb|sc|sd|se|sg|sh|si|sj|sk|sl|sm|sn|so|sr|st|su|sv|sy|sz|tc|td|tf|tg|th|tj|tk|tl|tm|tn|to|tp|tr|tt|tv|tw|tz|ua|ug|uk|us|uy|uz|va|vc|ve|vg|vi|vn|vu|wf|ws|ye|yt|za|zm|zw)$");
            //return Regex.IsMatch("" + sEmail, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,})+)$");

            // copied from here: https://stackoverflow.com/a/14075010/2639711
            bool mappingInvalid = false;
            Email = Regex.Replace(Email, @"(@)(.+)$", match => {
                string domainName = match.Groups[2].Value;
                try
                {
                    domainName = new System.Globalization.IdnMapping().GetAscii(domainName);
                }
                catch (ArgumentException)
                {
                    mappingInvalid = true;
                }
                return match.Groups[1].Value + domainName;
            });

            if (mappingInvalid)
            {
                return false;
            }

            return Regex.IsMatch(Email,
                    @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$",
                    RegexOptions.IgnoreCase);
        }
    }
}
