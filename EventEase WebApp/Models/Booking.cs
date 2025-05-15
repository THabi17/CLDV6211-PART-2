using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventEase_WebApp.Models
{
    public class Booking
    {
        [Key]
        public int Booking_ID { get; set; }

        [Required]
        public int Event_ID { get; set; }

        [ForeignKey(nameof(Event_ID))]
        public Event? Event { get; set; }

        [Required]
        public int Venue_ID { get; set; }

        [ForeignKey(nameof(Venue_ID))]
        public Venue? Venue { get; set; }
        public DateTime BookingDate { get; set; } = DateTime.Now;

    }
}
