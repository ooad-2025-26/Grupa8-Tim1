using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DigitalniKlubCitalaca.Data;
using DigitalniKlubCitalaca.Models;

namespace DigitalniKlubCitalaca.Controllers
{
    public class ProfilController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProfilController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Profil
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Profili.Include(p => p.Korisnik);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Profil/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profil = await _context.Profili
                .Include(p => p.Korisnik)
                .FirstOrDefaultAsync(m => m.ProfilId == id);
            if (profil == null)
            {
                return NotFound();
            }

            return View(profil);
        }

        // GET: Profil/Create
        public IActionResult Create()
        {
            ViewData["KorisnikId"] = new SelectList(_context.Korisnici, "KorisnikId", "KorisnikId");
            return View();
        }

        // POST: Profil/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProfilId,KorisnikId,OpisProfila,Lokacija")] Profil profil)
        {
            if (ModelState.IsValid)
            {
                _context.Add(profil);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["KorisnikId"] = new SelectList(_context.Korisnici, "KorisnikId", "KorisnikId", profil.KorisnikId);
            return View(profil);
        }

        // GET: Profil/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profil = await _context.Profili.FindAsync(id);
            if (profil == null)
            {
                return NotFound();
            }
            ViewData["KorisnikId"] = new SelectList(_context.Korisnici, "KorisnikId", "KorisnikId", profil.KorisnikId);
            return View(profil);
        }

        // POST: Profil/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProfilId,KorisnikId,OpisProfila,Lokacija")] Profil profil)
        {
            if (id != profil.ProfilId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(profil);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProfilExists(profil.ProfilId))
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
            ViewData["KorisnikId"] = new SelectList(_context.Korisnici, "KorisnikId", "KorisnikId", profil.KorisnikId);
            return View(profil);
        }

        // GET: Profil/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profil = await _context.Profili
                .Include(p => p.Korisnik)
                .FirstOrDefaultAsync(m => m.ProfilId == id);
            if (profil == null)
            {
                return NotFound();
            }

            return View(profil);
        }

        // POST: Profil/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var profil = await _context.Profili.FindAsync(id);
            if (profil != null)
            {
                _context.Profili.Remove(profil);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProfilExists(int id)
        {
            return _context.Profili.Any(e => e.ProfilId == id);
        }
    }
}
