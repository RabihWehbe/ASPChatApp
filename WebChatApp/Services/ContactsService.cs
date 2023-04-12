using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Xml.Linq;
using WebChatApp.Model;
using WebChatApp.ViewModels;

namespace WebChatApp.Services
{
    public class ContactsService
    {
        public AuthDbContext _context { get; set; }
        public UserInfo _userInfo { get; set; }


        public List<Contact>? contacts { get; set; } = null;

        private readonly IMemoryCache _cache;

        public  string _cacheKey { get; set; }

        public ContactsService(AuthDbContext context,UserInfo userInfo,IMemoryCache cache)
        {
            _context = context;
            _userInfo = userInfo;
            _cache = cache;
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

                //save changes to the cache
                contacts = await BrowseContacts(userID);
                contacts.Add(contact);
                _cacheKey = $"MyConatcts_{userID}";
                _cache.Set(_cacheKey, contacts,TimeSpan.FromMinutes(5));

                //save changes to database
                _context.Contacts.Add(contact);
                await _context.SaveChangesAsync();
            }
            return 1;
        } 


        public async Task<List<Contact>> BrowseContacts(int userId)
        {
            _cacheKey = $"MyConatcts_{userId}";
            Console.WriteLine($"------>BrowseContacts_{_cacheKey}");
            
            //trying to output a list of model, from caxche using the convenient key
            if (!_cache.TryGetValue(_cacheKey, out List<Contact> myContacts))
            {
                Console.WriteLine($"------>BrowseContacts empty cache List=> call DB");
                contacts = await _context.Contacts
                .Where(c => c.OwnerId == userId)
                .Select(r => new Contact
                {
                    Name = r.Name,
                    ContactId = r.ContactId,
                    OwnerId = r.OwnerId,
                    Contactee_Id = r.Contactee_Id,
                }).ToListAsync();

                _cache.Set(_cacheKey, contacts, TimeSpan.FromMinutes(5));
            }
            else
            {
                Console.WriteLine($"------>BrowseContacts cache List contains values => contacts directly fetched from cache");
                contacts = myContacts;
            }
            return contacts;
        }
    }
}
