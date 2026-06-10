using DigitalniKlubCitalaca.Data;
using DigitalniKlubCitalaca.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DigitalniKlubCitalaca.Controllers
{
    public class NotifikacijaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Korisnik> _userManager;

        public NotifikacijaController(
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
                return RedirectToAction("Index", "Home");

            var notifikacije = await _context.Notifikacije
                .Where(n => n.KorisnikId == korisnik.Id)
                .OrderByDescending(n => n.Datum)
                .ToListAsync();

            return View(notifikacije);
        }
    }
}