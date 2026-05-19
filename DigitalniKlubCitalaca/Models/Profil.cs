using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalniKlubCitalaca.Models
{
    public class Profil
    {
        [Key]
        public int ProfilId { get; set; }

        [ForeignKey("Korisnik")]
        public string KorisnikId { get; set; } = string.Empty;

        public Korisnik Korisnik { get; set; } = null!;

        public string OpisProfila { get; set; } = string.Empty;
        public string Lokacija { get; set; } = string.Empty;

        public Profil() { }
    }
}
