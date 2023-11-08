using System.ComponentModel.DataAnnotations;

namespace ThAmCo.Events.Data
{
    public class Staffing
    {
        public int StaffingId { get; set; }
        [Required]
        public int StaffId { get; set; }
        [Required]
        public int EventId { get; set; }
        [DataType(DataType.Date)]
        public DateTime AssignmentDate { get; set; }
        public String Role { get; set; }
    }
}
