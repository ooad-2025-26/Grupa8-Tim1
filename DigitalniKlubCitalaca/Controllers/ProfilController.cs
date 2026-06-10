using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DigitalniKlubCitalaca.Data;
using DigitalniKlubCitalaca.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace DigitalniKlubCitalaca.Controllers
{
    [Authorize]
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
            var korisnikId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var profil = await _context.Profili
                .Include(p => p.Korisnik)
                .FirstOrDefaultAsync(p => p.KorisnikId == korisnikId);

            if (profil == null)
            {
                return RedirectToAction(nameof(Create));
            }

            return View(new List<Profil> { profil });
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
            return View();
        }

        // POST: Profil/Create
        [HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create([Bind("OpisProfila,Lokacija")] Profil profil)
{
    var korisnikId = User.FindFirstValue(ClaimTypes.NameIdentifier);

    if (korisnikId == null)
    {
        return RedirectToPage("/Account/Login", new { area = "Identity" });
    }

    profil.KorisnikId = korisnikId;
    ModelState.Remove("KorisnikId");
    ModelState.Remove("Korisnik");

    if (ModelState.IsValid)
    {
        _context.Add(profil);
        await _context.SaveChangesAsync();

        _context.Notifikacije.Add(new Notifikacija
        {
            KorisnikId = korisnikId,
            Poruka = "Vaš profil je uspješno kreiran.",
            Link = Url.Action("Index", "Profil"),
            Datum = DateTime.Now,
            Procitana = false
        });

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    return View(profil);
}

        // GET: Profil/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var korisnikId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var profil = await _context.Profili.Include(p => p.Korisnik)
                    .FirstOrDefaultAsync(p => p.ProfilId == id && p.KorisnikId == korisnikId);

            if (profil == null)
            {
                return NotFound();
            }

            return View(profil);
        }

        // POST: Profil/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Profil profil)
        {
            if (id != profil.ProfilId)
            {
                return NotFound();
            }

            var korisnikId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var profilIzBaze = await _context.Profili
                .Include(p => p.Korisnik)
                .FirstOrDefaultAsync(p => p.ProfilId == id && p.KorisnikId == korisnikId);

            if (profilIzBaze == null)
            {
                return NotFound();
            }

            ModelState.Remove("KorisnikId");
            ModelState.Remove("Korisnik");
            ModelState.Remove("Korisnik.Id");
            ModelState.Remove("Korisnik.UserName");
            ModelState.Remove("Korisnik.NormalizedUserName");
            ModelState.Remove("Korisnik.NormalizedEmail");
            ModelState.Remove("Korisnik.PasswordHash");
            ModelState.Remove("Korisnik.SecurityStamp");
            ModelState.Remove("Korisnik.ConcurrencyStamp");

            if (ModelState.IsValid)
            {
                profilIzBaze.OpisProfila = profil.OpisProfila;
                profilIzBaze.Lokacija = profil.Lokacija;

                profilIzBaze.Korisnik.Ime = profil.Korisnik.Ime;
                profilIzBaze.Korisnik.Prezime = profil.Korisnik.Prezime;
                profilIzBaze.Korisnik.Regija = profil.Korisnik.Regija;
                profilIzBaze.Korisnik.Grad = profil.Lokacija;
                profilIzBaze.Korisnik.Biografija = profil.OpisProfila;
                
                await _context.SaveChangesAsync();

_context.Notifikacije.Add(new Notifikacija
{
    KorisnikId = korisnikId,
    Poruka = "Vaš profil je uspješno ažuriran.",
    Link = Url.Action("Index", "Profil"),
    Datum = DateTime.Now,
    Procitana = false
});

await _context.SaveChangesAsync();

TempData["SuccessMessage"] = "Profil je uspješno ažuriran!";

return RedirectToAction(nameof(Index));
            }

            return View(profilIzBaze);
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
