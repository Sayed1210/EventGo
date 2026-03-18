using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace EventGo.Models
{
    public class Concert
    {
        [Key]
        public int ConcertId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Organizer {  get; set; }
        public string Artist {  get; set; }
        public DateTime Date { get; set; }
        public string Image { get; set; }
        public int VenueId { get; set; }
        [ForeignKey("VenueId")]
        public Venue Venue { get; set; }
        public List<Ticket> Tickets { get; set; }
        public List<TicketType> TicketTypes { get; set; }


    }
}
