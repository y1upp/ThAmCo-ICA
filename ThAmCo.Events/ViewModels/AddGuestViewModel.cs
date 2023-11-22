using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ThAmCo.Events.ViewModels
{
    public class AddGuestViewModel
    {
        public int EventId { get; set; }

        [Required]
        [Display(Name = "Select Guest")]
        public int SelectedGuestId { get; set; }

        public List<SelectListItem> AvailableGuests { get; set; }
    }
}
