using DigitalniKlubCitalaca.Data;
using DigitalniKlubCitalaca.Models;

namespace DigitalniKlubCitalaca.Services
{
    public class NotifikacijaService
    {
        private readonly ApplicationDbContext _context;

        public NotifikacijaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task DodajNotifikacijuAsync(string korisnikId, string poruka, string? link = null)
        {
            var notifikacija = new Notifikacija
            {
                KorisnikId = korisnikId,
                Poruka = poruka,
                Link = link,
                Datum = DateTime.Now,
                Procitana = false
            };

            _context.Notifikacije.Add(notifikacija);
            await _context.SaveChangesAsync();
        }
    }
}