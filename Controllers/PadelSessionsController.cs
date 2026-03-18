using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EventGo.Models;

namespace EventGo.Controllers
{
    public class PadelSessionsController : Controller
    {
        private readonly PadelContext _context;

        public PadelSessionsController(PadelContext context)
        {
            _context = context;
        }

        // GET: PadelSessions
        public async Task<IActionResult> Index()
        {
            var padelContext = _context.PadelSessions.Include(p => p.Place);
            return View(await padelContext.ToListAsync());
        }

        // GET: PadelSessions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var padelSession = await _context.PadelSessions
                .Include(p => p.Place)
                .FirstOrDefaultAsync(m => m.SessionID == id);
            if (padelSession == null)
            {
                return NotFound();
            }

            return View(padelSession);
        }

        // GET: PadelSessions/Create
        public IActionResult Create()
        {
            ViewData["PlaceID"] = new SelectList(_context.Places, "PlaceId", "PlaceId");
            return View();
        }

        // POST: PadelSessions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SessionID,Price,userid,PlaceID")] PadelSession padelSession)
        {
            if (ModelState.IsValid)
            {
                _context.Add(padelSession);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PlaceID"] = new SelectList(_context.Places, "PlaceId", "PlaceId", padelSession.PlaceID);
            return View(padelSession);
        }

        // GET: PadelSessions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var padelSession = await _context.PadelSessions.FindAsync(id);
            if (padelSession == null)
            {
                return NotFound();
            }
            ViewData["PlaceID"] = new SelectList(_context.Places, "PlaceId", "PlaceId", padelSession.PlaceID);
            return View(padelSession);
        }

        // POST: PadelSessions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SessionID,Price,userid,PlaceID")] PadelSession padelSession)
        {
            if (id != padelSession.SessionID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(padelSession);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PadelSessionExists(padelSession.SessionID))
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
            ViewData["PlaceID"] = new SelectList(_context.Places, "PlaceId", "PlaceId", padelSession.PlaceID);
            return View(padelSession);
        }

        // GET: PadelSessions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var padelSession = await _context.PadelSessions
                .Include(p => p.Place)
                .FirstOrDefaultAsync(m => m.SessionID == id);
            if (padelSession == null)
            {
                return NotFound();
            }

            return View(padelSession);
        }

        // POST: PadelSessions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var padelSession = await _context.PadelSessions.FindAsync(id);
            if (padelSession != null)
            {
                _context.PadelSessions.Remove(padelSession);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PadelSessionExists(int id)
        {
            return _context.PadelSessions.Any(e => e.SessionID == id);
        }
    }
}
