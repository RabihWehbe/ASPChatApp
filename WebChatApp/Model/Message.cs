namespace WebChatApp.Model
{
    public class Message
    {
        public int MessageId { get; set; }
        public string Content { get; set; }
        public int SenderId { get; set; }

        public int ReceiverId { get; set; }
    }
}
