using armaradio.Models.ArmaAuth;

namespace armaradio.Repositories
{
    public interface IArmaAuth
    {
        bool UserIsLoggedIn();
        ArmaUser GetCurrentUser();
        AuthLoginResponse Login(string username, string password);
        AuthRegisterResponse Register(string email, string password);
        bool IsValidEmailAddress(string Email);
    }
}
