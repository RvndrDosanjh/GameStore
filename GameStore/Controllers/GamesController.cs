using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GameStore;
using GameStore.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;

namespace GameStore.Controllers
{
    public class GamesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
        public GamesController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Games
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Game.Include(g => g.Category).Include(g => g.Company).Include(g => g.Downloads);
            return View(await applicationDbContext.ToListAsync());
        }

        
        [HttpPost]
        public async Task<IActionResult> Index([FromForm] DateTime? from, [FromForm] DateTime? to, [FromForm] string name)
        {
            var games = await _context.Game.Include(g => g.Category).Include(g => g.Company).Include(g => g.Downloads)
                .Where(x => x.Name.Contains(name) &&
            (from == null || x.ReleaseDate>= from) && 
             (to == null || x.ReleaseDate<= to)          
            ).ToListAsync();

            //   var applicationDbContext = _context.Game.Include(g => g.Category).Include(g => g.Company).Include(g => g.Downloads);
            return View(games);
        }

        [HttpPost]
        [Route("Download")]
        public async Task<IActionResult> Download([FromForm] int id, [FromForm] string email, [FromForm] string Comment)
        {
            var game = await _context.Game.FirstOrDefaultAsync(x => x.GameId == id);
            _context.Downloads.Add(new Models.Download
            {
                Comment = Comment,
                Date = DateTime.Now,
                Email = email,
                GameId = id
            });
            _context.SaveChanges();
         //   var applicationDbContext = _context.Game.Include(g => g.Category).Include(g => g.Company).Include(g => g.Downloads);
            return Redirect($"/files/{game.Filename}");
        }

        // GET: Games/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Game
                .Include(g => g.Category)
                .Include(g => g.Company)
                .Include(g => g.Downloads)
                .Include(g => g.Updates)
                .FirstOrDefaultAsync(m => m.GameId == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // GET: Games/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryName");
            ViewData["CompanyId"] = new SelectList(_context.Company, "CompanyId", "CompanyName");
            return View();
        }

        // POST: Games/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GameId,Name,Filename,ImageFile,GameFile,DateAdded,CompanyId,CategoryId,ReleaseDate,Description")] Game game)
        {
            if (ModelState.IsValid)
            {
                _context.Add(game);
                await _context.SaveChangesAsync();
                var ms = game.ImageFile.OpenReadStream();
                var bytes = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(bytes, 0, (int)ms.Length);
                string filename = $"i{game.GameId}-{game.ImageFile.FileName.Replace(" ", "-")}";
                game.Image = filename;
                System.IO.File.WriteAllBytes(System.IO.Path.Combine(_env.WebRootPath, "files", filename), bytes);
                ms = game.GameFile.OpenReadStream();
                bytes = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(bytes, 0, (int)ms.Length);
                filename = $"g{game.GameId}-{game.GameFile.FileName.Replace(" ", "-")}";
                game.Filename = filename;
                System.IO.File.WriteAllBytes(System.IO.Path.Combine(_env.WebRootPath, "files", filename), bytes);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryName", game.CategoryId);
            ViewData["CompanyId"] = new SelectList(_context.Company, "CompanyId", "CompanyName", game.CompanyId);
            return View(game);
        }

        // GET: Games/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Game.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryName", game.CategoryId);
            ViewData["CompanyId"] = new SelectList(_context.Company, "CompanyId", "CompanyName", game.CompanyId);
            return View(game);
        }

        // POST: Games/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GameId,Name,Filename,ImageFile,GameFile,DateAdded,CompanyId,CategoryId,ReleaseDate,Description")] Game game)
        {
            if (id != game.GameId)
            {
                return NotFound();
            }


                try
                {
                    if (game.ImageFile != null)
                    {
                        var ms = game.ImageFile.OpenReadStream();
                        var bytes = new byte[ms.Length];
                        ms.Position = 0;
                        ms.Read(bytes, 0, (int)ms.Length);
                        string filename = $"i{game.GameId}-{game.ImageFile.FileName.Replace(" ", "-")}";
                        game.Image = filename;
                        System.IO.File.WriteAllBytes(System.IO.Path.Combine(_env.WebRootPath, "files", filename), bytes);
                    }
                    if (game.GameFile != null)
                    {
                        var ms = game.GameFile.OpenReadStream();
                        var bytes = new byte[ms.Length];
                        ms.Position = 0;
                        ms.Read(bytes, 0, (int)ms.Length);
                        var filename = $"g{game.GameId}-{game.GameFile.FileName.Replace(" ", "-")}";
                        game.Filename = filename;
                        System.IO.File.WriteAllBytes(System.IO.Path.Combine(_env.WebRootPath, "files", filename), bytes);

                    }
                        _context.Update(game);
                        await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameExists(game.GameId))
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

        // GET: Games/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Game
                .Include(g => g.Category)
                .Include(g => g.Company)
                .FirstOrDefaultAsync(m => m.GameId == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // POST: Games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var game = await _context.Game.FindAsync(id);
            _context.Game.Remove(game);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GameExists(int id)
        {
            return _context.Game.Any(e => e.GameId == id);
        }
    }
}
