namespace WebChatApp.Model
{
    public class Contact
    {
        public int ContactId { get; set; }

        //these ids should reference the ids in Users table:
        public int OwnerId { get; set; }
        public int Contactee_Id { get; set; }

        //name assigned by owner
        public string Name { get; set; }

    }
}
