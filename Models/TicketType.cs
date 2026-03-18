using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace EventGo.Models
{
    public class TicketType
    {
        [Key]
        public int TicketTypeId { get; set; }
        public String TypeName { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string? Image { get; set; }
        public int AvailableTickets { get; set; }

        [ForeignKey("ConcertId")]
        public int ConcertId { get; set; }
        public Concert Concert { get; set; }
        public List<Ticket> Tickets { get; set; }

    }
}
