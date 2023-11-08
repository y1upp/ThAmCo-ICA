using System.ComponentModel.DataAnnotations;

namespace ThAmCo.Events.ViewModels
{
    public class GuestViewModel
    {
        public int GuestId { get; set; }
        [Required]
        public String FirstName { get; set; }
        [Required]
        public String LastName { get; set; }
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public String Email { get; set; }
        [Phone(ErrorMessage = "Invalid Phone Number")]
        public String Phone { get; set; }
    }
}
