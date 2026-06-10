using DigitalniKlubCitalaca.Data;
using DigitalniKlubCitalaca.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DigitalniKlubCitalaca.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string pretraga)
        {
            var trenutniKorisnikId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var trenutniKorisnik = await _context.Korisnici
                .FirstOrDefaultAsync(k => k.Id == trenutniKorisnikId);

            if (trenutniKorisnik == null || trenutniKorisnik.Uloga != Uloga.administrator)
            {
               return Forbid();
            }

            var korisniciQuery = _context.Korisnici.AsQueryable();

            if (!string.IsNullOrWhiteSpace(pretraga))
            {
                korisniciQuery = korisniciQuery.Where(k =>
                    k.Ime.Contains(pretraga) ||
                    k.Prezime.Contains(pretraga) ||
                    k.Email.Contains(pretraga) ||
                    k.Grad.Contains(pretraga) ||
                    k.Regija.Contains(pretraga));
            }

            var korisnici = await korisniciQuery.ToListAsync();

            ViewBag.Pretraga = pretraga;

            return View(korisnici);
        }

        [HttpPost]
        public async Task<IActionResult> PromijeniStatus(string id)
        {
            var trenutniKorisnikId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var trenutniKorisnik = await _context.Korisnici
                .FirstOrDefaultAsync(k => k.Id == trenutniKorisnikId);

            if (trenutniKorisnik == null || trenutniKorisnik.Uloga != Uloga.administrator)
            {
                return Forbid();
            }

            var korisnik = await _context.Korisnici.FindAsync(id);

            if (korisnik == null)
                return NotFound();

            if (korisnik.StatusNaloga == StatusNaloga.aktivan)
                korisnik.StatusNaloga = StatusNaloga.blokiran;
            else
                korisnik.StatusNaloga = StatusNaloga.aktivan;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> PromijeniUlogu(string id)
        {
            var trenutniKorisnikId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var trenutniKorisnik = await _context.Korisnici
                .FirstOrDefaultAsync(k => k.Id == trenutniKorisnikId);

            if (trenutniKorisnik == null || trenutniKorisnik.Uloga != Uloga.administrator)
            {
                return Forbid();
            }

            var korisnik = await _context.Korisnici.FindAsync(id);

            if (korisnik == null)
                return NotFound();

            if (korisnik.Uloga == Uloga.korisnik)
                korisnik.Uloga = Uloga.moderator;
            else if (korisnik.Uloga == Uloga.moderator)
                korisnik.Uloga = Uloga.administrator;
            else
                korisnik.Uloga = Uloga.moderator;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Postavke()
        {
            return View();
        }
        public IActionResult DnevnikAktivnosti()
        {
            return View();
        }
    }
}