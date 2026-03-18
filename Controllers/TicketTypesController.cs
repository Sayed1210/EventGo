using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EventGo.Models;

namespace EventGo.Controllers
{
    public class TicketTypesController : Controller
    {
        private readonly PadelContext _context;

        public TicketTypesController(PadelContext context)
        {
            _context = context;
        }

        // GET: TicketTypes
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Index()
        {
            var concertContext = _context.TicketTypes.Include(t => t.Concert);
            return View(await concertContext.ToListAsync());
        }

        // GET: TicketTypes/Details/5
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketType = await _context.TicketTypes
                .Include(t => t.Concert)
                .FirstOrDefaultAsync(m => m.TicketTypeId == id);
            if (ticketType == null)
            {
                return NotFound();
            }

            return View(ticketType);
        }

        // GET: TicketTypes/Create
        [Authorize(Roles = "Admin")]

        public IActionResult Create(int? concertId)
        {
            if (concertId == null)
            {
                return NotFound(); // Return an error if no ConcertId is provided
            }

            // Set the ConcertId in ViewData to pass it to the view
            ViewData["ConcertId"] = concertId;

            return View();
        }

        // POST: TicketTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [Authorize(Roles = "Admin")]

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TicketTypeId,TypeName,AvailableTickets,Price,Description,Image")] TicketType ticketType, int concertId)
        {
            if (ticketType != null)
            {
                // Ensure the ConcertId is correctly set
                ticketType.ConcertId = concertId;

                _context.Add(ticketType);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Concerts", new { id = concertId }); // Redirect back to the concert details
            }

            return View(ticketType);
        }

        // GET: TicketTypes/Edit/5
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketType = await _context.TicketTypes.FindAsync(id);
            if (ticketType == null)
            {
                return NotFound();
            }
            ViewData["ConcertId"] = new SelectList(_context.Concerts, "ConcertId", "ConcertId", ticketType.ConcertId);
            return View(ticketType);
        }

        // POST: TicketTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Edit(int id, [Bind("TicketTypeId,TypeName,Price,AvailableTickets,Description,Image,ConcertId")] TicketType ticketType)
        {
            if (id != ticketType.TicketTypeId)
            {
                return NotFound();
            }

            if (ticketType!=null)
            {
                try
                {
                    _context.Update(ticketType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketTypeExists(ticketType.TicketTypeId))
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
            ViewData["ConcertId"] = new SelectList(_context.Concerts, "ConcertId", "ConcertId", ticketType.ConcertId);
            return View(ticketType);
        }

        // GET: TicketTypes/Delete/5
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketType = await _context.TicketTypes
                .Include(t => t.Concert)
                .FirstOrDefaultAsync(m => m.TicketTypeId == id);
            if (ticketType == null)
            {
                return NotFound();
            }

            return View(ticketType);
        }

        // POST: TicketTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticketType = await _context.TicketTypes.FindAsync(id);
            if (ticketType != null)
            {
                _context.TicketTypes.Remove(ticketType);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketTypeExists(int id)
        {
            return _context.TicketTypes.Any(e => e.TicketTypeId == id);
        }

    }
}
