using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventGo.Models
{
    public class Venue
    {
        [Key]
        public int VenueId { get; set; }
        [Required]
        [MaxLength(100)]
        
        public string Name { get; set; }
        [Required]
        public string Location { get; set; }
        [MaxLength(100)]
        public string Description { get; set; }
        [Range(1000, 100000)]
        public int Capacity { get; set; }
        public string Image { get; set; }

        public List<Concert> Concerts { get; set; }
    }
}
