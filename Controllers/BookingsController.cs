using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EventGo.Models;

namespace EventGo.Controllers
{
    [Authorize]
    public class BookingsController : Controller
    {
        private readonly PadelContext _context;
    


        public BookingsController(PadelContext context)
        {
            _context = context;
         

        }
        // GET: Booking/Index
        public async Task<IActionResult> Index()
        {
            // Get the currently authenticated user's ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Check if the user is an admin
            bool isAdmin = User.IsInRole("Admin"); // Adjust role name as necessary

            var bookingsQuery = from booking in _context.Bookings
                                join place in _context.Places on booking.PlaceId equals place.PlaceId // Joining tables
                                select new
                                {
                                    booking.BookingID,
                                    PlaceName = place.PlaceName, // Getting Place Name
                                    booking.BookingTime,
                                    booking.StartTime,
                                    booking.Hours,
                                    booking.TotalAmount,
                                    booking.User,
                                    booking.username// Use UserId to filter bookings
                                };

            // Filter bookings based on user role
            var bookings = isAdmin
                ? await bookingsQuery.ToListAsync() // Admin sees all bookings
                : await bookingsQuery.Where(b => b.User == userId).ToListAsync(); // Regular user sees only their bookings

            return View(bookings);
        }

        // GET: Booking/Create
        public async Task<IActionResult> Create(string id)
        {

            //ViewBag.Users = new SelectList(_context.Users, "UserID", "Username"); // Adjust according to your User model
            var userName = User.Identity.Name; // Get the username of the authenticated user
            ViewBag.Username = userName;


            ViewData["Places"] = new SelectList(_context.Places, "PlaceId", "PlaceName");
            return View();

        }
        

        // POST: Booking/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(Booking booking)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = User.Identity.Name;
            if (booking!=null)
            {
                booking.User = userId;
                booking.username = userName;

                // Calculate the end time based on StartTime and DurationHours
                DateTime endTime = booking.StartTime.AddHours(booking.Hours);
                booking.StartTime = new DateTime(booking.StartTime.Year, booking.StartTime.Month, booking.StartTime.Day, booking.StartTime.Hour, 0, 0);
                if (DateTime.Now > booking.StartTime)
                {
                    ModelState.AddModelError("", "You can't Book a previous Time!!");
                    ViewData["Places"] = new SelectList(_context.Places, "PlaceId", "PlaceName", booking.PlaceId);
                    ViewData["ErrorMessage"] = "You can't Book a previous Time!!";
                    return View(booking);
                }
                // Check if the place is available at the selected time
                bool isAvailable = await IsPlaceAvailable(booking.PlaceId, booking.StartTime, endTime);

                if (!isAvailable)
                {
                    ModelState.AddModelError("", "This place is already reserved for the selected time period.");
                    ViewData["Places"] = new SelectList(_context.Places, "PlaceId", "PlaceName", booking.PlaceId);
                    ViewData["ErrorMessage"] = "This place is already reserved for the selected time period.";
                    return View(booking);
                }
                var place = await _context.Places.FindAsync(booking.PlaceId);
                if (place != null)
                {
                    booking.TotalAmount = booking.Hours * place.Price; // Calculate total amount
                }

                booking.IsAvailable = true; // This might be redundant now
                booking.BookingTime = DateTime.Now;
                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["Places"] = new SelectList(_context.Places, "PlaceId", "PlaceName", booking.PlaceId);
            return View(booking);

        }

        private async Task<bool> IsPlaceAvailable(int placeId, DateTime startTime, DateTime endTime)
        {
            // Check for any overlapping bookings
            bool hasOverlap = await _context.Bookings
                .AnyAsync(b => b.PlaceId == placeId &&
                               b.StartTime < endTime &&
                               b.StartTime.AddHours(b.Hours) > startTime);

            return !hasOverlap;
        }



        // GET: Booking/Details/5
        public async Task<IActionResult> Details(int id)
        {
           

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: Booking/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            //ViewBag.Users = new SelectList(_context.Users, "UserID", "UserName", booking.UserID);
            ViewBag.Places = new SelectList(_context.Places, "PlaceId", "PlaceName", booking.PlaceId);
            return View(booking);
        }

        // POST: Booking/Edit/5
        // POST: Booking/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Booking booking)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = User.Identity.Name;
            if (id != booking.BookingID)
            {

                return NotFound();
            }

            if (booking != null)
            {
                var place = await _context.Places.FindAsync(booking.PlaceId);
                booking.TotalAmount = place.Price*booking.Hours;
                booking.User = userId;
                booking.username = userName;
                booking.BookingTime = DateTime.Now;
                _context.Update(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }

            //_context.Update(booking);
            //await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
            return View(booking);
        }

        // GET: Booking/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var booking = await _context.Bookings.FirstOrDefaultAsync(m => m.BookingID == id);

            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }
        // POST: Booking/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {    _context.Bookings.Remove(booking);
           
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        


    }
}
