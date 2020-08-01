using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GameStore.Data;
using GameStore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace GameStore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserRolesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserRolesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UserRoles
        public async Task<IActionResult> Index()
        {
            ViewBag.Users = await _context.Users.ToListAsync();
            ViewBag.Roles = await _context.Roles.ToListAsync();
            return View(await _context.UserRoles.Select(x => new UserRole { RoleId = x.RoleId, UserId= x.UserId }).ToListAsync());

        }

        // GET: UserRoles/Details/5
 

        // GET: UserRoles/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Users = new SelectList(await _context.Users.ToListAsync(), "Id", "UserName");
            ViewBag.Roles = new SelectList(await _context.Roles.ToListAsync(), "Id", "Name");
            return View();
        }

        // POST: UserRoles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,RoleId")] UserRole userRole)
        {
            if (ModelState.IsValid)
            {
                _context.UserRoles.Add(new IdentityUserRole<string> { RoleId = userRole.RoleId, UserId = userRole.UserId });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Users = new SelectList(await _context.Users.ToListAsync(), "Id", "UserName");
            ViewBag.Roles = new SelectList(await _context.Roles.ToListAsync(), "Id", "Name");
            return View(new UserRole { RoleId = userRole.RoleId, UserId = userRole.UserId });
        }

        // GET: UserRoles/Edit/5
        public async Task<IActionResult> Edit(string id)
        {

            if (id == null)
            {
                return NotFound();
            }
            string[] ids = id.Split(',');
            var userId = ids[0];
            var roleId = ids[1];
            var userRole = await _context.UserRoles.FirstOrDefaultAsync(x => x.UserId == userId && x.RoleId == roleId);
            if (userRole == null)
            {
                return NotFound();
            }
            ViewBag.Users = new SelectList(await _context.Users.ToListAsync(), "Id", "UserName", userId);
            ViewBag.Roles = new SelectList(await _context.Roles.ToListAsync(), "Id", "Name", roleId);
            return View(new UserRole { RoleId = userRole.RoleId, UserId = userRole.UserId });
        }

        // POST: UserRoles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("UserId,RoleId")] UserRole userRole)
        { 
            string[] ids = id.Split(',');
            var userId = ids[0];
            var roleId = ids[1];
            if (id != userRole.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userRole);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserRoleExists(userRole.UserId))
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
            ViewBag.Users = new SelectList(await _context.Users.ToListAsync(), "Id", "UserName", userId);
            ViewBag.Roles = new SelectList(await _context.Roles.ToListAsync(), "Id", "Name", roleId);
            return View(new UserRole { RoleId = userRole.RoleId, UserId = userRole.UserId });
        }

        // GET: UserRoles/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            string[] ids = id.Split(',');
            var userId = ids[0];
            var roleId = ids[1];
            if (id == null)
            {
                return NotFound();
            }

            var userRole = await _context.UserRoles
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (userRole == null)
            {
                return NotFound();
            }
            ViewBag.User = (await _context.Users.FirstOrDefaultAsync(x => x.Id == userId)).UserName;
            ViewBag.Roles = (await _context.Roles.FirstOrDefaultAsync(x => x.Id == roleId)).Name;
            return View(new UserRole { RoleId = userRole.RoleId, UserId = userRole.UserId });
        }

        // POST: UserRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var userRole = await _context.UserRoles.FindAsync(id);
            _context.UserRoles.Remove(userRole);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserRoleExists(string id)
        {
            return _context.UserRoles.Any(e => e.UserId == id);
        }
    }
}
