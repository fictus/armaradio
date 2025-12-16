using armaradio.Models.ArmaAuth;

namespace armaradio.Repositories
{
    public interface IArmaAuth
    {
        bool UserIsLoggedIn();
        string GenerateJwtToken(string email);
        ArmaUser GetCurrentUser();
        bool IsAdminUser();
        AuthLoginResponse Login(string username, string password);
        AuthRegisterResponse Register(string email, string password);
        bool IsValidEmailAddress(string Email);
    }
}
