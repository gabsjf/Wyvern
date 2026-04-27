using System.ComponentModel.DataAnnotations;
using Wyvern.Application.DTOs.Atributo;

namespace Wyvern.Application.DTOs.Personagem
{
    public class PersonagemUpdateDto
    {
        [Required]
        public int PersonagemId { get; set; }
        [Required]
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public int TipoId { get; set; }
        public AtributoUpdateDto? Atributo { get; set; }
        public PersonagemPlayerUpdateDto? PersonagemPlayer { get; set; }
        public PersonagemCombateUpdateDto? PersonagemCombate { get; set; }
    }
}
