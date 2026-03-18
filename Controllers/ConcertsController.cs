using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EventGo.Models;
using EventGo.ViewModel;

namespace EventGo.Controllers
{
    public class ConcertsController : Controller
    {
        private readonly PadelContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ConcertsController(PadelContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        [Authorize(Roles = "Admin")]

        public IActionResult Index(string? concertName)
        {
            // If a search query is provided, filter concerts by name
            var concerts = string.IsNullOrEmpty(concertName)
                ? _context.Concerts.Include(c => c.Venue).ToList()
                : _context.Concerts
                    .Where(c => c.Name.Contains(concertName))
                    .Include(c => c.Venue)
                    .ToList();

            return View(concerts);
        }

        public async Task<IActionResult> Purchase(int? concertId)
        {
            if (concertId == null)
            {
                return NotFound();
            }

            var concert = await _context.Concerts
                .Include(c => c.TicketTypes) // Include ticket types
                .FirstOrDefaultAsync(c => c.ConcertId == concertId);

            if (concert == null)
            {
                return NotFound();
            }

            var purchaseViewModel = new PurchaseViewModel
            {
                ConcertId = concert.ConcertId,
                ConcertName = concert.Name,
                TicketTypeSelections = concert.TicketTypes.Select(t => new TicketTypeSelection
                {
                    TicketTypeId = t.TicketTypeId,
                    TypeName = t.TypeName,
                    Price = t.Price,
                    AvailableTickets = t.AvailableTickets,
                    SelectedQuantity = 0 // Default selected quantity to 0
                }).ToList()
            };

            return View(purchaseViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessPurchase(int concertId, Dictionary<int, int> ticketQuantities)
        {
            if (ticketQuantities == null || !ticketQuantities.Any())
            {
                return RedirectToAction("Purchase", new { concertId });
            }

            var concert = await _context.Concerts
                .Include(c => c.TicketTypes)
                .FirstOrDefaultAsync(c => c.ConcertId == concertId);

            if (concert == null)
            {
                return NotFound();
            }

            foreach (var ticketQuantity in ticketQuantities)
            {
                var ticketType = concert.TicketTypes.FirstOrDefault(t => t.TicketTypeId == ticketQuantity.Key);

                if (ticketType != null && ticketQuantity.Value > 0 && ticketQuantity.Value <= ticketType.AvailableTickets)
                {
                    // Create tickets for the selected quantity
                    for (int i = 0; i < ticketQuantity.Value; i++)
                    {
                        _context.Tickets.Add(new Ticket
                        {
                            PurchaseDate = DateTime.Now,
                            TicketTypeId = ticketType.TicketTypeId
                        });
                    }

                    // Update the available tickets for the ticket type
                    ticketType.AvailableTickets -= ticketQuantity.Value;
                }
            }


            await _context.SaveChangesAsync();

            // Redirect to the payment confirmation page (or further process)
            return RedirectToAction("Payment", new { concertId });
        }

        public IActionResult Payment(int concertId)
        {
            var concert = _context.Concerts
                .Include(c => c.TicketTypes)
                .FirstOrDefault(c => c.ConcertId == concertId);

            if (concert == null)
            {
                return NotFound();
            }

            return View(concert);
        }




        [Authorize(Roles = "Admin")]

        // GET: Concert/Details/5
        public IActionResult Details(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var concert = _context.Concerts
                    .Include(c => c.Venue) // Including the Venue details
                    .Include(c => c.TicketTypes) // Including related TicketTypes
                    .FirstOrDefault(c => c.ConcertId == id);

                if (concert == null)
                {
                    return NotFound();
                }

                return View(concert);
            
        }



        [Authorize(Roles = "Admin")]

        // GET: Concerts/Create
        public IActionResult Create()
        {
            ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "Name");
            return View();
        }


        public async Task<IActionResult> ConcertDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Fetch the concert and its associated ticket types and venue
            var concert = await _context.Concerts
                .Include(c => c.Venue)
                .Include(c => c.TicketTypes)  // Include TicketTypes
                .FirstOrDefaultAsync(c => c.ConcertId == id);

            if (concert == null)
            {
                return NotFound();
            }

            // Create the ViewModel
            var viewModel = new ConcertDetailsViewModel
            {
                Concert = concert,
                TicketTypes = concert.TicketTypes.ToList()
            };

            return View(viewModel);
        }



        // POST: Concerts/Create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ConcertId,Name,Description,Artist,Organizer,Date,Image,VenueId")] Concert concert, IFormFile imageFile)
        {
            if (concert!=null)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    // Set custom path for concert images
                    string fileName = Path.GetFileNameWithoutExtension(imageFile.FileName);
                    string extension = Path.GetExtension(imageFile.FileName);
                    fileName = $"{fileName}{extension}"; // Add a timestamp to ensure uniqueness

                    string webRootPath = _hostingEnvironment.WebRootPath;
                    string imagesFolder = Path.Combine(webRootPath, "images/concerts"); // Custom folder for concert images
                    string filePath = Path.Combine(imagesFolder, fileName);

                    if (!Directory.Exists(imagesFolder))
                    {
                        Directory.CreateDirectory(imagesFolder);
                    }

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    concert.Image = $"/images/concerts/{fileName}"; // Store the path in the database
                }

                _context.Add(concert);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id = concert.ConcertId });
            }
            ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "Name", concert.VenueId);
            return View(concert);
        }

        // GET: Concerts/Edit/5
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var concert = await _context.Concerts.FindAsync(id);
            if (concert == null)
            {
                return NotFound();
            }

            // Pass the current image path to the view
            ViewData["CurrentImage"] = concert.Image;

            ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "Location", concert.VenueId);
            return View(concert);
        }

        // POST: Concerts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Edit(int id, [Bind("ConcertId,Name,Description,Artist,Organizer,Date,VenueId")] Concert concert, IFormFile imageFile)
        {
            if (id != concert.ConcertId)
            {
                return NotFound();
            }

            if (concert!=null)
            {
                try
                {
                    // Fetch the existing concert record
                    var existingConcert = await _context.Concerts.AsNoTracking().FirstOrDefaultAsync(c => c.ConcertId == id);

                    if (existingConcert == null)
                    {
                        return NotFound();
                    }

                    if (imageFile != null && imageFile.Length > 0)
                    {
                        // Delete the old image if it exists
                        if (!string.IsNullOrEmpty(existingConcert.Image))
                        {
                            string oldImagePath = Path.Combine(_hostingEnvironment.WebRootPath, existingConcert.Image.TrimStart('/'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        // Upload the new image
                        string fileName = Path.GetFileNameWithoutExtension(imageFile.FileName);
                        string extension = Path.GetExtension(imageFile.FileName);
                        fileName = $"{fileName}{extension}";

                        string webRootPath = _hostingEnvironment.WebRootPath;
                        string imagesFolder = Path.Combine(webRootPath, "images/concerts");
                        string filePath = Path.Combine(imagesFolder, fileName);

                        if (!Directory.Exists(imagesFolder))
                        {
                            Directory.CreateDirectory(imagesFolder);
                        }

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }

                        // Set the new image path
                        concert.Image = $"/images/concerts/{fileName}";
                    }
                    else
                    {
                        // Keep the existing image if no new image is uploaded
                        concert.Image = existingConcert.Image;
                    }

                    // Update concert details
                    _context.Update(concert);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConcertExists(concert.ConcertId))
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

            ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "Location", concert.VenueId);
            return View(concert);
        }

        // Helper method to check if concert exists
        private bool ConcertExists(int id)
        {
            return _context.Concerts.Any(e => e.ConcertId == id);
        }

        // Add Search Feature
        public IActionResult SearchByName(string? concertName)
        {
            if (concertName != null)
            {
                var concerts = _context.Concerts.Where(o => o.Name.StartsWith(concertName)).ToList();
                return View("Index", concerts);
            }
            return View("Index", _context.Concerts.ToList());
        }
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var concert = await _context.Concerts
                .FirstOrDefaultAsync(m => m.ConcertId == id);
            if (concert == null)
            {
                return NotFound();
            }

            return View(concert);
        }

        // POST: Venues/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var concert = await _context.Concerts.FindAsync(id);
            if (concert != null)
            {
                if (!string.IsNullOrEmpty(concert.Image))
                {
                    string imagePath = Path.Combine(_hostingEnvironment.WebRootPath, concert.Image.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }
                _context.Concerts.Remove(concert);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        
    }
}
