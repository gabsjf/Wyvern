using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wyvern.Application.DTOs.Campanha;
using Wyvern.Application.DTOs.Sessao;
using Wyvern.Domain.Entities;
using Wyvern.Infrastructure.Data;
using Wyvern.Infrastructure.Repositories.Campanha;


namespace Wyvern.Api.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class CampanhaController : ControllerBase
    {
        private readonly CampanhaRepository _repository;
        private readonly IMapper _mapper;

        public CampanhaController(CampanhaRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CampanhaResponseDto>> GetCampanha()
        {
            var campanhas = _repository.GetCampanhas();
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
            var campanha = _repository.GetCampanha(id);
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

            _repository.CreateCampanha(campanha);

            var campanhaCompleta = _repository.GetCampanha(campanha.CampanhaId);

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

            var campanhaNoBanco = _repository.GetCampanha(id);

            if (campanhaNoBanco == null)
                return NotFound("Campanha não encontrada");

            _mapper.Map(campanhaDto, campanhaNoBanco);

            _repository.UpdateCampanha(campanhaNoBanco);

            var campanhaAtualizada = _repository.GetCampanha(id);

            var campanhaDtoAtualizada = _mapper.Map<CampanhaResponseDetailDto>(campanhaAtualizada);

            return Ok(campanhaDtoAtualizada);
        }

        [HttpDelete("{id:int}")]
        public ActionResult DeleteCampanha(int id)
        {

            var campanha = _repository.DeleteCampanha(id);
            if (campanha == null)
            {
                return BadRequest("Dados inválidos");
            }
            return Ok("Campanha deletada com sucesso");
        }
    }
}
