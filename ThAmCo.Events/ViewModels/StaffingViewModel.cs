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
        public String Role { get; set; }    

    }
}
