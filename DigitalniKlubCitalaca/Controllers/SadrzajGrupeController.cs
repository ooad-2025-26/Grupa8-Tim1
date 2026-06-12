using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DigitalniKlubCitalaca.Data;
using DigitalniKlubCitalaca.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace DigitalniKlubCitalaca.Controllers
{
    public class SadrzajGrupeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SadrzajGrupeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var sada = DateTime.Now;

            var sadrzaji = _context.SadrzajiGrupe
                .Include(s => s.Autor)
                .Include(s => s.CitalackaGrupa)
                .Where(s => s.TipSadrzaja != TipSadrzaja.story
                            || s.DatumObjave.AddHours(24) > sada);

            return View(await sadrzaji.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var sadrzajGrupe = await _context.SadrzajiGrupe
                .Include(s => s.Autor)
                .Include(s => s.CitalackaGrupa)
                .FirstOrDefaultAsync(m => m.SadrzajId == id);

            if (sadrzajGrupe == null) return NotFound();

            if (sadrzajGrupe.TipSadrzaja == TipSadrzaja.story &&
                sadrzajGrupe.DatumObjave.AddHours(24) <= DateTime.Now)
            {
                return NotFound();
            }

            ViewBag.Komentari = await _context.Komentari
                .Include(k => k.Autor)
                .Where(k => k.SadrzajId == id)
                .OrderByDescending(k => k.DatumKomentara)
                .ToListAsync();

            var korisnikId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var korisnik = await _context.Korisnici
                .FirstOrDefaultAsync(k => k.Id == korisnikId);

            ViewBag.JeModerator = korisnik != null && korisnik.Uloga == Uloga.moderator;

            return View(sadrzajGrupe);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DodajKomentar(int sadrzajId, string tekst)
        {
            var korisnikId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (korisnikId == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (string.IsNullOrWhiteSpace(tekst))
                return RedirectToAction("Details", new { id = sadrzajId });

            var komentar = new Komentar
            {
                SadrzajId = sadrzajId,
                AutorId = korisnikId,
                Tekst = tekst,
                DatumKomentara = DateTime.Now
            };

            _context.Komentari.Add(komentar);
            await _context.SaveChangesAsync();

            var sadrzaj = await _context.SadrzajiGrupe
                .Include(s => s.CitalackaGrupa)
                .FirstOrDefaultAsync(s => s.SadrzajId == sadrzajId);

            if (sadrzaj != null &&
                !string.IsNullOrEmpty(sadrzaj.AutorId) &&
                sadrzaj.AutorId != korisnikId)
            {
                _context.Notifikacije.Add(new Notifikacija
                {
                    KorisnikId = sadrzaj.AutorId,
                    Poruka = $"Novi komentar na vašoj objavi u grupi \"{sadrzaj.CitalackaGrupa.Naziv}\".",
                    Link = Url.Action("Details", "SadrzajGrupe", new { id = sadrzaj.SadrzajId }),
                    Datum = DateTime.Now,
                    Procitana = false
                });

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Details", new { id = sadrzajId });
        }

        [Authorize]
        public async Task<IActionResult> Create(int grupaId)
        {
            var korisnikId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var trenutniKorisnik = await _context.Korisnici
                .FirstOrDefaultAsync(k => k.Id == korisnikId);

            if (trenutniKorisnik == null)
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            if (trenutniKorisnik.Uloga == Uloga.administrator)
            {
                TempData["Greska"] = "Administrator ne može objavljivati sadržaj u čitalačkim grupama.";
                return RedirectToAction("Details", "CitalackaGrupa", new { id = grupaId });
            }

            var objava = new SadrzajGrupe
            {
                GrupaId = grupaId,
                DatumObjave = DateTime.Now
            };

            return View(objava);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([FromForm] SadrzajGrupe sadrzajGrupe, [FromForm] IFormFile? pdfFile)
        {
            ModelState.Remove("Autor");
            ModelState.Remove("CitalackaGrupa");
            ModelState.Remove("AutorId");
            ModelState.Remove("DatumObjave");
            ModelState.Remove("Link");

            var korisnikId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var trenutniKorisnik = await _context.Korisnici
    .FirstOrDefaultAsync(k => k.Id == korisnikId);

            if (trenutniKorisnik == null)
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            if (trenutniKorisnik.Uloga == Uloga.administrator)
            {
                TempData["Greska"] = "Administrator ne može objavljivati sadržaj u čitalačkim grupama.";
                return RedirectToAction("Details", "CitalackaGrupa", new { id = sadrzajGrupe.GrupaId });
            }


            if (korisnikId == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            sadrzajGrupe.AutorId = korisnikId;
            sadrzajGrupe.DatumObjave = DateTime.Now;
            if (string.IsNullOrWhiteSpace(sadrzajGrupe.Link))
            {
                sadrzajGrupe.Link = string.Empty;
            }

            if (string.IsNullOrWhiteSpace(sadrzajGrupe.Opis))
                sadrzajGrupe.Opis = string.Empty;

            if (sadrzajGrupe.TipSadrzaja == TipSadrzaja.pdf)
            {
                if (pdfFile == null || pdfFile.Length == 0)
                    ModelState.AddModelError("pdfFile", "Morate odabrati PDF dokument.");
            }

            if (sadrzajGrupe.TipSadrzaja == TipSadrzaja.story)
            {
                if (string.IsNullOrWhiteSpace(sadrzajGrupe.Link))
                    ModelState.AddModelError("Link", "Morate odabrati sliku ili uslikati story.");
            }

            if (!ModelState.IsValid)
                return View(sadrzajGrupe);

            if (sadrzajGrupe.TipSadrzaja == TipSadrzaja.pdf && pdfFile != null)
            {
                var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(pdfFile.FileName);
                var filePath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await pdfFile.CopyToAsync(stream);
                }

                sadrzajGrupe.Link = "/uploads/" + fileName;
            }

            _context.SadrzajiGrupe.Add(sadrzajGrupe);
            await _context.SaveChangesAsync();

            var grupa = await _context.CitalackeGrupe
                .FirstOrDefaultAsync(g => g.GrupaId == sadrzajGrupe.GrupaId);

            var nazivGrupe = grupa?.Naziv ?? "čitalačkoj grupi";

            var korisnici = await _context.ClanstvaGrupe
                .Include(c => c.Korisnik)
                .Where(c => c.GrupaId == sadrzajGrupe.GrupaId)
                .Select(c => c.Korisnik)
                .ToListAsync();

            var poruka = sadrzajGrupe.TipSadrzaja == TipSadrzaja.story
                ? $"Novi story u grupi \"{nazivGrupe}\"."
                : $"Nova objava u grupi \"{nazivGrupe}\".";

            foreach (var korisnik in korisnici)
            {
                if (korisnik.Id != korisnikId)
                {
                    _context.Notifikacije.Add(new Notifikacija
                    {
                        KorisnikId = korisnik.Id,
                        Poruka = poruka,
                        Link = Url.Action("Details", "CitalackaGrupa", new { id = sadrzajGrupe.GrupaId }),
                        Datum = DateTime.Now,
                        Procitana = false
                    });
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "CitalackaGrupa", new { id = sadrzajGrupe.GrupaId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SadrzajGrupe sadrzajGrupe, IFormFile? pdfFile)
        {
            ModelState.Remove("Autor");
            ModelState.Remove("CitalackaGrupa");

            if (id != sadrzajGrupe.SadrzajId) return NotFound();

            if (sadrzajGrupe.TipSadrzaja == TipSadrzaja.story &&
                string.IsNullOrWhiteSpace(sadrzajGrupe.Link))
            {
                ModelState.AddModelError("Link", "Morate odabrati sliku ili uslikati story.");
            }

            if (sadrzajGrupe.TipSadrzaja == TipSadrzaja.pdf && pdfFile != null)
            {
                var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(pdfFile.FileName);
                var filePath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await pdfFile.CopyToAsync(stream);
                }

                sadrzajGrupe.Link = "/uploads/" + fileName;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sadrzajGrupe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SadrzajGrupeExists(sadrzajGrupe.SadrzajId))
                        return NotFound();

                    throw;
                }

                return RedirectToAction("Details", "CitalackaGrupa", new { id = sadrzajGrupe.GrupaId });
            }

            return View(sadrzajGrupe);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            var korisnikId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var korisnik = await _context.Korisnici
                .FirstOrDefaultAsync(k => k.Id == korisnikId);

            if (korisnik == null || korisnik.Uloga != Uloga.moderator)
            {
                return RedirectToAction("AccessDenied", "Home");
            }
            if (id == null) return NotFound();

            var sadrzajGrupe = await _context.SadrzajiGrupe
                .Include(s => s.Autor)
                .Include(s => s.CitalackaGrupa)
                .FirstOrDefaultAsync(m => m.SadrzajId == id);

            if (sadrzajGrupe == null) return NotFound();

            return View(sadrzajGrupe);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var korisnikId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var korisnik = await _context.Korisnici
                .FirstOrDefaultAsync(k => k.Id == korisnikId);

            if (korisnik == null || korisnik.Uloga != Uloga.moderator)
            {
                return RedirectToAction("AccessDenied", "Home");
            }
            var sadrzajGrupe = await _context.SadrzajiGrupe.FindAsync(id);

            if (sadrzajGrupe == null) return NotFound();

            int grupaId = sadrzajGrupe.GrupaId;

            _context.SadrzajiGrupe.Remove(sadrzajGrupe);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "CitalackaGrupa", new { id = grupaId });
        }

        private bool SadrzajGrupeExists(int id)
        {
            return _context.SadrzajiGrupe.Any(e => e.SadrzajId == id);
        }
    }
}