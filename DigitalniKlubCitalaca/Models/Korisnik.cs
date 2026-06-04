using Microsoft.AspNetCore.Identity;

namespace DigitalniKlubCitalaca.Models
{
    public class Korisnik : IdentityUser
    {
        public string Ime { get; set; } = string.Empty;
        public string Prezime { get; set; } = string.Empty;
        public string ProfilnaSlika { get; set; } = string.Empty;
        public string Biografija { get; set; } = string.Empty;

        public double LokacijaX { get; set; }
        public double LokacijaY { get; set; }

        public string Regija { get; set; } = string.Empty;
        public string Grad { get; set; } = string.Empty;

        public Uloga Uloga { get; set; }
        public StatusNaloga StatusNaloga { get; set; }

        public Korisnik() { }
    }
}