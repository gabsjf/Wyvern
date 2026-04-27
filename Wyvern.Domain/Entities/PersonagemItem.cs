using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Wyvern.Domain.Entities
{
    public class PersonagemItem
    {
        [Key]
        public int PersonagemId { get; set; }
        public int ItemId { get; set; }
        public Personagem Personagem { get; set; }
        public Item Item { get; set; }
    }
}
