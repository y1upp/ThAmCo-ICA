using System.ComponentModel.DataAnnotations;

namespace ThAmCo.Events.Data
{
    public class GuestBooking
    {
        public int GuestBookingId { get; set; }
        
        [Required]
        public int GuestId { get; set; }
        public Guest Guest { get; set; }
        
        [Required]
        public int EventId { get; set; }
        public Event Event { get; set; }
        
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime BookingDate { get; set; }   
    }
}
