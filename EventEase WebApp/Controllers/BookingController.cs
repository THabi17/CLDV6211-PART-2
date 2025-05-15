
using EventEase_WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



namespace EventEase.Controllers
{
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(String searchString)
        {
            var bookings = _context.Booking
                .Include(b => b.Event)
                .Include(b => b.Venue)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                bookings = bookings.Where(b =>
                b.Venue.VenueName.Contains(searchString) ||
                b.Event.EventName.Contains(searchString));

            }
            return View(await bookings.ToListAsync());
        }

        public IActionResult Create()
        {
            ViewData["Events"] = _context.Event.ToList();
            ViewData["Venues"] = _context.Venue.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Booking booking)
        {
            var selectedEvent = await _context.Event.FirstOrDefaultAsync(e => e.Event_ID == booking.Event_ID);

            if (selectedEvent == null)
            {
                ModelState.AddModelError("", "Selected event not found.");
                ViewData["Events"] = _context.Event.ToList();
                ViewData["Venues"] = _context.Venue.ToList();
                return View(booking);
            }

            // Check manually for double booking
            var conflict = await _context.Booking
                .Include(b => b.Event)
                .AnyAsync(b => b.Venue_ID == booking.Venue_ID &&
                               b.Event.EventDate.Date== selectedEvent.EventDate.Date);

            if (conflict)
            {
                ModelState.AddModelError("", "This venue is already booked for that date.");
                ViewData["Events"] = _context.Event.ToList();
                ViewData["Venues"] = _context.Venue.ToList();
                return View(booking);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(booking);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Booking created successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    // If database constraint fails (e.g., unique key violation), show friendly message
                    ModelState.AddModelError("", "This venue is already booked for that date.");
                    ViewData["Events"] = _context.Event.ToList();
                    ViewData["Venues"] = _context.Venue.ToList();
                    return View(booking);
                }
            }

            ViewData["Events"] = _context.Event.ToList();
            ViewData["Venues"] = _context.Venue.ToList();
            return View(booking);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking
                .Include(b => b.Event)
                .Include(b => b.Venue)
                .FirstOrDefaultAsync(m => m.Booking_ID == id);

            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }
    }
}
