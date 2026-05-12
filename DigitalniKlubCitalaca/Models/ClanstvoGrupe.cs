using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalniKlubCitalaca.Models
{
    public class ClanstvoGrupe
    {
        [Key]
        public int ClanstvoId { get; set; }

        [ForeignKey("Korisnik")]
        public int KorisnikId { get; set; }

        public Korisnik Korisnik { get; set; } = null!;

        [ForeignKey("CitalackaGrupa")]
        public int GrupaId { get; set; }

        public CitalackaGrupa CitalackaGrupa { get; set; } = null!;

        public DateTime DatumPridruzivanja { get; set; }

        public StatusClanstva StatusClanstva { get; set; }

        public ClanstvoGrupe() { }
    }
}
