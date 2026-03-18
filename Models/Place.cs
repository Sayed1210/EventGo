using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventGo.Models
{
    public class Place
    {
        [Key]
        public int PlaceId { get; set; }

        public string ?ImagePath { get; set; }
        public string ?ImageName { get; set; }
        public string PlaceName { get; set; }

        public string Address { get; set; }

        public int Capacity { get; set; }
        public double Price { get; set; }

        public string ?Description { get; set; }

        // Navigate properity
        public ICollection<PadelSession> PadelSessions { get; set; }

    }
}
