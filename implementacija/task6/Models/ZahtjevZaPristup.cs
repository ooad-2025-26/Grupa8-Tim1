using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC.Models
{
    public class ZahtjevZaPristup
    {
        [Key]
        public int ZahtjevId { get; set; }

        [ForeignKey("Korisnik")]
        public int KorisnikId { get; set; }

        public Korisnik Korisnik { get; set; } = null!;

        [ForeignKey("CitalackaGrupa")]
        public int GrupaId { get; set; }

        public CitalackaGrupa CitalackaGrupa { get; set; } = null!;

        public DateTime DatumZahtjeva { get; set; }

        public StatusZahtjeva StatusZahtjeva { get; set; }

        public ZahtjevZaPristup() { }
    }
}