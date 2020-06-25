using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GameStore.Data;
using GameStore.Models;

namespace GameStore.Controllers
{
    public class UpdatesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UpdatesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Updates
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Update.Include(u => u.Game);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Updates/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var update = await _context.Update
                .Include(u => u.Game)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (update == null)
            {
                return NotFound();
            }

            return View(update);
        }

        // GET: Updates/Create
        public IActionResult Create()
        {
            ViewData["GameId"] = new SelectList(_context.Game, "GameId", "Name");
            return View();
        }

        // POST: Updates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,GameId,DateAdded")] Update update)
        {
            if (ModelState.IsValid)
            {
                _context.Add(update);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GameId"] = new SelectList(_context.Game, "GameId", "Name", update.GameId);
            return View(update);
        }

        // GET: Updates/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var update = await _context.Update.FindAsync(id);
            if (update == null)
            {
                return NotFound();
            }
            ViewData["GameId"] = new SelectList(_context.Game, "GameId", "Name", update.GameId);
            return View(update);
        }

        // POST: Updates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,GameId,DateAdded")] Update update)
        {
            if (id != update.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(update);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UpdateExists(update.Id))
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
            ViewData["GameId"] = new SelectList(_context.Game, "GameId", "Name", update.GameId);
            return View(update);
        }

        // GET: Updates/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var update = await _context.Update
                .Include(u => u.Game)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (update == null)
            {
                return NotFound();
            }

            return View(update);
        }

        // POST: Updates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var update = await _context.Update.FindAsync(id);
            _context.Update.Remove(update);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UpdateExists(int id)
        {
            return _context.Update.Any(e => e.Id == id);
        }
    }
}
