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
    public class ZahtjevZaPristupController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ZahtjevZaPristupController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ZahtjevZaPristup
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ZahtjeviZaPristup.Include(z => z.CitalackaGrupa).Include(z => z.Korisnik);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ZahtjevZaPristup/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zahtjevZaPristup = await _context.ZahtjeviZaPristup
                .Include(z => z.CitalackaGrupa)
                .Include(z => z.Korisnik)
                .FirstOrDefaultAsync(m => m.ZahtjevId == id);
            if (zahtjevZaPristup == null)
            {
                return NotFound();
            }

            return View(zahtjevZaPristup);
        }

        // GET: ZahtjevZaPristup/Create
        public IActionResult Create()
        {
            ViewData["GrupaId"] = new SelectList(_context.CitalackeGrupe, "GrupaId", "GrupaId");
            ViewData["KorisnikId"] = new SelectList(_context.Korisnici, "KorisnikId", "KorisnikId");
            return View();
        }

        // POST: ZahtjevZaPristup/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ZahtjevId,KorisnikId,GrupaId,DatumZahtjeva,StatusZahtjeva")] ZahtjevZaPristup zahtjevZaPristup)
        {
            if (ModelState.IsValid)
            {
                _context.Add(zahtjevZaPristup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GrupaId"] = new SelectList(_context.CitalackeGrupe, "GrupaId", "GrupaId", zahtjevZaPristup.GrupaId);
            ViewData["KorisnikId"] = new SelectList(_context.Korisnici, "KorisnikId", "KorisnikId", zahtjevZaPristup.KorisnikId);
            return View(zahtjevZaPristup);
        }

        // GET: ZahtjevZaPristup/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zahtjevZaPristup = await _context.ZahtjeviZaPristup.FindAsync(id);
            if (zahtjevZaPristup == null)
            {
                return NotFound();
            }
            ViewData["GrupaId"] = new SelectList(_context.CitalackeGrupe, "GrupaId", "GrupaId", zahtjevZaPristup.GrupaId);
            ViewData["KorisnikId"] = new SelectList(_context.Korisnici, "KorisnikId", "KorisnikId", zahtjevZaPristup.KorisnikId);
            return View(zahtjevZaPristup);
        }

        // POST: ZahtjevZaPristup/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ZahtjevId,KorisnikId,GrupaId,DatumZahtjeva,StatusZahtjeva")] ZahtjevZaPristup zahtjevZaPristup)
        {
            if (id != zahtjevZaPristup.ZahtjevId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(zahtjevZaPristup);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ZahtjevZaPristupExists(zahtjevZaPristup.ZahtjevId))
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
            ViewData["GrupaId"] = new SelectList(_context.CitalackeGrupe, "GrupaId", "GrupaId", zahtjevZaPristup.GrupaId);
            ViewData["KorisnikId"] = new SelectList(_context.Korisnici, "KorisnikId", "KorisnikId", zahtjevZaPristup.KorisnikId);
            return View(zahtjevZaPristup);
        }

        // GET: ZahtjevZaPristup/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zahtjevZaPristup = await _context.ZahtjeviZaPristup
                .Include(z => z.CitalackaGrupa)
                .Include(z => z.Korisnik)
                .FirstOrDefaultAsync(m => m.ZahtjevId == id);
            if (zahtjevZaPristup == null)
            {
                return NotFound();
            }

            return View(zahtjevZaPristup);
        }

        // POST: ZahtjevZaPristup/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var zahtjevZaPristup = await _context.ZahtjeviZaPristup.FindAsync(id);
            if (zahtjevZaPristup != null)
            {
                _context.ZahtjeviZaPristup.Remove(zahtjevZaPristup);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ZahtjevZaPristupExists(int id)
        {
            return _context.ZahtjeviZaPristup.Any(e => e.ZahtjevId == id);
        }
    }
}
