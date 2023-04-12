using System.ComponentModel.DataAnnotations;

namespace WebChatApp.ViewModels
{
    public class ContactViewModel
    {
        public int OwnerId { get; set; }
        public int Contactee_Id { get; set; }

        [Required]
        [MinLength(3)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
