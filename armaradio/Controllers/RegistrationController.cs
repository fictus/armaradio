using armaradio.Models.Registration;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace armaradio.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly Repositories.IDapperHelper _dapper;
        private readonly ArmaSmtp.IArmaEmail _email;
        public RegistrationController(
            Repositories.IDapperHelper dapper,
            ArmaSmtp.IArmaEmail email
        )
        {
            _dapper = dapper;
            _email = email;
        }

        [HttpGet]
        public IActionResult ConfirmEmail(string t)
        {
            ConfirmEmailDataItem returnItem = new ConfirmEmailDataItem()
            {
                PreviuslyConfirmed = false,
                IsSuccess = false,
                NotFound = true
            };

            if (!string.IsNullOrEmpty(t))
            {
                returnItem = _dapper.GetFirstOrDefault<ConfirmEmailDataItem>("radioconn", "ArmaUsers_ConfirmEmailViaToken", new
                {
                    token = t
                });
            }

            return View(returnItem);
        }

        [HttpGet]
        public IActionResult Unsubscribe(string e)
        {
            RemoveFromEmailListDataItem returnItem = new RemoveFromEmailListDataItem()
            {
                WasRemovedFromEmails = false
            };

            if (!string.IsNullOrEmpty(e))
            {
                using (var con = _dapper.GetConnection("radioconn"))
                {
                    string email = _dapper.GetFirstOrDefault<string>(con, "ArmaUsers_GetEmailConfirmationToken", new
                    {
                        token = e
                    });

                    if (!string.IsNullOrWhiteSpace(email))
                    {
                        int result = _dapper.ExecuteNonQuery(con, "ArmaUsers_RemoveFromEmailList", new
                        {
                            email = email
                        });

                        returnItem.Email = email.Trim();
                        returnItem.WasRemovedFromEmails = (result > 0);
                    }
                }
            }

            return View(returnItem);
        }
    }
}
