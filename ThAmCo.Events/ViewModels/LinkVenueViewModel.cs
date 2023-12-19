using ThAmCo.Events.Data;
using ThAmCo.Venues.Data;

namespace ThAmCo.Events.ViewModels {
public class LinkVenueViewModel
{
    public Event Event { get; set; }
    public List<Venue> AvailableVenues { get; set; }
    public int SelectedVenueId { get; set; }
    
    }

}
