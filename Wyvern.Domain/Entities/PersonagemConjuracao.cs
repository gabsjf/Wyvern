using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wyvern.Domain.Entities
{
    public class PersonagemConjuracao
    {
        [Key]
        [ForeignKey("Personagem")]
        public int PersonagemId { get; set; }
        public Personagem Personagem { get; set; }

        public string? AtributoConjuracao { get; set; }
        public int ModificadorConjuracao { get; set; }
        public int CdMagia { get; set; }
        public int ModificadorAtaqueMagico { get; set; }

        public int SlotsTotalNivel1 { get; set; }
        public int SlotsGastosNivel1 { get; set; }
        public int SlotsTotalNivel2 { get; set; }
        public int SlotsGastosNivel2 { get; set; }
        public int SlotsTotalNivel3 { get; set; }
        public int SlotsGastosNivel3 { get; set; }
        public int SlotsTotalNivel4 { get; set; }
        public int SlotsGastosNivel4 { get; set; }
        public int SlotsTotalNivel5 { get; set; }
        public int SlotsGastosNivel5 { get; set; }
        public int SlotsTotalNivel6 { get; set; }
        public int SlotsGastosNivel6 { get; set; }
        public int SlotsTotalNivel7 { get; set; }
        public int SlotsGastosNivel7 { get; set; }
        public int SlotsTotalNivel8 { get; set; }
        public int SlotsGastosNivel8 { get; set; }
        public int SlotsTotalNivel9 { get; set; }
        public int SlotsGastosNivel9 { get; set; }
    }
}
