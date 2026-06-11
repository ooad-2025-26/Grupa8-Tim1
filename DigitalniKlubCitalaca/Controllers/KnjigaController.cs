using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DigitalniKlubCitalaca.Data;
using DigitalniKlubCitalaca.Models;
using System.Security.Claims;

namespace DigitalniKlubCitalaca.Controllers
{
    public class KnjigaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KnjigaController(ApplicationDbContext context)
        {
            _context = context;
        }

        private async Task<bool> JeAdministrator()
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated)
                return false;

            var korisnikId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var korisnik = await _context.Korisnici
                .FirstOrDefaultAsync(k => k.Id == korisnikId);

            return korisnik != null && korisnik.Uloga == Uloga.administrator;
        }

        public async Task<IActionResult> Index(string? zanr)
        {
            var knjige = _context.Knjige.AsQueryable();

            if (!string.IsNullOrWhiteSpace(zanr))
            {
                knjige = knjige.Where(k => k.Zanr == zanr);
            }

            ViewBag.Zanrovi = await _context.Knjige
                .Where(k => !string.IsNullOrWhiteSpace(k.Zanr))
                .Select(k => k.Zanr)
                .Distinct()
                .OrderBy(z => z)
                .ToListAsync();

            ViewBag.OdabraniZanr = zanr;
            ViewBag.JeAdministrator = await JeAdministrator();

            return View(await knjige.OrderBy(k => k.Naziv).ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var knjiga = await _context.Knjige
                .FirstOrDefaultAsync(m => m.KnjigaId == id);

            if (knjiga == null) return NotFound();

            ViewBag.JeAdministrator = await JeAdministrator();

            return View(knjiga);
        }

        [Authorize]
        public async Task<IActionResult> Citaj(int id)
        {
            var knjiga = await _context.Knjige.FindAsync(id);

            if (knjiga == null) return NotFound();

            if (string.IsNullOrWhiteSpace(knjiga.PdfPutanja))
                return NotFound();

            return Redirect(knjiga.PdfPutanja);
        }

        [Authorize]
        public async Task<IActionResult> Create()
        {
            if (!await JeAdministrator())
                return Forbid();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(Knjiga knjiga, IFormFile? pdfFile)
        {
            if (!await JeAdministrator())
                return Forbid();

            ModelState.Remove("PdfPutanja");

            if (pdfFile == null || pdfFile.Length == 0)
            {
                ModelState.AddModelError("pdfFile", "Morate odabrati PDF knjige.");
            }

            if (!ModelState.IsValid)
                return View(knjiga);

            var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "biblioteka");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var fileName = Guid.NewGuid() + Path.GetExtension(pdfFile!.FileName);
            var filePath = Path.Combine(folder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await pdfFile.CopyToAsync(stream);
            }

            knjiga.PdfPutanja = "/biblioteka/" + fileName;

            _context.Knjige.Add(knjiga);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (!await JeAdministrator())
                return Forbid();

            if (id == null) return NotFound();

            var knjiga = await _context.Knjige.FindAsync(id);

            if (knjiga == null) return NotFound();

            return View(knjiga);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, Knjiga knjiga, IFormFile? pdfFile)
        {
            if (!await JeAdministrator())
                return Forbid();

            if (id != knjiga.KnjigaId) return NotFound();

            ModelState.Remove("PdfPutanja");

            var staraKnjiga = await _context.Knjige.AsNoTracking()
                .FirstOrDefaultAsync(k => k.KnjigaId == id);

            if (staraKnjiga == null) return NotFound();

            knjiga.PdfPutanja = staraKnjiga.PdfPutanja;

            if (pdfFile != null && pdfFile.Length > 0)
            {
                var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "biblioteka");

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                var fileName = Guid.NewGuid() + Path.GetExtension(pdfFile.FileName);
                var filePath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await pdfFile.CopyToAsync(stream);
                }

                knjiga.PdfPutanja = "/biblioteka/" + fileName;
            }

            if (!ModelState.IsValid)
                return View(knjiga);

            _context.Update(knjiga);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (!await JeAdministrator())
                return Forbid();

            if (id == null) return NotFound();

            var knjiga = await _context.Knjige
                .FirstOrDefaultAsync(m => m.KnjigaId == id);

            if (knjiga == null) return NotFound();

            return View(knjiga);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!await JeAdministrator())
                return Forbid();

            var knjiga = await _context.Knjige.FindAsync(id);

            if (knjiga != null)
            {
                _context.Knjige.Remove(knjiga);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}