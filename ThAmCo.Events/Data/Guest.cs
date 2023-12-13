using System.ComponentModel.DataAnnotations;

namespace ThAmCo.Events.Data
{
    public class Guest
    {
        public int GuestId { get; set; }

        // Guest Properties
        [Required]
        public String FirstName { get; set; }
        [Required]
        public String LastName { get; set; }
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public String Email { get; set; }
        [Phone(ErrorMessage = "Invalid Phone Number")]
        public String Phone { get; set; }
        public bool IsDeleted { get; set; }
        public bool RecentlyDeleted { get; set; }

        public void Anonymize()
        {
            FirstName = "Anonymized";
            LastName = "User";
            Email = "anonymized@example.com";
            Phone = "000-000-0000";
            IsDeleted = true;  // Assuming you still want to mark it as deleted
        }

        public ICollection<GuestBooking> GuestBookings { get; set; }
    }
}
