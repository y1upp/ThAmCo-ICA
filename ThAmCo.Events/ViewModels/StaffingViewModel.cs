using System.ComponentModel.DataAnnotations;

namespace ThAmCo.Events.ViewModels
{
    public class StaffingViewModel
    {
        public int StaffingId { get; set; }

        [Required]
        public int StaffId { get; set; }

        [Required]
        public int EventId { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime AssignmentDate { get; set; }

        public string Role { get; set; }

        // Include additional properties from Staff and Event
        public string StaffFirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string EventTitle { get; set; }
    }
}