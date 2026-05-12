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
    public class ClanstvoGrupeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClanstvoGrupeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ClanstvoGrupe
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ClanstvaGrupe.Include(c => c.CitalackaGrupa).Include(c => c.Korisnik);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ClanstvoGrupe/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clanstvoGrupe = await _context.ClanstvaGrupe
                .Include(c => c.CitalackaGrupa)
                .Include(c => c.Korisnik)
                .FirstOrDefaultAsync(m => m.ClanstvoId == id);
            if (clanstvoGrupe == null)
            {
                return NotFound();
            }

            return View(clanstvoGrupe);
        }

        // GET: ClanstvoGrupe/Create
        public IActionResult Create()
        {
            ViewData["GrupaId"] = new SelectList(_context.CitalackeGrupe, "GrupaId", "GrupaId");
            ViewData["KorisnikId"] = new SelectList(_context.Korisnici, "KorisnikId", "KorisnikId");
            return View();
        }

        // POST: ClanstvoGrupe/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClanstvoId,KorisnikId,GrupaId,DatumPridruzivanja,StatusClanstva")] ClanstvoGrupe clanstvoGrupe)
        {
            if (ModelState.IsValid)
            {
                _context.Add(clanstvoGrupe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GrupaId"] = new SelectList(_context.CitalackeGrupe, "GrupaId", "GrupaId", clanstvoGrupe.GrupaId);
            ViewData["KorisnikId"] = new SelectList(_context.Korisnici, "KorisnikId", "KorisnikId", clanstvoGrupe.KorisnikId);
            return View(clanstvoGrupe);
        }

        // GET: ClanstvoGrupe/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clanstvoGrupe = await _context.ClanstvaGrupe.FindAsync(id);
            if (clanstvoGrupe == null)
            {
                return NotFound();
            }
            ViewData["GrupaId"] = new SelectList(_context.CitalackeGrupe, "GrupaId", "GrupaId", clanstvoGrupe.GrupaId);
            ViewData["KorisnikId"] = new SelectList(_context.Korisnici, "KorisnikId", "KorisnikId", clanstvoGrupe.KorisnikId);
            return View(clanstvoGrupe);
        }

        // POST: ClanstvoGrupe/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClanstvoId,KorisnikId,GrupaId,DatumPridruzivanja,StatusClanstva")] ClanstvoGrupe clanstvoGrupe)
        {
            if (id != clanstvoGrupe.ClanstvoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(clanstvoGrupe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClanstvoGrupeExists(clanstvoGrupe.ClanstvoId))
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
            ViewData["GrupaId"] = new SelectList(_context.CitalackeGrupe, "GrupaId", "GrupaId", clanstvoGrupe.GrupaId);
            ViewData["KorisnikId"] = new SelectList(_context.Korisnici, "KorisnikId", "KorisnikId", clanstvoGrupe.KorisnikId);
            return View(clanstvoGrupe);
        }

        // GET: ClanstvoGrupe/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clanstvoGrupe = await _context.ClanstvaGrupe
                .Include(c => c.CitalackaGrupa)
                .Include(c => c.Korisnik)
                .FirstOrDefaultAsync(m => m.ClanstvoId == id);
            if (clanstvoGrupe == null)
            {
                return NotFound();
            }

            return View(clanstvoGrupe);
        }

        // POST: ClanstvoGrupe/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var clanstvoGrupe = await _context.ClanstvaGrupe.FindAsync(id);
            if (clanstvoGrupe != null)
            {
                _context.ClanstvaGrupe.Remove(clanstvoGrupe);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClanstvoGrupeExists(int id)
        {
            return _context.ClanstvaGrupe.Any(e => e.ClanstvoId == id);
        }
    }
}
