using System.ComponentModel.DataAnnotations;

namespace Wyvern.Application.DTOs.Personagem
{
    public class PersonagemItemAddDto
    {
        [Required]
        public int PersonagemId { get; set; }
        [Required]
        public int ItemId { get; set; }
    }
}
