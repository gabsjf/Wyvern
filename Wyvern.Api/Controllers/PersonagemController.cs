using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Wyvern.Application.DTOs.Personagem;
using Wyvern.Domain.Entities;
using Wyvern.Infrastructure.Repositories;

namespace Wyvern.Api.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class PersonagemController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        public PersonagemController (IUnitOfWork uof, IMapper mapper)
        {
            _uof = uof;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonagemResponseDto>>> GetPersonagens()
        {
            var personagens = await _uof.PersonagemRepository.GetPersonagensAsync();

            if (personagens == null || !personagens.Any())
            {
               
                return Ok(new List<PersonagemResponseDto>());
            }

            var personagensDto = _mapper.Map<List<PersonagemResponseDto>>(personagens);
            return Ok(personagensDto);
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<PersonagemResponseDto>> GetPersonagemById( int id)
        {
            var personagens = await _uof.PersonagemRepository.GetPersonagemAsync(id);

            if (personagens == null )
            {

                return BadRequest("Personagem Não encontrado");
            }
            var personagemDto = _mapper.Map<PersonagemResponseDto>(personagens);
            return Ok(personagemDto);

        }

        [HttpPost]
        public async Task<ActionResult<PersonagemResponseDto>> CreatePersonagem(PersonagemCreateDto personagemDto)
        {
            if (personagemDto == null) return BadRequest("Dados inválidos");

            var personagem = _mapper.Map<Personagem>(personagemDto);
            personagem.CriadoEm = DateTime.Now;
            personagem.Ativo = true;

            await _uof.PersonagemRepository.CreatePersonagemAsync(personagem);

            
            var retorno = await _uof.PersonagemRepository.GetPersonagemAsync(personagem.PersonagemId);

            if (retorno == null)
            {
                return CreatedAtAction(nameof(GetPersonagemById), new { id = personagem.PersonagemId }, null);
            }
            var retornoDto = _mapper.Map<PersonagemResponseDto>(retorno);
            return CreatedAtAction(nameof(GetPersonagemById), new { id = personagem.PersonagemId }, retornoDto);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdatePersonagem(int id, PersonagemUpdateDto personagemDto)
        {
            var pBanco = await _uof.PersonagemRepository.GetPersonagemAsync(id);

            if (pBanco == null) return NotFound("Personagem não encontrado");

            pBanco.Nome = personagemDto.Nome;
            pBanco.Descricao = personagemDto.Descricao;
            pBanco.TipoId = personagemDto.TipoId;

            if (personagemDto.Atributo != null)
            {
                pBanco.Atributo ??= new Atributo { PersonagemId = pBanco.PersonagemId };
                _mapper.Map(personagemDto.Atributo, pBanco.Atributo);
            }

            if (personagemDto.PersonagemPlayer != null)
            {
                pBanco.PersonagemPlayer ??= new PersonagemPlayer { PersonagemId = pBanco.PersonagemId };
                _mapper.Map(personagemDto.PersonagemPlayer, pBanco.PersonagemPlayer);
            }

            if (personagemDto.PersonagemCombate != null)
            {
                pBanco.PersonagemCombate ??= new PersonagemCombate { PersonagemId = pBanco.PersonagemId };
                _mapper.Map(personagemDto.PersonagemCombate, pBanco.PersonagemCombate);
            }

            if (personagemDto.PersonagemDetalhes != null)
            {
                pBanco.PersonagemDetalhes ??= new PersonagemDetalhes { PersonagemId = pBanco.PersonagemId };
                _mapper.Map(personagemDto.PersonagemDetalhes, pBanco.PersonagemDetalhes);
            }

            if (personagemDto.PersonagemDinheiro != null)
            {
                pBanco.PersonagemDinheiro ??= new PersonagemDinheiro { PersonagemId = pBanco.PersonagemId };
                _mapper.Map(personagemDto.PersonagemDinheiro, pBanco.PersonagemDinheiro);
            }

            await _uof.PersonagemRepository.UpdatePersonagemAsync(pBanco);
            return Ok(_mapper.Map<PersonagemResponseDto>(pBanco));
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeletePersonagem(int id)
        {
            var personagem = await _uof.PersonagemRepository.DeletePersonagemAsync(id);
            if (personagem == null) return NotFound("Personagem não encontrado");
            return Ok(new { mensagem = "Personagem desativado com sucesso", id });
        }

        [HttpPost("import-pdf")]
        public async Task<ActionResult<PersonagemResponseDto>> ImportPdf(IFormFile file, [FromServices] Wyvern.Application.Services.IPdfParserService pdfParserService)
        {
            if (file == null || file.Length == 0) return BadRequest("Nenhum arquivo enviado.");

            try
            {
                using var stream = file.OpenReadStream();
                var personagem = pdfParserService.ParsePdf(stream);
                
                // Salvar no banco
                await _uof.PersonagemRepository.CreatePersonagemAsync(personagem);
                
                var retorno = await _uof.PersonagemRepository.GetPersonagemAsync(personagem.PersonagemId);
                var retornoDto = _mapper.Map<PersonagemResponseDto>(retorno);
                
                return Ok(retornoDto);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao importar PDF: {ex.Message}");
            }
        }

        [HttpGet("{id:int}/export-pdf")]
        public async Task<IActionResult> ExportPdf(int id, [FromServices] Wyvern.Application.Services.IPdfExportService pdfExportService)
        {
            var pBanco = await _uof.PersonagemRepository.GetPersonagemAsync(id);
            if (pBanco == null) return NotFound("Personagem não encontrado");

            try
            {
                var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "ficha55.pdf");
                var pdfBytes = pdfExportService.ExportPdf(pBanco, templatePath);
                return File(pdfBytes, "application/pdf", $"{pBanco.Nome ?? "Personagem"}.pdf");
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao exportar PDF: {ex.Message}");
            }
        }

    }
}
