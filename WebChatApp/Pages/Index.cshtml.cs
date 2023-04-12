using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebChatApp.Model;
using WebChatApp.Services;

namespace WebChatApp.Pages
{

    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public User? user { get; set; }

        public List<Contact> contacts { get; set; }

        public UserInfo _userInfoService { get; set; }

        public ContactsService _contactsService { get; set; }

        public IndexModel(ILogger<IndexModel> logger,UserInfo userInfoService,ContactsService contactsService)
        {
            _logger = logger;
            this._userInfoService = userInfoService;
            this._contactsService = contactsService;
        }

        public async Task OnGetAsync()
        {
            //Getting User infos:
            user = await this._userInfoService.getLoggedUser(this.User);

            contacts = await this._contactsService.BrowseContacts(this.user.UserId);
        }
    }
}