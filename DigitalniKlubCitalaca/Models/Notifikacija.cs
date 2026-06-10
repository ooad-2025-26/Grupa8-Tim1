using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalniKlubCitalaca.Models
{
    public class Notifikacija
    {
        [Key]
        public int NotifikacijaId { get; set; }

        [Required]
        public required string KorisnikId { get; set; }

        [ForeignKey("KorisnikId")]
        public Korisnik? Korisnik { get; set; }

        [Required]
        public required string Poruka { get; set; }

        public string? Link { get; set; }

        public DateTime Datum { get; set; } = DateTime.Now;

        public bool Procitana { get; set; } = false;
    }
}