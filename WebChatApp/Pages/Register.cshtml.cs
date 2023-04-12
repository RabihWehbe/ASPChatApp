using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebChatApp.Model;
using WebChatApp.Services;
using WebChatApp.ViewModels;

namespace WebChatApp.Pages
{
    public class RegisterModel : PageModel
    {
        public UserManager<IdentityUser> _userManager { get; set; }

        public SignInManager<IdentityUser> _signInManager { get; set; }

        public RegisterUser _registerUser { get; set; }


        //note that identity user is our user type during authentication
        //we are injecting these 2 properties in the RegModel constructor
        public RegisterModel(UserManager<IdentityUser> userManager,SignInManager<IdentityUser> signInManager,RegisterUser registerUser)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._registerUser = registerUser;
        }

        [BindProperty]
        public Register modelReg { get; set; }

        public void OnGet()
        {
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser()
                {
                    UserName = modelReg.Email,
                    Email = modelReg.Email,
                };

                User userModel = new User
                {
                    Name = user.UserName,
                    Email= user.Email,
                    Password = modelReg.Password,
                    image = "static value"
                };

                var result = await _userManager.CreateAsync(user,modelReg.Password);

                if (result.Succeeded)
                {
                    await _registerUser.addAccount(userModel);
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToPage("Index");
                }

                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return Page();
        }


    }
}
