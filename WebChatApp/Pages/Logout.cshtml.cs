using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebChatApp.Services;

namespace WebChatApp.Pages
{
    public class LogoutModel : PageModel
    {


        private readonly SignInManager<IdentityUser> _signInManager;

        public LogoutModel(SignInManager<IdentityUser> signInManager)
        {
            this._signInManager = signInManager;
        }


        public void OnGet()
        {
        }


        public async Task<IActionResult> OnPostLogoutAsync()
        {
            await _signInManager.SignOutAsync();
            return RedirectToPage("/Login");
        }


        public IActionResult OnPostDontLogout()
        {
            return RedirectToPage("/Index");
        }
    }
}
