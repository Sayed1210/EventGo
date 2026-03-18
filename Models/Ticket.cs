using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace EventGo.Models
{
    public class Ticket
    {
        [Key]
        public int TicketId { get; set; }
        public DateTime PurchaseDate { get; set; }

        [ForeignKey("TicketTypeId")]
        public int TicketTypeId { get; set; }
        public TicketType TicketType { get; set; }
    }
}
