using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Wyvern.Domain.Entities
{
    public class PersonagemMagia
    {
        [Key]
        public int PersonagemId { get; set; }
        public Personagem Personagem { get; set; }
        public int MagiaId { get; set; }
        public Magia Magia { get; set; }
    }
}
