using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using WebChatApp.Model;

namespace WebChatApp.Services
{
    public class UserInfo
    {
        public User? user { get; set; } = null;


        private readonly SignInManager<IdentityUser> _signInManager;

        public AuthDbContext _authDbContext { get; set; }

        public IHttpContextAccessor _httpContextAccessor { get; set; }

        public UserInfo(SignInManager<IdentityUser> signInManager,AuthDbContext authDbContext, IHttpContextAccessor httpContextAccessor)
        {
            //Console.WriteLine("userInfo scoped service..");
            this._signInManager = signInManager;
            this._authDbContext = authDbContext;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<User?> getLoggedUser(ClaimsPrincipal claimsPrincipal)
        {
            if(user == null)
            {

                IdentityUser logged = await this._signInManager.UserManager.GetUserAsync(claimsPrincipal);

                user = await _authDbContext.Users
                    .Where(r => r.Email == logged.Email)
                    .Select(selected => new User
                    {
                        UserId = selected.UserId,
                        Name = selected.Name,
                        image = selected.image,
                        Email = selected.Email,
                        Password = selected.Password
                    }).SingleOrDefaultAsync();
            }
            if(user != null) _httpContextAccessor.HttpContext.Items["userId"] = user.UserId;
            return user;
        }
    }
}
