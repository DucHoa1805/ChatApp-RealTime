using System.Security.Claims;
using ChatApp_RealTime.Models;
using ChatApp_RealTime.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ChatApp_RealTime.Pages
{
    [Authorize]
    public class ChatModel : PageModel
    {
        private readonly IChatService _chatService;

        public ChatModel(IChatService chatService)
        {
            _chatService = chatService;
        }

        public string CurrentUsername { get; set; } = string.Empty;
        public string? ChatPartnerUsername { get; set; }
        public List<Message> Messages { get; set; } = new();
        public List<User> AllUsers { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(string? partner)
        {
            CurrentUsername = User.Identity!.Name!;
            AllUsers = await _chatService.GetAllUsersAsync();

            if (!string.IsNullOrEmpty(partner))
            {
                var currentUser = await _chatService.GetUserByUsernameAsync(CurrentUsername);
                var partnerUser = await _chatService.GetUserByUsernameAsync(partner);

                if (currentUser != null && partnerUser != null)
                {
                    ChatPartnerUsername = partner;
                    Messages = await _chatService.GetMessagesAsync(currentUser.Id, partnerUser.Id);
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToPage("/Login");
        }
    }
}
