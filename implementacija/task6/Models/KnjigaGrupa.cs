using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC.Models
{
    public class KnjigaGrupa
    {
        [Key]
        public int KnjigaGrupaId { get; set; }

        [ForeignKey("Knjiga")]
        public int KnjigaId { get; set; }

        public Knjiga Knjiga { get; set; } = null!;

        [ForeignKey("CitalackaGrupa")]
        public int GrupaId { get; set; }

        public CitalackaGrupa CitalackaGrupa { get; set; } = null!;

        public KnjigaGrupa() { }
    }
}