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
    public class KomentarController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KomentarController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Komentar
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Komentari.Include(k => k.Autor).Include(k => k.SadrzajGrupe);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Komentar/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var komentar = await _context.Komentari
                .Include(k => k.Autor)
                .Include(k => k.SadrzajGrupe)
                .FirstOrDefaultAsync(m => m.KomentarId == id);
            if (komentar == null)
            {
                return NotFound();
            }

            return View(komentar);
        }

        // GET: Komentar/Create
        public IActionResult Create()
        {
            ViewData["AutorId"] = new SelectList(_context.Korisnici, "KorisnikId", "KorisnikId");
            ViewData["SadrzajId"] = new SelectList(_context.SadrzajiGrupe, "SadrzajId", "SadrzajId");
            return View();
        }

        // POST: Komentar/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("KomentarId,SadrzajId,AutorId,Tekst,DatumKomentara,StatusKomentara")] Komentar komentar)
        {
            if (ModelState.IsValid)
            {
                _context.Add(komentar);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AutorId"] = new SelectList(_context.Korisnici, "KorisnikId", "KorisnikId", komentar.AutorId);
            ViewData["SadrzajId"] = new SelectList(_context.SadrzajiGrupe, "SadrzajId", "SadrzajId", komentar.SadrzajId);
            return View(komentar);
        }

        // GET: Komentar/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var komentar = await _context.Komentari.FindAsync(id);
            if (komentar == null)
            {
                return NotFound();
            }
            ViewData["AutorId"] = new SelectList(_context.Korisnici, "KorisnikId", "KorisnikId", komentar.AutorId);
            ViewData["SadrzajId"] = new SelectList(_context.SadrzajiGrupe, "SadrzajId", "SadrzajId", komentar.SadrzajId);
            return View(komentar);
        }

        // POST: Komentar/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("KomentarId,SadrzajId,AutorId,Tekst,DatumKomentara,StatusKomentara")] Komentar komentar)
        {
            if (id != komentar.KomentarId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(komentar);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KomentarExists(komentar.KomentarId))
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
            ViewData["AutorId"] = new SelectList(_context.Korisnici, "KorisnikId", "KorisnikId", komentar.AutorId);
            ViewData["SadrzajId"] = new SelectList(_context.SadrzajiGrupe, "SadrzajId", "SadrzajId", komentar.SadrzajId);
            return View(komentar);
        }

        // GET: Komentar/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var komentar = await _context.Komentari
                .Include(k => k.Autor)
                .Include(k => k.SadrzajGrupe)
                .FirstOrDefaultAsync(m => m.KomentarId == id);
            if (komentar == null)
            {
                return NotFound();
            }

            return View(komentar);
        }

        // POST: Komentar/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var komentar = await _context.Komentari.FindAsync(id);
            if (komentar != null)
            {
                _context.Komentari.Remove(komentar);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KomentarExists(int id)
        {
            return _context.Komentari.Any(e => e.KomentarId == id);
        }
    }
}
