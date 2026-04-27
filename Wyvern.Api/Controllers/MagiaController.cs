using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Wyvern.Application.DTOs.Magia;
using Wyvern.Domain.Entities;
using Wyvern.Infrastructure.Data;

[ApiController]
[Route("[controller]")]
public class MagiaController : ControllerBase
{
    private readonly WyvernDbContext _context;
    private readonly IMapper _mapper;

    public MagiaController(WyvernDbContext context, IMapper mapper )
    {
        _context = context;
        _mapper = mapper;

    }

    [HttpGet]
    public ActionResult<IEnumerable<MagiaResponseDto>> GetMagias()
    {
        var magias = _context.Magias.Where(i => i.Ativo).ToList();
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
        var magia = _context.Magias.FirstOrDefault(m => m.MagiaId == id && m.Ativo);
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
        _context.Magias.Add(magia);
        _context.SaveChanges();
        var magiaCriadaDto = _mapper.Map<MagiaResponseDto>(magia);
        return CreatedAtAction(nameof(GetMagiaById), new { id = magia.MagiaId }, magiaCriadaDto);
    }

    [HttpPut("{id:int}")]
    public ActionResult UpdateMagia(int id, MagiaUpdateDto magiaDto)
    {
        var magiaBanco = _context.Magias.FirstOrDefault(m => m.MagiaId == id && m.Ativo);
        if (magiaBanco == null) return NotFound("Magia não encontrada.");
        _mapper.Map(magiaDto,magiaBanco);
        _context.SaveChanges();
        return Ok(_mapper.Map<MagiaResponseDto>(magiaBanco));
    }

    [HttpDelete("{id:int}")]
    public ActionResult DeleteMagia(int id)
    {
        var magia = _context.Magias.FirstOrDefault(m => m.MagiaId == id);
        if (magia == null) return NotFound("Magia não encontrada.");

        magia.Ativo = false;
        _context.SaveChanges();
        return Ok(new { mensagem = "Magia desativada", id });
    }
}