using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace DAL.models
{
    public class Ticket : BaseClass
    {
        public int BookingID { get; set; } 
        [ValidateNever]
        public Booking Booking { get; set; } = null!;
        public string TicketNumber { get; set; } // unique
        public string? Title { get; set; }
        public int SeatId { get; set; } // المفتاح الأجنبي للمقعد
        [ValidateNever]
        public Seat Seat { get; set; } = null!;

    }
}
