using EventGo.Models;
using System.Collections.Generic;
namespace EventGo.ViewModel
{
    public class PurchaseViewModel
    {
        public int ConcertId { get; set; }
        public string ConcertName { get; set; }
        public List<TicketTypeSelection> TicketTypeSelections { get; set; }
    }

    public class TicketTypeSelection
    {
        public int TicketTypeId { get; set; }
        public string TypeName { get; set; }
        public double Price { get; set; }
        public int AvailableTickets { get; set; }
        public int SelectedQuantity { get; set; } // User input for number of tickets
    }
}
