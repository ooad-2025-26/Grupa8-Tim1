using System;
using System.ComponentModel.DataAnnotations;

namespace MVC.Models
{
    public class CitalackaGrupa
    {
        [Key]
        public int GrupaId { get; set; }

        public string Naziv { get; set; } = string.Empty;
        public string Opis { get; set; } = string.Empty;
        public string Zanr { get; set; } = string.Empty;
        public string Regija { get; set; } = string.Empty;
        public string Grad { get; set; } = string.Empty;

        public int MaksimalanBrojClanova { get; set; }

        public TipGrupe TipGrupe { get; set; }

        public DateTime DatumKreiranja { get; set; }

        public CitalackaGrupa() { }
    }
}