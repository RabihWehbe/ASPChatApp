using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebChatApp.Model;
using WebChatApp.Services;
using WebChatApp.ViewModels;

namespace WebChatApp.Pages
{

    [Authorize]
    public class CreateContactModel : PageModel
    {

        [BindProperty(SupportsGet = true)]
        public int userId { get; set; }

        [BindProperty]
        public ContactViewModel contact { get; set; }

        public ContactsService _service { get; set; }

        public bool showSuccessMessage { get; set; } = false;

        public CreateContactModel(ContactsService service)
        {
            this._service = service;
        }


        public void OnGet()
        {
        }



        public async Task<IActionResult> OnPostAsync()
        {
            int result;
            if (ModelState.IsValid)
            {
                result = await  this._service.insertContact(contact,userId);
                if(result == 0)
                {
                    ModelState.AddModelError("","The email provided doesn't exist");
                }
                else if(result == -1)
                {
                    ModelState.AddModelError("","You can't register your email account as contact!!!");
                }
                else if (result == -2)
                {
                    ModelState.AddModelError("", "Contact already exist,you can edit it though by selecting it");
                }
                else showSuccessMessage = true;
            }
            return Page();
        }
    }
}
