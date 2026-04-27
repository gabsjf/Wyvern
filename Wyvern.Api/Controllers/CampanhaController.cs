using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wyvern.Application.DTOs.Campanha;
using Wyvern.Application.DTOs.Sessao;
using Wyvern.Domain.Entities;
using Wyvern.Infrastructure.Data;

namespace Wyvern.Api.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class CampanhaController : ControllerBase
    {
        private readonly WyvernDbContext _context;
        private readonly IMapper _mapper;

        public CampanhaController(WyvernDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CampanhaResponseDto>> GetCampanha()
        {
            var campanhas = _context.Campanhas
                .Include(c => c.Mestre)
                .Include(c => c.Sessoes)
                .Where(c => c.Ativo)
                .ToList();
            if (!campanhas.Any())
            {
                return NotFound("Nenhuma sessão encontrada");
            }
            var campanhasDto = _mapper.Map<List<CampanhaResponseDto>>(campanhas);
            return Ok(campanhasDto);
        }
        [HttpGet("{id:int}")]
        public ActionResult<CampanhaResponseDetailDto> GetCampanhaById (int id)
        {
            var campanha = _context.Campanhas
                .Include(c => c.Mestre)
                .Include (c => c.Sessoes)
                .FirstOrDefault(c => c.CampanhaId == id && c.Ativo);
            if ( campanha == null)
            {
                return NotFound("Campanha nao encontrada");
            }
            var campanhaDto = _mapper.Map<CampanhaResponseDetailDto>(campanha);
            campanhaDto.MestreNome = campanha.Mestre?.Nome ?? string.Empty;
            campanhaDto.Sessoes = _mapper.Map<List<SessaoResponseDto>>(campanha.Sessoes ?? new List<Sessao>());
            return Ok(campanhaDto);
        }
        [HttpPost]
        public ActionResult<CampanhaResponseDetailDto> CreateCampanha(CreateCampanhaDto campanhaDto)
        {
            if (campanhaDto == null)
            {
                return BadRequest("Dados inválidos");
            }
            var campanha = _mapper.Map<Campanha>(campanhaDto);
            _context.Campanhas.Add(campanha);
            _context.SaveChanges();

            var campanhaCompleta = _context.Campanhas
                .Include(c => c.Mestre)
                .Include(c => c.Sessoes)
                .FirstOrDefault(c => c.CampanhaId == campanha.CampanhaId);
            if (campanhaCompleta == null)
            {
                return CreatedAtAction(nameof(GetCampanhaById), new { id = campanha.CampanhaId }, null);
            }
            var campanhaCriadaDto = _mapper.Map<CampanhaResponseDetailDto>(campanhaCompleta);
            campanhaCriadaDto.MestreNome = campanhaCompleta.Mestre?.Nome ?? string.Empty;
            campanhaCriadaDto.Sessoes = _mapper.Map<List<SessaoResponseDto>>(campanhaCompleta.Sessoes ?? new List<Sessao>());
            return CreatedAtAction(nameof(GetCampanhaById), new { id = campanha.CampanhaId }, campanhaCriadaDto);
        }
        [HttpDelete("{id:int}")]
        public ActionResult DeleteCampanha(int id)
        {

            var campanha = _context.Campanhas.FirstOrDefault(c => c.CampanhaId == id);
            if (campanha == null)
            {
                return BadRequest("Dados inválidos");
            }
            campanha.Ativo = false;
            _context.SaveChanges();
            return Ok("Campanha deletada com sucesso");
        }
    }
}
