using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DigitalniKlubCitalaca.Data;
using DigitalniKlubCitalaca.Models;
using Microsoft.AspNetCore.Identity;

namespace DigitalniKlubCitalaca.Controllers
{
    public class CitalackaGrupaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Korisnik> _userManager;

        public CitalackaGrupaController(
            ApplicationDbContext context,
            UserManager<Korisnik> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var korisnik = await _userManager.GetUserAsync(User);

            if (korisnik == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            var mojeGrupe = await _context.ClanstvaGrupe
                .Where(c => c.KorisnikId == korisnik.Id)
                .Select(c => c.GrupaId)
                .ToListAsync();

            var grupe = await _context.CitalackeGrupe
                .Where(g => mojeGrupe.Contains(g.GrupaId))
                .ToListAsync();

            return View(grupe);
        }

        public async Task<IActionResult> DostupneGrupe(string? regija, string? zanr)
        {
            var korisnik = await _userManager.GetUserAsync(User);

            if (korisnik == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            var mojeGrupe = await _context.ClanstvaGrupe
                .Where(c => c.KorisnikId == korisnik.Id)
                .Select(c => c.GrupaId)
                .ToListAsync();

            var mojiZahtjevi = await _context.ZahtjeviZaPristup
                .Where(z => z.KorisnikId == korisnik.Id &&
                            z.StatusZahtjeva == StatusZahtjeva.na_cekanju)
                .Select(z => z.GrupaId)
                .ToListAsync();

            var dostupneGrupeQuery = _context.CitalackeGrupe
                .Where(g => !mojeGrupe.Contains(g.GrupaId) &&
                            !mojiZahtjevi.Contains(g.GrupaId))
                .AsQueryable();

            ViewBag.Regije = await dostupneGrupeQuery
                .Where(g => !string.IsNullOrWhiteSpace(g.Regija))
                .Select(g => g.Regija)
                .Distinct()
                .OrderBy(r => r)
                .ToListAsync();

            ViewBag.Zanrovi = await dostupneGrupeQuery
                .Where(g => !string.IsNullOrWhiteSpace(g.Zanr))
                .Select(g => g.Zanr)
                .Distinct()
                .OrderBy(z => z)
                .ToListAsync();

            if (!string.IsNullOrWhiteSpace(regija))
            {
                dostupneGrupeQuery = dostupneGrupeQuery
                    .Where(g => g.Regija == regija);
            }

            if (!string.IsNullOrWhiteSpace(zanr))
            {
                dostupneGrupeQuery = dostupneGrupeQuery
                    .Where(g => g.Zanr == zanr);
            }

            ViewBag.OdabranaRegija = regija;
            ViewBag.OdabraniZanr = zanr;

            var dostupneGrupe = await dostupneGrupeQuery
                .OrderByDescending(g => g.DatumKreiranja)
                .ToListAsync();

            return View(dostupneGrupe);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PridruziSe(int id)
        {
            var korisnik = await _userManager.GetUserAsync(User);

            if (korisnik == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            var grupa = await _context.CitalackeGrupe.FindAsync(id);

            if (grupa == null)
                return NotFound();

            bool vecJeClan = await _context.ClanstvaGrupe
                .AnyAsync(c => c.KorisnikId == korisnik.Id && c.GrupaId == id);

            bool vecPoslaoZahtjev = await _context.ZahtjeviZaPristup
                .AnyAsync(z => z.KorisnikId == korisnik.Id &&
                               z.GrupaId == id &&
                               z.StatusZahtjeva == StatusZahtjeva.na_cekanju);

            if (vecJeClan || vecPoslaoZahtjev)
                return RedirectToAction(nameof(DostupneGrupe));

            if (grupa.TipGrupe == TipGrupe.javna)
            {
                _context.ClanstvaGrupe.Add(new ClanstvoGrupe
                {
                    KorisnikId = korisnik.Id,
                    GrupaId = grupa.GrupaId,
                    DatumPridruzivanja = DateTime.Now,
                    StatusClanstva = StatusClanstva.clan
                });

                _context.Notifikacije.Add(new Notifikacija
                {
                    KorisnikId = korisnik.Id,
                    Poruka = $"Uspješno ste se pridružili grupi \"{grupa.Naziv}\".",
                    Link = Url.Action("Details", "CitalackaGrupa", new { id = grupa.GrupaId }),
                    Datum = DateTime.Now,
                    Procitana = false
                });
            }
            else
            {
                _context.ZahtjeviZaPristup.Add(new ZahtjevZaPristup
                {
                    KorisnikId = korisnik.Id,
                    GrupaId = grupa.GrupaId,
                    DatumZahtjeva = DateTime.Now,
                    StatusZahtjeva = StatusZahtjeva.na_cekanju
                });

                _context.Notifikacije.Add(new Notifikacija
                {
                    KorisnikId = korisnik.Id,
                    Poruka = $"Vaš zahtjev za pristup grupi \"{grupa.Naziv}\" je poslan.",
                    Link = Url.Action("DostupneGrupe", "CitalackaGrupa"),
                    Datum = DateTime.Now,
                    Procitana = false
                });

                var admini = await _context.Korisnici
                    .Where(k => k.Uloga == Uloga.administrator)
                    .ToListAsync();

                foreach (var admin in admini)
                {
                    if (admin.Id == korisnik.Id)
                        continue;

                    _context.Notifikacije.Add(new Notifikacija
                    {
                        KorisnikId = admin.Id,
                        Poruka = $"Novi zahtjev za pristup privatnoj grupi \"{grupa.Naziv}\".",
                        Link = Url.Action("Index", "ZahtjevZaPristup"),
                        Datum = DateTime.Now,
                        Procitana = false
                    });
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(DostupneGrupe));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var citalackaGrupa = await _context.CitalackeGrupe
                .FirstOrDefaultAsync(m => m.GrupaId == id);

            if (citalackaGrupa == null) return NotFound();

            ViewBag.Objave = await _context.SadrzajiGrupe
                .Include(s => s.Autor)
                .Where(s => s.GrupaId == id)
                .OrderByDescending(s => s.DatumObjave)
                .ToListAsync();

            return View(citalackaGrupa);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GrupaId,Naziv,Opis,Zanr,Regija,Grad,MaksimalanBrojClanova,TipGrupe")] CitalackaGrupa citalackaGrupa)
        {
            ModelState.Remove("DatumKreiranja");

            citalackaGrupa.DatumKreiranja = DateTime.Now;

            if (ModelState.IsValid)
            {
                _context.Add(citalackaGrupa);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(citalackaGrupa);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var citalackaGrupa = await _context.CitalackeGrupe.FindAsync(id);

            if (citalackaGrupa == null) return NotFound();

            return View(citalackaGrupa);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GrupaId,Naziv,Opis,Zanr,Regija,Grad,MaksimalanBrojClanova,TipGrupe,DatumKreiranja")] CitalackaGrupa citalackaGrupa)
        {
            if (id != citalackaGrupa.GrupaId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(citalackaGrupa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CitalackaGrupaExists(citalackaGrupa.GrupaId))
                        return NotFound();

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(citalackaGrupa);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var citalackaGrupa = await _context.CitalackeGrupe
                .FirstOrDefaultAsync(m => m.GrupaId == id);

            if (citalackaGrupa == null) return NotFound();

            return View(citalackaGrupa);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var citalackaGrupa = await _context.CitalackeGrupe.FindAsync(id);

            if (citalackaGrupa != null)
            {
                _context.CitalackeGrupe.Remove(citalackaGrupa);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool CitalackaGrupaExists(int id)
        {
            return _context.CitalackeGrupe.Any(e => e.GrupaId == id);
        }
    }
}