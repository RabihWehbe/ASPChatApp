using Microsoft.AspNetCore.Identity;
using WebChatApp.Model;

namespace WebChatApp.Services
{
    public class RegisterUser
    {

        private readonly SignInManager<IdentityUser> _signInManager;

        public AuthDbContext _authDbContext { get; set; }

        public RegisterUser(SignInManager<IdentityUser> signInManager, AuthDbContext authDbContext)
        {
            _signInManager = signInManager;
            _authDbContext = authDbContext;
        }

        public async Task addAccount(User user)
        {
            _authDbContext.Add(user);
            await _authDbContext.SaveChangesAsync();
        }
    }
}
