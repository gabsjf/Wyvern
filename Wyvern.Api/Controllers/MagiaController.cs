using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Wyvern.Application.DTOs.Magia;
using Wyvern.Domain.Entities;
using Wyvern.Infrastructure.Repositories;

[ApiController]
[Route("[controller]")]
public class MagiaController : ControllerBase
{
    private readonly IUnitOfWork _uof;
    private readonly IMapper _mapper;

    public MagiaController(IUnitOfWork uof, IMapper mapper )
    {
        _uof = uof;
        _mapper = mapper;

    }

    [HttpGet]
    public ActionResult<IEnumerable<MagiaResponseDto>> GetMagias()
    {
        var magias = _uof.MagiaRepository.GetMagias();
        if (!magias.Any())
        {
            return NotFound("Magia não encontrada");
        }
        var magiasDto = _mapper.Map<List<MagiaResponseDto>>(magias);
        return Ok(magiasDto);
    }

    [HttpGet("{id:int}")]
    public ActionResult<MagiaResponseDto> GetMagiaById(int id)
    {
        var magia = _uof.MagiaRepository.GetMagiaById(id);
        if (magia == null) return NotFound("Magia não encontrada ou inativa.");
        var magiaDto = _mapper.Map<MagiaResponseDto>(magia);
        return Ok(magiaDto);
    }

    [HttpPost]
    public ActionResult<MagiaResponseDto> CreateMagia(MagiaCreateDto magiaDto)
    {
        if (magiaDto == null)
        {
            return BadRequest("item inválido");
        }
        var magia = _mapper.Map<Magia>(magiaDto);
        _uof.MagiaRepository.CreateMagia(magia);
        var magiaCriadaDto = _mapper.Map<MagiaResponseDto>(magia);
        return CreatedAtAction(nameof(GetMagiaById), new { id = magia.MagiaId }, magiaCriadaDto);
    }

    [HttpPut("{id:int}")]
    public ActionResult UpdateMagia(int id, MagiaUpdateDto magiaDto)
    {
        var magiaBanco = _uof.MagiaRepository.GetMagiaById(id);
        if (magiaBanco == null) return NotFound("Magia não encontrada.");
        _mapper.Map(magiaDto,magiaBanco);
        _uof.MagiaRepository.UpdateMagia(magiaBanco);
        return Ok(_mapper.Map<MagiaResponseDto>(magiaBanco));
    }

    [HttpDelete("{id:int}")]
    public ActionResult DeleteMagia(int id)
    {
        var magia = _uof.MagiaRepository.DeleteMagia(id);
        if (magia == null) return NotFound("Magia não encontrada.");

        magia.Ativo = false;
        return Ok(new { mensagem = "Magia desativada", id });
    }
}