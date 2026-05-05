using System.ComponentModel.DataAnnotations;

namespace MVC.Models
{
    public class Korisnik
    {
        [Key]
        public int KorisnikId { get; set; }

        public string Ime { get; set; } = string.Empty;
        public string Prezime { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Lozinka { get; set; } = string.Empty;
        public string ProfilnaSlika { get; set; } = string.Empty;
        public string Biografija { get; set; } = string.Empty;
        public double LokacijaX { get; set; }
        public double LokacijaY { get; set; }

        public Uloga Uloga { get; set; }
        public StatusNaloga StatusNaloga { get; set; }

        public Korisnik() { }
    }
}