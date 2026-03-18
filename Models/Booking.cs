using System;
using System.ComponentModel.DataAnnotations;

namespace EventGo.Models
{
    public class Booking
    {
        [Key]
        public int BookingID { get; set; }
        public DateTime BookingTime { get; set; }
        public bool IsAvailable { get; set; }
        public int PlaceId { get; set; }
        public string User{ get; set; }
        public string username { get; set; }
        public DateTime StartTime { get; set; }

        [Required]
        [Range(1, 24, ErrorMessage = "Duration must be between 1 and 24 hours")]
        public int Hours { get; set; }
        public double TotalAmount { get; set; }

        // Navigate properities
        // 1 to m 

        // 1 to 1
        public Place ?place { get; set; }

        // 1 to 1
        public Payment ?Payment { get; set; }
    }
}
