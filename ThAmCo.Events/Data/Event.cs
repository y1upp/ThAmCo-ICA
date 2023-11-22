using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;


namespace ThAmCo.Events.Data
{
    public class Event
    {

        // add catering later?
        [ForeignKey("EventType")]
        public int EventTypeId { get; set; }
        public int EventId { get; set; }
        
        [Required]
        public String Title { get; set; }
        
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }
        
        [Required]
        public int VenueId { get; set; }

        public ICollection<GuestBooking> GuestBookings { get; set; }
        public ICollection<Staffing> Staffings { get; set; }
        public EventType EventType { get; set; }
        public Reservation Reservation { get; set; }
    }
}
