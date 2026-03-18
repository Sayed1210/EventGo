using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EventGo.Models;

namespace EventGo.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly PadelContext _context;

        public PaymentsController(PadelContext context)
        {
            _context = context;
        }

        // GET: Payments
        public async Task<IActionResult> Index()
        {
            var padelContext = _context.Payments.Include(p => p.Booking);
            return View(await padelContext.ToListAsync());
        }

        // GET: Payments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payments
                .Include(p => p.Booking)
                .FirstOrDefaultAsync(m => m.PaymentID == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // GET: Payments/Create
        public IActionResult Create(int bookingId)
        {
            ViewData["BookingID"] = new SelectList(_context.Bookings, "BookingID", "BookingID");
            var userName = User.Identity.Name; // Get the username of the authenticated user
            ViewBag.Username = userName;
            return View();
        }

        // POST: Payments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PaymentID,BookingID,Amount,PaymentTime,IsPaid,PaymentMethod")] Payment payment)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = User.Identity.Name;
            if (payment!=null)
            {
                payment.Userid = userId;
                payment.username = userName;
              

                var booking = await _context.Bookings.FindAsync(payment.BookingID);
                if (booking != null)
                {
                    payment.Amount = booking.TotalAmount;
                    payment.PaymentTime = DateTime.Now;
                    payment.IsPaid = true; // Assuming payment is successful

                    _context.Add(payment);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("PaymentSuccess");
                }
                ModelState.AddModelError("", "Selected booking not found.");
            }
            ViewData["BookingID"] = new SelectList(_context.Bookings, "BookingID", "BookingID", payment.BookingID);
            return View(payment);
        }
        public IActionResult PaymentSuccess()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetBookingAmount(int bookingId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking != null)
            {
                return Json(new { amount = booking.TotalAmount });
            }
            return NotFound();
        }

        // GET: Payments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
            {
                return NotFound();
            }
            ViewData["BookingID"] = new SelectList(_context.Bookings, "BookingID", "BookingID", payment.BookingID);
            return View(payment);
        }

        // POST: Payments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PaymentID,BookingID,Amount,PaymentTime,IsPaid,PaymentMethod")] Payment payment)
        {
            if (id != payment.PaymentID)
            {
                return NotFound();
            }

            if (payment != null)
            {
                try
                {
                    _context.Update(payment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaymentExists(payment.PaymentID))
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
            ViewData["BookingID"] = new SelectList(_context.Bookings, "BookingID", "BookingID", payment.BookingID);
            return View(payment);
        }

        // GET: Payments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payments
                .Include(p => p.Booking)
                .FirstOrDefaultAsync(m => m.PaymentID == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // POST: Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment != null)
            {
                _context.Payments.Remove(payment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PaymentExists(int id)
        {
            return _context.Payments.Any(e => e.PaymentID == id);
        }
    }
}
