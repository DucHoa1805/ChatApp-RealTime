using ChatApp_RealTime.Models;

namespace ChatApp_RealTime.Services
{
    public interface IChatService
    {
        Task<List<Message>> GetMessagesAsync(int userId1, int userId2);
        Task<Message> SendMessageAsync(int senderId, int receiverId, string content);
        Task<List<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserByUsernameAsync(string username);
    }
}
