using System.ComponentModel.DataAnnotations;

namespace MVC.Models
{
    public class Knjiga
    {
        [Key]
        public int KnjigaId { get; set; }

        public string Naziv { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public string Opis { get; set; } = string.Empty;

        public int BrojStranica { get; set; }

        public string Zanr { get; set; } = string.Empty;

        public Knjiga() { }
    }
}