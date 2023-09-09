using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Models;

namespace TaskManager.Controllers
{
    public class Tasks : Controller
    {
        private readonly ApplicationDbContext _context;

        public Tasks(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Tasks
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var userEmail = @User.Identity?.Name;
            return View(await _context.TaskObj.Where(t => t.UserEmail==userEmail).ToListAsync());
        }

        // GET: Tasks/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskObj = await _context.TaskObj
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taskObj == null)
            {
                return NotFound();
            }

            return View(taskObj);
        }

        // GET: Tasks/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DueDay,Priority,Description")] TaskObj taskObj)
        {
            var userEmail = @User.Identity?.Name;
            taskObj.UserEmail = userEmail ?? "no user found!";
            if (ModelState.IsValid)
            {
                _context.Add(taskObj);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(taskObj);
        }

        // GET: Tasks/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskObj = await _context.TaskObj
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taskObj == null)
            {
                return NotFound();
            }

            return View(taskObj);
        }

        // POST: Tasks/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var taskObj = await _context.TaskObj.FindAsync(id);
            if (taskObj != null)
            {
                _context.TaskObj.Remove(taskObj);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaskObjExists(int id)
        {
          return (_context.TaskObj?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
