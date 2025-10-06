using BAL.model;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace DAL.models
{
    public class Booking : BaseClass
    {
        public string UserId { get; set; }
        [ValidateNever]
        public AppUser AppUser { get; set; } = null!;
        public int FlightId { get; set; } 
        [ValidateNever]
        public Flight Flight { get; set; } = null!;
        public DateTime BookingDate { get; set; }
        public string PNR { get; set; } // unique
        // اشوف هو حجز كام تذكرة عن طريق الفرن كي و احسبهم 
        // اية فيهم مميز و اية عادي 
        public decimal TotalPrice { get; set; }
        public Status Status { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
        [ValidateNever]
        public Payment? Payment { get; set; }

        [ValidateNever]
        public ICollection<Ticket> Tickets { get; set; } = new HashSet<Ticket>();
        [ValidateNever]
        public ICollection<BookingAddOn> BookingAddOns { get; set; } = new HashSet<BookingAddOn>();

    }

    public enum Status
    {
        Pending = 1,
        Confirmed = 2,
        Cancelled = 3
    }

    //حساب TotalPrice
    //الـ TotalPrice في Booking هيجمع كل حاجة:
    //BasePrice للرحلة.
    //AddOn، بنضيفهم من جدول BookingAddOns.
    //مثال تطبيقي:
    //Flight.BasePrice = 2000 جنيه × 2 مقعد (Business) = 10,000 جنيه.
    //BookingAddOns:
    //2 حقيبة × 500 جنيه = 1000 جنيه.
    //مقعد مميز × 300 جنيه = 300 جنيه.
    //TotalPrice = 10,000 + 1000 + 300 = 11,300 جنيه.
}
