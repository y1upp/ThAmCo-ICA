using System.ComponentModel.DataAnnotations;
using ThAmCo.Events.Data;

namespace ThAmCo.Events.ViewModels
{
    public class EventViewModel
    {
        public int EventId { get; set; }
        [Required]
        public String Title { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }
        [Required]
        public int VenueId { get; set; } 
        public ICollection<Staffing> Staffings { get; set; }
    }
}
