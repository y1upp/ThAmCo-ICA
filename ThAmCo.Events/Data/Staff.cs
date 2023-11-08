using System.ComponentModel.DataAnnotations;

namespace ThAmCo.Events.Data
{
    public class Staff
    {
        public int StaffId { get; set; }
        [Required]
        public String FirstName { get; set; }
        [Required]
        public String LastName { get; set; }
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public String Email { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        public String PhoneNumber { get; set; } 
    }
}
