using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalniKlubCitalaca.Models
{
    public class Profil
    {
        [Key]
        public int ProfilId { get; set; }

        [ForeignKey("Korisnik")]
        public int KorisnikId { get; set; }

        public Korisnik Korisnik { get; set; } = null!;

        public string OpisProfila { get; set; } = string.Empty;
        public string Lokacija { get; set; } = string.Empty;

        public Profil() { }
    }
}
