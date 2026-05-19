using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalniKlubCitalaca.Models
{
    public class SadrzajGrupe
    {
        [Key]
        public int SadrzajId { get; set; }

        [ForeignKey("Autor")]
        public string AutorId { get; set; } = string.Empty;

        public Korisnik Autor { get; set; } = null!;

        [ForeignKey("CitalackaGrupa")]
        public int GrupaId { get; set; }

        public CitalackaGrupa CitalackaGrupa { get; set; } = null!;

        public DateTime DatumObjave { get; set; }

        public string Naslov { get; set; } = string.Empty;
        public string Opis { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;

        public TipSadrzaja TipSadrzaja { get; set; }
        public StatusSadrzaja StatusSadrzaja { get; set; }

        public SadrzajGrupe() { }
    }
}
