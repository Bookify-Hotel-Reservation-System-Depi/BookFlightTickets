using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace DAL.models
{
    public class Seat : BaseClass
    {
        public int AirplaneId { get; set; } 
        [ValidateNever]
        public Airplane Airplane { get; set; } = null!;

        public string Row { get; set; }  
        public short Number { get; set; } 

        public SeatClass Class { get; set; }
        public bool IsAvailable { get; set; } = true;

        [ValidateNever]
        public ICollection<Ticket> Tickets { get; set; } = new HashSet<Ticket>(); 
    }

    public enum SeatClass
    {
        Economy = 1,
        Business = 2,
        First = 3
    }
}
