using Wyvern.Application.DTOs.Atributo;

namespace Wyvern.Application.DTOs.Personagem
{
    public class PersonagemResponseDto
    {
        public int PersonagemId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public int CampanhaId { get; set; }
        public int TipoId { get; set; }
        public int CriadoPorId { get; set; }
        public DateTime CriadoEm { get; set; }
        public bool Ativo { get; set; }
        public AtributoResponseDto? Atributo { get; set; }
        public PersonagemPlayerResponseDto? PersonagemPlayer { get; set; }
        public PersonagemCombateResponseDto? PersonagemCombate { get; set; }
    }
}
