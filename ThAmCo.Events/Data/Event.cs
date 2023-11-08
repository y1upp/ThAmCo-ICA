using System.ComponentModel.DataAnnotations;

namespace ThAmCo.Events.Data
{
    public class Event
    {

        // add catering later?
        public int EventId { get; set; }
        [Required]
        public String Title { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }
        [Required]
        public int VenueId { get; set; }
    }
}
