using System;

namespace EventGo.Models
{
    public class Payment
    {

        public int PaymentID { get; set; }

        public int BookingID { get; set; }

        public double Amount { get; set; }

        public DateTime PaymentTime { get; set; }
        public string Userid { get; set; }
        public string username { get; set; }

        public bool IsPaid { get; set; }

        public string PaymentMethod { get; set; }

        // Navigate properity
        public Booking Booking { get; set; } = null!;
    }
}
