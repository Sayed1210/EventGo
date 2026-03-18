using EventGo.Models;
using System.Collections.Generic;

namespace EventGo.ViewModel
{
    public class ConcertDetailsViewModel
    {

        public Concert Concert { get; set; }
        public List<TicketType> TicketTypes { get; set; }


    }

}
