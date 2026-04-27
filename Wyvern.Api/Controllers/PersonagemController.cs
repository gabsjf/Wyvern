using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wyvern.Application.DTOs.Personagem;
using Wyvern.Domain.Entities;
using Wyvern.Infrastructure.Data;

namespace Wyvern.Api.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class PersonagemController : ControllerBase
    {
        private readonly WyvernDbContext _context;
        private readonly IMapper _mapper;
        public PersonagemController (WyvernDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PersonagemResponseDto>> GetPersonagens()
        {
            
            var personagens = _context.Personagens
                .AsNoTracking() 
                .Include(p => p.Atributo)
                .Include(p => p.PersonagemPlayer)
                .Include(p => p.PersonagemCombate)
                .Where(p => p.Ativo)
                .ToList();

            if (personagens == null || !personagens.Any())
            {
               
                return Ok(new List<PersonagemResponseDto>());
            }

            var personagensDto = _mapper.Map<List<PersonagemResponseDto>>(personagens);
            return Ok(personagensDto);
        }
        [HttpGet("{id:int}")]
        public ActionResult<PersonagemResponseDto> GetPersonagemById( int id)
        {
            var personagens = _context.Personagens
                .Include(p => p.Atributo)
                .Include(p => p.PersonagemPlayer)
                .Include(p => p.PersonagemCombate)
                .FirstOrDefault(p => p.PersonagemId == id && p.Ativo);

            if (personagens == null )
            {

                return BadRequest("Personagem Não encontrado");
            }
            var personagemDto = _mapper.Map<PersonagemResponseDto>(personagens);
            return Ok(personagemDto);

        }

        [HttpPost]
        public ActionResult<PersonagemResponseDto> CreatePersonagem(PersonagemCreateDto personagemDto)
        {
            if (personagemDto == null) return BadRequest("Dados inválidos");

            var personagem = _mapper.Map<Personagem>(personagemDto);
            personagem.CriadoEm = DateTime.Now;
            personagem.Ativo = true;

            _context.Personagens.Add(personagem);
            _context.SaveChanges();

            
            var retorno = _context.Personagens
                .Include(p => p.Atributo)
                .Include(p => p.PersonagemPlayer)
                .Include(p => p.PersonagemCombate)
                .FirstOrDefault(p => p.PersonagemId == personagem.PersonagemId);

            if (retorno == null)
            {
                return CreatedAtAction(nameof(GetPersonagemById), new { id = personagem.PersonagemId }, null);
            }
            var retornoDto = _mapper.Map<PersonagemResponseDto>(retorno);
            return CreatedAtAction(nameof(GetPersonagemById), new { id = personagem.PersonagemId }, retornoDto);
        }

        [HttpPut("{id:int}")]
        public ActionResult UpdatePersonagem(int id, PersonagemUpdateDto personagemDto)
        {
            var pBanco = _context.Personagens
                .Include(p => p.Atributo)
                .Include(p => p.PersonagemPlayer)
                .Include(p => p.PersonagemCombate)
                .FirstOrDefault(p => p.PersonagemId == id && p.Ativo);

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

            _context.SaveChanges();
            return Ok(_mapper.Map<PersonagemResponseDto>(pBanco));
        }

        [HttpDelete("{id:int}")]
        public ActionResult DeletePersonagem(int id)
        {
            var personagem = _context.Personagens.FirstOrDefault(p => p.PersonagemId == id);
            if (personagem == null) return NotFound("Personagem não encontrado");

            personagem.Ativo = false; 
            _context.SaveChanges();

            return Ok(new { mensagem = "Personagem desativado com sucesso", id });
        }

    }
}
