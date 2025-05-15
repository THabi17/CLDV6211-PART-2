using System.ComponentModel.DataAnnotations;

namespace EventEase_WebApp.Models
{
    public class Event
    {
        [Key]
        public int Event_ID { get; set; }
        
        public int? Venue_ID {get; set; }


        public string? EventName { get; set; }
        [Required]

        public string? Description { get; set; }

        [Required]
        public DateTime EventDate { get; set; }

        public Venue? Venue { get; set; }    

        public List<Booking> Booking { get; set; } = new();
    }
}
