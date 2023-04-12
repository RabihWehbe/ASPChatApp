using Microsoft.EntityFrameworkCore;
using WebChatApp.Model;
using WebChatApp.ViewModels;

namespace WebChatApp.Services
{
    public class ContactsService
    {
        public AuthDbContext _context { get; set; }
        public UserInfo _userInfo { get; set; }

        public ListsService _listsService { get; set; }

        public ContactsService(AuthDbContext context,UserInfo userInfo,ListsService listsService)
        {
            _context = context;
            _userInfo = userInfo;
            _listsService = listsService;
        }

        public async Task<User?> getUserId(string Email)
        {
            User? user = await _context.Users
                .Where(x => x.Email == Email)
                .Select(r => new User { UserId = r.UserId,Name = r.Name, Email = r.Email })
                .SingleOrDefaultAsync();

            return user;
        }

        public async Task<User> getUser(int userId)
        {
            User user = await _context.Users.FindAsync(userId);
            return user;
        }


        public async Task<bool> isContactExist(int userId,int contactee_id)
        {
            var contact = await _context.Contacts
                .Where(c => c.Contactee_Id == contactee_id && c.OwnerId == userId)
                .SingleOrDefaultAsync();

            if (contact == null) return false;
            return true;
        }

        public async Task<int> insertContact(ContactViewModel viewModel,int userID)
        {
            User? contactee = await getUserId(viewModel.Email);
            
            //if email provided doesn't exist:
            if(contactee == null)
            {
                return 0;
            }
            //if the email provided is the user email:
            var checkOwner = await getUser(userID);
            if(checkOwner.Email == contactee.Email)
            {
                return -1;
            }
            //if the contact already registered by owner:
            bool isContactAlreadyExist = await isContactExist(userID, contactee.UserId);
            if(isContactAlreadyExist)
            {
                return -2;
            }
            //pass all the tests
            else
            {
                Contact contact = new Contact
                {
                    OwnerId = userID,
                    Contactee_Id = contactee.UserId,
                    Name = viewModel.Name,
                };
                _context.Contacts.Add(contact);
                await _context.SaveChangesAsync();
                _listsService.contacts.Add(contact);
            }
            return 1;
        } 


        public async Task<List<Contact>> BrowseContacts(int userId)
        {
            if(_listsService.contacts.Count == 0 && _listsService.isBrowsedContact == false)
            {
                Console.WriteLine("----> calling DB context once for filling the contacts lists");
                _listsService.contacts = await _context.Contacts
                    .Where(c => c.OwnerId == userId)
                    .Select(r => new Contact
                    {
                        Name = r.Name,
                        ContactId = r.ContactId,
                        OwnerId = r.OwnerId,
                        Contactee_Id = r.Contactee_Id,
                    }).ToListAsync();
            }
            else Console.WriteLine("----> This time we didn't call the DB context");
            return _listsService.contacts;

            //List<Contact> contacts = await _context.Contacts
            //        .Where(c => c.OwnerId == userId)
            //        .Select(r => new Contact
            //        {
            //            Name = r.Name,
            //            ContactId = r.ContactId,
            //            OwnerId = r.OwnerId,
            //           Contactee_Id = r.Contactee_Id,
            //      }).ToListAsync();
            //return contacts;
        }
    }
}
