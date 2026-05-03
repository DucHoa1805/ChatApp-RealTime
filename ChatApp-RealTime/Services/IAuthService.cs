using ChatApp_RealTime.Models;

namespace ChatApp_RealTime.Services
{
    public interface IAuthService
    {
        Task<User?> RegisterAsync(string username, string email, string password);
        Task<User?> LoginAsync(string username, string password);
        bool UsernameExists(string username);
        bool EmailExists(string email);
    }
}
