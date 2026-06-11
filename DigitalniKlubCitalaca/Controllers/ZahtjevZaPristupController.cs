using System;
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

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ZahtjeviZaPristup
                .Include(z => z.CitalackaGrupa)
                .Include(z => z.Korisnik);

            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var zahtjevZaPristup = await _context.ZahtjeviZaPristup
                .Include(z => z.CitalackaGrupa)
                .Include(z => z.Korisnik)
                .FirstOrDefaultAsync(m => m.ZahtjevId == id);

            if (zahtjevZaPristup == null)
                return NotFound();

            return View(zahtjevZaPristup);
        }

        public IActionResult Create()
        {
            ViewData["GrupaId"] = new SelectList(_context.CitalackeGrupe, "GrupaId", "GrupaId");
            ViewData["KorisnikId"] = new SelectList(_context.Korisnici, "Id", "Id");
            return View();
        }

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
            ViewData["KorisnikId"] = new SelectList(_context.Korisnici, "Id", "Id", zahtjevZaPristup.KorisnikId);

            return View(zahtjevZaPristup);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var zahtjevZaPristup = await _context.ZahtjeviZaPristup.FindAsync(id);

            if (zahtjevZaPristup == null)
                return NotFound();

            ViewData["GrupaId"] = new SelectList(_context.CitalackeGrupe, "GrupaId", "GrupaId", zahtjevZaPristup.GrupaId);
            ViewData["KorisnikId"] = new SelectList(_context.Korisnici, "Id", "Id", zahtjevZaPristup.KorisnikId);

            return View(zahtjevZaPristup);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ZahtjevId,KorisnikId,GrupaId,DatumZahtjeva,StatusZahtjeva")] ZahtjevZaPristup zahtjevZaPristup)
        {
            if (id != zahtjevZaPristup.ZahtjevId)
                return NotFound();

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
                        return NotFound();

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["GrupaId"] = new SelectList(_context.CitalackeGrupe, "GrupaId", "GrupaId", zahtjevZaPristup.GrupaId);
            ViewData["KorisnikId"] = new SelectList(_context.Korisnici, "Id", "Id", zahtjevZaPristup.KorisnikId);

            return View(zahtjevZaPristup);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Odobri(int id)
        {
            var zahtjev = await _context.ZahtjeviZaPristup
                .Include(z => z.CitalackaGrupa)
                .FirstOrDefaultAsync(z => z.ZahtjevId == id);

            if (zahtjev == null)
                return NotFound();

            zahtjev.StatusZahtjeva = StatusZahtjeva.odobren;

            bool vecJeClan = await _context.ClanstvaGrupe
                .AnyAsync(c => c.KorisnikId == zahtjev.KorisnikId &&
                               c.GrupaId == zahtjev.GrupaId);

            if (!vecJeClan)
            {
                _context.ClanstvaGrupe.Add(new ClanstvoGrupe
                {
                    KorisnikId = zahtjev.KorisnikId,
                    GrupaId = zahtjev.GrupaId,
                    DatumPridruzivanja = DateTime.Now,
                    StatusClanstva = StatusClanstva.clan
                });
            }

            _context.Notifikacije.Add(new Notifikacija
            {
                KorisnikId = zahtjev.KorisnikId,
                Poruka = $"Vaš zahtjev za pristup grupi \"{zahtjev.CitalackaGrupa.Naziv}\" je odobren.",
                Link = Url.Action("Details", "CitalackaGrupa", new { id = zahtjev.GrupaId }),
                Datum = DateTime.Now,
                Procitana = false
            });

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Odbij(int id)
        {
            var zahtjev = await _context.ZahtjeviZaPristup
                .Include(z => z.CitalackaGrupa)
                .FirstOrDefaultAsync(z => z.ZahtjevId == id);

            if (zahtjev == null)
                return NotFound();

            zahtjev.StatusZahtjeva = StatusZahtjeva.odbijen;

            _context.Notifikacije.Add(new Notifikacija
            {
                KorisnikId = zahtjev.KorisnikId,
                Poruka = $"Vaš zahtjev za pristup grupi \"{zahtjev.CitalackaGrupa.Naziv}\" je odbijen.",
                Link = Url.Action("DostupneGrupe", "CitalackaGrupa"),
                Datum = DateTime.Now,
                Procitana = false
            });

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var zahtjevZaPristup = await _context.ZahtjeviZaPristup
                .Include(z => z.CitalackaGrupa)
                .Include(z => z.Korisnik)
                .FirstOrDefaultAsync(m => m.ZahtjevId == id);

            if (zahtjevZaPristup == null)
                return NotFound();

            return View(zahtjevZaPristup);
        }

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