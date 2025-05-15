using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventEase_WebApp.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {



        }

        public DbSet<Event> Event { get; set; }
        public DbSet<Venue> Venue { get; set; }
        public DbSet<Booking> Booking { get; set; }

        
    }
}
