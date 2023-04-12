using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebChatApp.Services;

namespace WebChatApp.Pages
{
    public class LogoutModel : PageModel
    {


        private readonly SignInManager<IdentityUser> _signInManager;

        public ListsService _listsService { get; set; }

        public LogoutModel(SignInManager<IdentityUser> signInManager,ListsService listsService)
        {
            this._signInManager = signInManager;
            this._listsService = listsService;
        }


        public void OnGet()
        {
        }


        public async Task<IActionResult> OnPostLogoutAsync()
        {
            await _signInManager.SignOutAsync();
            _listsService.contacts.Clear();
            _listsService.isBrowsedContact = false;
            return RedirectToPage("/Login");
        }


        public IActionResult OnPostDontLogout()
        {
            return RedirectToPage("/Index");
        }
    }
}
