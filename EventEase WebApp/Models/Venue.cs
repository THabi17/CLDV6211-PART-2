﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventEase_WebApp.Models
{
    public class Venue
    {
        [Key]
        [Column("Venue_ID")]
        public int Venue_ID { get; set; }

        [Required]
        public string? VenueName { get; set; }

        [Required]
        public string? Location { get; set; }

        [Required]
        [Range(1, int.MaxValue,ErrorMessage = "Capacity must be greater than 0")]
        public int Capacity { get; set; }
        public string? ImageUrl { get; set; }
        

        [NotMapped]
        public IFormFile? ImageFile { get; set; }   
    }
}
