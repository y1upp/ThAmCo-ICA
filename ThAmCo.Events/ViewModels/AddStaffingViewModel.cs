using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ThAmCo.Events.ViewModels
{
    public class AddStaffingViewModel
    {
        [Required]
        public int EventId { get; set; }

        [Required(ErrorMessage = "Please select staff member")]
        [Display(Name = "Select Staff Member")]
        public int SelectedStaffId { get; set; }

        [Required(ErrorMessage = "Please select a role")]
        public string SelectedRole { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Assignment Date")]
        public DateTime AssignmentDate { get; set; }

        public List<SelectListItem> AvailableStaffMembers { get; set; }

        public List<SelectListItem> AvailableRoles { get; set; }
    }
}