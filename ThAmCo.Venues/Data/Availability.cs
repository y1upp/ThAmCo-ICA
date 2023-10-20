﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThAmCo.Venues.Data
{
    public class Availability
    {
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [MinLength(5), MaxLength(5)]
        public string VenueCode { get; set; }

        [ForeignKey(nameof(VenueCode))]
        public Venue Venue { get; set; }

        [Range(0.0, Double.MaxValue)]
        public double CostPerHour { get; set; }

        public Reservation Reservation { get; set; }
    }
}
