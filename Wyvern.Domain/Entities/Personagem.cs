using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wyvern.Domain.Entities
{
    public class Personagem
    {
        [Key]
        public int PersonagemId { get; set; }
        [Required]
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public int CampanhaId { get; set; }
        public Campanha Campanha { get; set; }
        public int TipoId { get; set; } // enum se vai ser jogador,npc ou monstro
        public TipoPersonagem Tipo { get; set; }
        public int CriadoPorId { get; set; }
        public Usuario CriadoPor { get; set; }
        public DateTime CriadoEm { get; set; }
        public Atributo Atributo { get; set; }
        public bool Ativo { get; set; } = true;
        public List<PersonagemItem>? PersonagemItens { get; set; }
        public List<PersonagemMagia>? PersonagemMagias { get; set; }
        public PersonagemPlayer? PersonagemPlayer { get; set; }
        public PersonagemCombate? PersonagemCombate { get; set; } 
        public List<PersonagemPericia>? PersonagemPericias { get; set; }


    }
}
