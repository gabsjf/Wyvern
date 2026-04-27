using System.ComponentModel.DataAnnotations;
using Wyvern.Application.DTOs.Atributo;

namespace Wyvern.Application.DTOs.Personagem
{
    public class PersonagemCreateDto
    {
        [Required]
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        [Required]
        public int CampanhaId { get; set; }
        [Required]
        public int TipoId { get; set; }
        [Required]
        public int CriadoPorId { get; set; }
        public CreateAtributoDto? Atributo { get; set; }
        public PersonagemPlayerUpdateDto? PersonagemPlayer { get; set; }
        public PersonagemCombateUpdateDto? PersonagemCombate { get; set; }
    }
}
