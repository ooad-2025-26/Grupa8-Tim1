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
    public class KnjigaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KnjigaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Knjiga
        public async Task<IActionResult> Index()
        {
            return View(await _context.Knjige.ToListAsync());
        }

        // GET: Knjiga/Details/5
        // GET: Knjiga/Details/5
public async Task<IActionResult> Details(int? id)
{
    if (id == null)
    {
        return NotFound();
    }

    var knjiga = await _context.Knjige
        .FirstOrDefaultAsync(m => m.KnjigaId == id);

    if (knjiga == null)
    {
        return NotFound();
    }

    ViewBag.Komentari = await _context.Komentari
        .Include(k => k.Autor)
        .OrderByDescending(k => k.DatumKomentara)
        .Take(5)
        .ToListAsync();

    return View(knjiga);
}
        // POST: Knjiga/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("KnjigaId,Naziv,Autor,Opis,BrojStranica,Zanr")] Knjiga knjiga)
        {
            if (ModelState.IsValid)
            {
                _context.Add(knjiga);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(knjiga);
        }

        // GET: Knjiga/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var knjiga = await _context.Knjige.FindAsync(id);
            if (knjiga == null)
            {
                return NotFound();
            }
            return View(knjiga);
        }

        // POST: Knjiga/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("KnjigaId,Naziv,Autor,Opis,BrojStranica,Zanr")] Knjiga knjiga)
        {
            if (id != knjiga.KnjigaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(knjiga);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KnjigaExists(knjiga.KnjigaId))
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
            return View(knjiga);
        }

        // GET: Knjiga/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var knjiga = await _context.Knjige
                .FirstOrDefaultAsync(m => m.KnjigaId == id);
            if (knjiga == null)
            {
                return NotFound();
            }

            return View(knjiga);
        }

        // POST: Knjiga/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var knjiga = await _context.Knjige.FindAsync(id);
            if (knjiga != null)
            {
                _context.Knjige.Remove(knjiga);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> DodajKomentar(int knjigaId, string tekst)
{
    if (string.IsNullOrWhiteSpace(tekst))
    {
        TempData["KomentarGreska"] = "Komentar ne može biti prazan.";
        return RedirectToAction(nameof(Details), new { id = knjigaId });
    }

    var prviSadrzaj = await _context.SadrzajiGrupe.FirstOrDefaultAsync();

    var prviKorisnik = await _context.Korisnici.FirstOrDefaultAsync();

    if (prviSadrzaj == null || prviKorisnik == null)
    {
        TempData["KomentarGreska"] = "Nije moguće dodati komentar jer nedostaje sadržaj ili korisnik.";
        return RedirectToAction(nameof(Details), new { id = knjigaId });
    }

    var komentar = new Komentar
    {
        Tekst = tekst,
        DatumKomentara = DateTime.Now,
        SadrzajId = prviSadrzaj.SadrzajId,
        AutorId = prviKorisnik.Id
    };

    _context.Komentari.Add(komentar);
    await _context.SaveChangesAsync();

    TempData["KomentarPoruka"] = "Komentar je uspješno objavljen.";

    return RedirectToAction(nameof(Details), new { id = knjigaId });
}
        private bool KnjigaExists(int id)
        {
            return _context.Knjige.Any(e => e.KnjigaId == id);
        }
    }
}
