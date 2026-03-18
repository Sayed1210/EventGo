using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EventGo.Models;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace EventGo.Controllers
{
    public class PlacesController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ILogger<PlacesController> _logger;
        private readonly PadelContext _context;

        public PlacesController(PadelContext context, ILogger<PlacesController> logger, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
        }


        // GET: Places
        public async Task<IActionResult> Index()
        {
            var places = await _context.Places.ToListAsync();
            return View(places); 
        }
        public IActionResult Search(string? search)
        {
            if (search!=null)
            {
                var query = _context.Places.Where(o => o.PlaceName.StartsWith(search) || o.Address.StartsWith(search)).ToList();
                return View("Index", query);
            }

            return View("Index", _context.Places.ToList());
        }

        // GET: Places/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var place = await _context.Places
                .FirstOrDefaultAsync(m => m.PlaceId == id);
            if (place == null)
            {
                return NotFound();
            }

            return View(place);
        }

        // GET: Places/Create
        [Authorize(Roles ="Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Places/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PlaceId,PlaceName,Address,Capacity,Description,Price")] Place place, IFormFile imageFile)
        {
            _logger.LogInformation("Create method called");

            if (place!=null)
            {
                _logger.LogInformation("ModelState is valid");
                
                
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        _logger.LogInformation($"Image file received. Name: {imageFile.FileName}, Size: {imageFile.Length} bytes");

                        string fileName = Path.GetFileNameWithoutExtension(imageFile.FileName);
                        string extension = Path.GetExtension(imageFile.FileName);
                        fileName = $"{fileName}{extension}";

                        string webRootPath = _hostingEnvironment.WebRootPath;
                        _logger.LogInformation($"WebRootPath: {webRootPath}");

                        string imagesFolder = Path.Combine(webRootPath, "images");
                        _logger.LogInformation($"Images folder path: {imagesFolder}");

                        string filePath = Path.Combine(imagesFolder, fileName);
                        _logger.LogInformation($"Full file path: {filePath}");

                        if (!Directory.Exists(imagesFolder))
                        {
                            _logger.LogInformation($"Creating directory: {imagesFolder}");
                            Directory.CreateDirectory(imagesFolder);
                        }

                        _logger.LogInformation("Attempting to save file");
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }
                        _logger.LogInformation("File saved successfully");

                        place.ImageName = fileName;
                        place.ImagePath = $"/images/{fileName}";
                        _logger.LogInformation($"Image name set to: {place.ImageName}");
                        _logger.LogInformation($"Image path set to: {place.ImagePath}");
                    }
                    _context.Add(place);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation($"Place saved to database. ID: {place.PlaceId}");
                    return RedirectToAction(nameof(Index));
                

            }
            return View(place);
        }
        // GET: Places/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var place = await _context.Places.FindAsync(id);
            if (place == null)
            {
                return NotFound();
            }
            return View(place);
        }

        // POST: Places/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PlaceId,PlaceName,Address,Capacity,Description,ImagePath,ImageName,Price")] Place place, IFormFile imageFile)
        {
            if (id != place.PlaceId)
            {
                return NotFound();
            }

            if (place != null)
            {
                try
                {
                    // If an image is uploaded
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        // Generate a unique file name and get the extension
                        string fileName = Path.GetFileNameWithoutExtension(imageFile.FileName);
                        string extension = Path.GetExtension(imageFile.FileName);
                        fileName = fileName + extension;

                        // Define the path where the file will be saved
                        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);

                        // Save the file
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }

                        // Set the image path and name in the place object
                        place.ImageName = fileName;
                        place.ImagePath = "/images/" + fileName;
                    }

                    _context.Update(place);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlaceExists(place.PlaceId))
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
            return View(place);
        }

        // GET: Places/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var place = await _context.Places
                .FirstOrDefaultAsync(m => m.PlaceId == id);
            if (place == null)
            {
                return NotFound();
            }

            return View(place);
        }

        // POST: Places/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var place = await _context.Places.FindAsync(id);
            if (place != null)
            {
                _context.Places.Remove(place);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlaceExists(int id)
        {
            return _context.Places.Any(e => e.PlaceId == id);
        }

    }
}
