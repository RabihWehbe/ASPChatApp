namespace WebChatApp.Model
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string image { get; set; }

        public ICollection<Message> Messages { get; set; }
        public ICollection<Contact> Contacts { get; set; }
    }
}
