using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EventGo.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace EventGo.Controllers
{
    public class VenuesController : Controller
    {
        private readonly PadelContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public VenuesController(PadelContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<IActionResult> UserIndex()
        {
            return View(await _context.Venues.ToListAsync());
        }

        public IActionResult Privacy()
        {
            return View();
        }



        // GET: Venues
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Venues.ToListAsync());
        }


        public async Task<IActionResult> UserConcerts(int Id)
        {
            // Retrieve the venue details based on the VenueId
            Venue ven = _context.Venues.FirstOrDefault(e => e.VenueId == Id);
            ViewBag.Location = ven.Location;
            ViewBag.Name = ven.Name;

            // Get the concerts for the specified venue and include the related ConcertArtist and Artist
            var concerts = await _context.Concerts.Where(c => c.VenueId == Id).ToListAsync();

            return View(concerts);
        }

        // Search venues by name
        [Authorize(Roles = "Admin")]
        public IActionResult SearchByName(string? venueName)
        {
            if (!string.IsNullOrEmpty(venueName))
            {
                var venues = _context.Venues.Where(v => v.Name.StartsWith(venueName)).ToList();
                return View("Index", venues);
            }
            return View("Index", _context.Venues.ToList());
        }
        public IActionResult SearchByConcertUser(string? concertName)
        {
            if (!string.IsNullOrEmpty(concertName))
            {
                var concerts = _context.Concerts.Where(v => v.Name.StartsWith(concertName)).ToList();
                return View("UserConcerts", concerts);
            }
            return View("UserConcerts", _context.Concerts.ToList());
        }
        public IActionResult SearchByNameUser(string? venueName)
        {
            if (!string.IsNullOrEmpty(venueName))
            {
                var venues = _context.Venues.Where(v => v.Name.StartsWith(venueName)).ToList();
                return View("UserIndex", venues);
            }
            return View("UserIndex", _context.Venues.ToList());
        }

        // GET: Venues/Details/5
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venue = await _context.Venues
                .FirstOrDefaultAsync(m => m.VenueId == id);
            if (venue == null)
            {
                return NotFound();
            }

            return View(venue);
        }

        // GET: Venues/Create
        public IActionResult Create()
        {
            return View();
        }

        
        // POST: Venues/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Create([Bind("VenueId,Name,Location,Description,Capacity")] Venue venue, IFormFile imageFile)
        {
            if (venue != null)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    // Define the custom folder for venue images
                    string fileName = Path.GetFileNameWithoutExtension(imageFile.FileName);
                    string extension = Path.GetExtension(imageFile.FileName);
                    fileName = $"{fileName}{extension}";
                    
                    // Custom folder path
                    string customFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads", "venues");
                    string filePath = Path.Combine(customFolderPath, fileName);

                    // Check if the custom folder exists, create it if it doesn't
                    if (!Directory.Exists(customFolderPath))
                    {
                        Directory.CreateDirectory(customFolderPath);
                    }

                    // Upload the image file to the specified folder
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    // Store the path of the uploaded image in the database (relative path)
                    venue.Image = $"/uploads/venues/{fileName}";
                }

                _context.Add(venue);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(venue);
        }

        // GET: Venues/Edit/5
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venue = await _context.Venues.FindAsync(id);
            if (venue == null)
            {
                return NotFound();
            }

            // Pass the current image path to the view
            ViewData["CurrentImage"] = venue.Image;

            return View(venue);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Edit(int id, [Bind("VenueId,Name,Location,Description,Capacity")] Venue venue, IFormFile imageFile)
        {
            if (id != venue.VenueId)
            {
                return NotFound();
            }

            if (venue != null)
            {
                try
                {
                    var existingVenue = await _context.Venues.AsNoTracking().FirstOrDefaultAsync(v => v.VenueId == id);

                    if (existingVenue == null)
                    {
                        return NotFound();
                    }

                    // Check if a new image is uploaded
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        // Delete the old image if it exists
                        if (!string.IsNullOrEmpty(existingVenue.Image))
                        {
                            string oldImagePath = Path.Combine(_hostingEnvironment.WebRootPath, existingVenue.Image.TrimStart('/'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        // Upload the new image
                        string fileName = Path.GetFileNameWithoutExtension(imageFile.FileName);
                        string extension = Path.GetExtension(imageFile.FileName);
                        fileName = $"{fileName}{extension}";

                        // Custom folder path
                        string customFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads", "venues");
                        string filePath = Path.Combine(customFolderPath, fileName);

                        if (!Directory.Exists(customFolderPath))
                        {
                            Directory.CreateDirectory(customFolderPath);
                        }

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }

                        // Set the new image path
                        venue.Image = $"/uploads/venues/{fileName}";
                    }
                    else
                    {
                        // Keep the existing image if no new image is uploaded
                        venue.Image = existingVenue.Image;
                    }

                    _context.Update(venue);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VenueExists(venue.VenueId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(venue);
        }

        // GET: Venues/Delete/5
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venue = await _context.Venues
                .FirstOrDefaultAsync(m => m.VenueId == id);
            if (venue == null)
            {
                return NotFound();
            }

            return View(venue);
        }

        // POST: Venues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venue = await _context.Venues.FindAsync(id);
            if (venue != null)
            {
                if (!string.IsNullOrEmpty(venue.Image))
                {
                    string imagePath = Path.Combine(_hostingEnvironment.WebRootPath, venue.Image.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }
                _context.Venues.Remove(venue);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool VenueExists(int id)
        {
            return _context.Venues.Any(e => e.VenueId == id);
        }
    }
}
