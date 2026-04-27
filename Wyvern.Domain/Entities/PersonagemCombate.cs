using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Wyvern.Domain.Entities
{
    public class PersonagemCombate
    {
        [Key]
        public int PersonagemId { get; set; }
        public Personagem Personagem { get; set; }
        public int VidaAtual { get; set; }
        public int VidaMaxima { get; set; }
        public int ClasseArmadura { get; set; }
        public int Iniciativa { get; set; }
        public int Deslocamento { get; set; }
    }
}
