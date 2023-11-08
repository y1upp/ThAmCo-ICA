using System.ComponentModel.DataAnnotations;

namespace ThAmCo.Events.ViewModels
{
    public class GuestBookingViewModel
    {
        public int GuestBookingId { get; set; }
        [Required]
        public int GuestId { get; set; }
        [Required]
        public int EventId { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime BookingDate { get; set; }
    }
}
