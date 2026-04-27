using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Wyvern.Application.DTOs.Magia;
using Wyvern.Application.DTOs.Pericia;
using Wyvern.Domain.Entities;
using Wyvern.Infrastructure.Data;

[ApiController]
[Route("[controller]")]
public class PericiaController : ControllerBase
{
    private readonly WyvernDbContext _context;
    private readonly IMapper _mapper;

    public PericiaController(WyvernDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<PericiaResponseDto>> GetPericias()
    {
        var pericias =_context.Pericias.Where(p => p.Ativo).ToList();
        if (!pericias.Any()) {
            return NotFound("Pericia não encontrada");
        }
        var periciasDto = _mapper.Map<List<PericiaResponseDto>>(pericias);
        return Ok(periciasDto);
    }

    [HttpGet("{id:int}")]
    public ActionResult<PericiaResponseDto> GetPericiaById(int id)
    {
        var pericia = _context.Pericias.FirstOrDefault(p => p.PericiaId == id && p.Ativo);
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
        _context.Pericias.Add(pericia);
        _context.SaveChanges();
        var periciaCriadaDto = _mapper.Map<PericiaResponseDto>(pericia);
        return CreatedAtAction(nameof(GetPericiaById), new { id = pericia.PericiaId }, periciaCriadaDto);
    }

    [HttpPut("{id:int}")]
    public ActionResult UpdatePericia(int id, PericiaUpdateDto periciaDto)
    {
        var periciaBanco = _context.Pericias.FirstOrDefault(p => p.PericiaId == id && p.Ativo);
        if (periciaBanco == null) return NotFound("Perícia não encontrada.");
        _mapper.Map(periciaDto, periciaBanco);
        _context.SaveChanges();
        return Ok(_mapper.Map<PericiaResponseDto>(periciaBanco));
    }

    [HttpDelete("{id:int}")]
    public ActionResult DeletePericia(int id)
    {
        var pericia = _context.Pericias.FirstOrDefault(p => p.PericiaId == id);
        if (pericia == null) return NotFound("Perícia não encontrada.");

        pericia.Ativo = false;
        _context.SaveChanges();
        return Ok(new { mensagem = "Perícia desativada", id });
    }
}