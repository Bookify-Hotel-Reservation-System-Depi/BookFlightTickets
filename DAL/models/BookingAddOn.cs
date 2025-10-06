using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace DAL.models
{
    public class BookingAddOn : BaseClass
    {
        public int BookingID { get; set; } 
        [ValidateNever]
        public Booking Booking { get; set; } = null!;
        public int AddOnID { get; set; } 
        [ValidateNever]
        public AddOn AddOn { get; set; } = null!;
        public short Quantity { get; set; }
        public decimal TotalPrice { get; set; }      
    }
}