using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Wyvern.Application.DTOs.Campanha;
using Wyvern.Application.DTOs.Sessao;
using Wyvern.Domain.Entities;
using Wyvern.Infrastructure.Repositories;


namespace Wyvern.Api.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class CampanhaController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;

        public CampanhaController(IUnitOfWork uof, IMapper mapper)
        {
            _uof = uof;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CampanhaResponseDto>> GetCampanha()
        {
            var campanhas = _uof.CampanhaRepository.GetCampanhas();
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
            var campanha = _uof.CampanhaRepository.GetCampanha(id);
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
                return BadRequest("Dados inválidos");

            var campanha = _mapper.Map<Campanha>(campanhaDto);

            _uof.CampanhaRepository.CreateCampanha(campanha);

            var campanhaCompleta = _uof.CampanhaRepository.GetCampanha(campanha.CampanhaId);

            if (campanhaCompleta == null)
                return CreatedAtAction(nameof(GetCampanhaById), new { id = campanha.CampanhaId }, null);

            var campanhaCriadaDto = _mapper.Map<CampanhaResponseDetailDto>(campanhaCompleta);

            return CreatedAtAction(nameof(GetCampanhaById), new { id = campanha.CampanhaId }, campanhaCriadaDto);
        }
        [HttpPut("{id:int}")]
        public ActionResult<CampanhaResponseDetailDto> UpdateCampanha(int id, CampanhaUpdatetDto campanhaDto)
        {
            if (campanhaDto == null)
                return BadRequest("Dados inválidos");

            var campanhaNoBanco = _uof.CampanhaRepository.GetCampanha(id);

            if (campanhaNoBanco == null)
                return NotFound("Campanha não encontrada");

            _mapper.Map(campanhaDto, campanhaNoBanco);

            _uof.CampanhaRepository.UpdateCampanha(campanhaNoBanco);

            var campanhaAtualizada = _uof.CampanhaRepository.GetCampanha(id);

            var campanhaDtoAtualizada = _mapper.Map<CampanhaResponseDetailDto>(campanhaAtualizada);

            return Ok(campanhaDtoAtualizada);
        }

        [HttpDelete("{id:int}")]
        public ActionResult DeleteCampanha(int id)
        {

            var campanha = _uof.CampanhaRepository.DeleteCampanha(id);
            if (campanha == null)
            {
                return BadRequest("Dados inválidos");
            }
            return Ok("Campanha deletada com sucesso");
        }
    }
}
