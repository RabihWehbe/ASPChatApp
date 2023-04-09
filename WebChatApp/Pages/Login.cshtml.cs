using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebChatApp.ViewModels;

namespace WebChatApp.Pages
{
    public class LoginModel : PageModel
    {

        [BindProperty]
        public Login modelLog { get; set; }


        public SignInManager<IdentityUser> _signInManager { get; set; }

        public void OnGet()
        {
        }

        public LoginModel(SignInManager<IdentityUser> signInManager)
        {
            this._signInManager = signInManager;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var identityResult = await _signInManager.PasswordSignInAsync(modelLog.Email, modelLog.Password, modelLog.RememberMe, false);

                if (identityResult.Succeeded)
                {
                    /*if(returnUrl == "" || returnUrl == "/")
                    {
                        return RedirectToPage("Index");
                    }
                    else
                    {
                        return RedirectToPage(returnUrl);
                    }*/
                    return RedirectToPage("/Index");
                }

                ModelState.AddModelError("","Username or Password incorrect");
            }

            return Page();
        }
    }
}
