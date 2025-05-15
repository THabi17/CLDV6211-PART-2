
using System.Data;
using System.Net.Mime;
using Azure.Storage.Blobs;
using EventEase_WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventEase.Controllers
{
    public class VenueController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VenueController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Venue.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Venue venue)
        {
            if (ModelState.IsValid)
            {

                if (venue.ImageFile != null)
                {
                    var blobUrl = await UploadingImageToBlobAsync(venue.ImageFile);

                    venue.ImageUrl = blobUrl;
                }
                _context.Add(venue);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Venue created succesfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(venue);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var venue = await _context.Venue.FindAsync(id);
            if (venue == null) return NotFound();

            return View(venue);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Venue venue)
        {
            if (id != venue.Venue_ID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if( venue.ImageFile != null)
                    {
                        var blobUrl = await UploadingImageToBlobAsync(venue.ImageFile);

                        venue.ImageUrl = blobUrl;
                    }
                    else
                    {
                        
                    }

                    _context.Update(venue);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Venue updated succesfully.";
                    
                }
                catch (DbUpdateConcurrencyException) 
                {
                    if (!VenueExists(venue.Venue_ID))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(venue);
        }

        // STEP 1: Confirm Deletion (GET)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var venue = await _context.Venue.FirstOrDefaultAsync(v => v.Venue_ID == id);
            if (venue == null) return NotFound();

            return View(venue);
        }

        // STEP 2: Perform Deletion (POST) - Logic to restrict the deletion of venues associated with active bookings.
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venue = await _context.Venue.FindAsync(id);
            if (venue == null) return NotFound();

            var hasBookings = await _context.Booking.AnyAsync(b => b.Venue_ID == id);
            if (hasBookings)
            {
                TempData["ErrorMessage"] = "Cannot delete venue because it has existing bookings.";
                return RedirectToAction(nameof(Index));
            }

            _context.Venue.Remove(venue);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Venue deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venue = await _context.Venue
              .FirstOrDefaultAsync(m => m.Venue_ID == id);

            if (venue == null)
            {
                return NotFound();
            }

            return View(venue);
        }

        private async Task<string> UploadingImageToBlobAsync(IFormFile imageFile)
        {
            var connectionString = "DefaultEndpointsProtocol=https;AccountName=eventstoragepart2;AccountKey=aRQ4ca2+BbCn+v4sVLNq+bMim3QiViPSMJGZqXxrAtWK4T0/nsQFwjcm54LCcc6c1H+uMtlcKqUh+AStyf9mlA==;EndpointSuffix=core.windows.net";
            var containerName = "eventcontainer";

            var blobServiceClient = new BlobServiceClient(connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(Guid.NewGuid() + Path.GetExtension(imageFile.Name));

            var blobHttpHeaders = new Azure.Storage.Blobs.Models.BlobHttpHeaders()
            {
                ContentType = imageFile.ContentType
            };
            using (var stream = imageFile.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, new Azure.Storage.Blobs.Models.BlobUploadOptions
                {
                    HttpHeaders = blobHttpHeaders
                });
            }
            return blobClient.Uri.ToString();
        }

        private bool VenueExists(int id) 
        {
            return _context.Venue.Any(e=> e.Venue_ID == id);

        }
    }

}