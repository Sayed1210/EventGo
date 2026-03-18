using System.ComponentModel.DataAnnotations;


namespace EventGo.Models
{
    public class PadelSession
    {
        [Key]
        public int SessionID { get; set; }
        public double Price { get; set; }

        public int ?userid { get; set; }
        public int PlaceID { get; set; }

        // Navigate properity 1 to m
        public Place ?Place { get; set; }
        // Navigate properity 1 to 1

        public Booking ?Bookings { get; set; }

    }
}
