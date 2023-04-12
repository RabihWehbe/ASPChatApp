using WebChatApp.Model;

namespace WebChatApp.Services
{
    public class ListsService
    {


        public bool isBrowsedContact = false;
        public List<Contact>? contacts { get; set; } = new List<Contact>();


    }
}
