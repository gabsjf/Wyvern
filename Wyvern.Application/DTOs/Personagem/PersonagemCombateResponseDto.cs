using System;
using System.Collections.Generic;
using System.Text;

namespace Wyvern.Application.DTOs.Personagem
{
    public class PersonagemCombateResponseDto
    {
        public int PersonagemId { get; set; }
        public int VidaAtual { get; set; }
        public int VidaMaxima { get; set; }
        public int ClasseArmadura { get; set; }
        public int Iniciativa { get; set; }
        public int Deslocamento { get; set; }
    }
}
