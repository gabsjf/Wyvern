using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Wyvern.Application.DTOs.Pericia;
using Wyvern.Domain.Entities;
using Wyvern.Infrastructure.Repositories;

[ApiController]
[Route("[controller]")]
public class PericiaController : ControllerBase
{
    private readonly IUnitOfWork _uof;
    private readonly IMapper _mapper;

    public PericiaController(IUnitOfWork uof, IMapper mapper)
    {
        _uof = uof;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<PericiaResponseDto>> GetPericias()
    {
        var pericias = _uof.PericiaRepository.GetPericias();
        if (!pericias.Any()) {
            return NotFound("Pericia não encontrada");
        }
        var periciasDto = _mapper.Map<List<PericiaResponseDto>>(pericias);
        return Ok(periciasDto);
    }

    [HttpGet("{id:int}")]
    public ActionResult<PericiaResponseDto> GetPericiaById(int id)
    {
        var pericia = _uof.PericiaRepository.GetPericia(id);
        if (pericia == null) return NotFound("Perícia não encontrada.");
        var periciaDto = _mapper.Map<PericiaResponseDto>(pericia);
        return Ok(periciaDto);
    }

    [HttpPost]
    public ActionResult<PericiaResponseDto> CreatePericia(CreatePericiaDto periciaDto)
    {
        if(periciaDto == null)
        {
            return BadRequest("pericia inválida");
        }
        var pericia = _mapper.Map<Pericia>(periciaDto);
        _uof.PericiaRepository.CreatePericia(pericia);
        var periciaCriadaDto = _mapper.Map<PericiaResponseDto>(pericia);
        return CreatedAtAction(nameof(GetPericiaById), new { id = pericia.PericiaId }, periciaCriadaDto);
    }

    [HttpPut("{id:int}")]
    public ActionResult UpdatePericia(int id, PericiaUpdateDto periciaDto)
    {
        var periciaBanco = _uof.PericiaRepository.GetPericia(id);
        if (periciaBanco == null) return NotFound("Perícia não encontrada.");
        _mapper.Map(periciaDto, periciaBanco);
        _uof.PericiaRepository.UpdatePericia(periciaBanco);
        return Ok(_mapper.Map<PericiaResponseDto>(periciaBanco));
    }

    [HttpDelete("{id:int}")]
    public ActionResult DeletePericia(int id)
    {
        var pericia = _uof.PericiaRepository.DeletePericia(id);
        if (pericia == null) return NotFound("Perícia não encontrada.");
        return Ok(new { mensagem = "Perícia desativada", id });
    }
}