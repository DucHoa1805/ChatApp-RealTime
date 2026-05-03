namespace ChatApp_RealTime.Models
{
    public class ChatViewModel
    {
        public User CurrentUser { get; set; } = null!;
        public User? ChatPartner { get; set; }
        public List<Message> Messages { get; set; } = new();
        public List<User> AllUsers { get; set; } = new();
    }
}
