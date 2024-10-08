using armaradio.Models.Registration;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace armaradio.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly Repositories.IDapperHelper _dapper;
        public RegistrationController(
            Repositories.IDapperHelper dapper
        )
        {
            _dapper = dapper;
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
            return View();
        }
    }
}
