using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalniKlubCitalaca.Models
{
    public class Komentar
    {
        [Key]
        public int KomentarId { get; set; }

        [ForeignKey("SadrzajGrupe")]
        public int SadrzajId { get; set; }

        public SadrzajGrupe SadrzajGrupe { get; set; } = null!;

        [ForeignKey("Autor")]
        public int AutorId { get; set; }

        public Korisnik Autor { get; set; } = null!;

        public string Tekst { get; set; } = string.Empty;

        public DateTime DatumKomentara { get; set; }

        public StatusKomentara StatusKomentara { get; set; }

        public Komentar() { }
    }
}
