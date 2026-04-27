using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wyvern.Application.DTOs.Sessao;
using Wyvern.Domain.Entities;
using Wyvern.Infrastructure.Data;

namespace Wyvern.Api.Controllers;


[ApiController]
[Route("[Controller]")]
public class SessaoController : ControllerBase
{
    private readonly WyvernDbContext _context;
    private readonly IMapper _mapper;

    public SessaoController(WyvernDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<SessaoResponseDto>> GetSessao()
    {
        var sessions = _context.Sessoes.Include(s => s.Campanha)
                        .Where(s => s.Ativo)
                        .ToList();
        if (!sessions.Any())
        {
            return NotFound("Nenhuma sessão encontrada");
        }
        var sessionsDto = _mapper.Map<List<SessaoResponseDto>>(sessions);
        return Ok(sessionsDto);
    }

    [HttpGet("{id:int}")]
    public ActionResult<SessaoResponseDto> GetSessaoById(int id)
    {
        var session = _context.Sessoes.Include(s => s.Campanha)
            .FirstOrDefault(s => s.SessaoId == id && s.Ativo);
        if (session == null)
        {
            return NotFound("Sessao não encontrado");
        }
        var sessionDto = _mapper.Map<SessaoResponseDto>(session);
        return Ok(sessionDto);
    }
    [HttpPost]
    public ActionResult<SessaoResponseDto> CreateSessao(CreateSessaoDto sessaoDto)
    {
        if (sessaoDto == null) return BadRequest("Dados inválidos");

        var sessao = _mapper.Map<Sessao>(sessaoDto);
        _context.Sessoes.Add(sessao);
        _context.SaveChanges();

        var sessaoCompleta = _context.Sessoes
            .Include(s => s.Campanha)
            .FirstOrDefault(s => s.SessaoId == sessao.SessaoId);
        if (sessaoCompleta == null)
        {
            return CreatedAtAction(nameof(GetSessaoById), new { id = sessao.SessaoId }, null);
        }
        var sessaoCriadaDto = _mapper.Map<SessaoResponseDto>(sessaoCompleta);
        return CreatedAtAction(nameof(GetSessaoById), new { id = sessao.SessaoId }, sessaoCriadaDto);
    }
    [HttpPut("{id:int}")]
    public ActionResult UpdateSessao(int id, UpdateSessaoDto sessaoDto)
    {
        var sessaoBanco = _context.Sessoes.FirstOrDefault(s => s.SessaoId == id && s.Ativo);
        if (sessaoBanco == null) return NotFound("Sessão não encontrada.");
        if (sessaoBanco.CampanhaId != sessaoDto.CampanhaId)
        {
            var campanhaTrue = _context.Campanhas.Any(c => c.CampanhaId == sessaoDto.CampanhaId);
            if (!campanhaTrue) return BadRequest("A nova campanha informada não existe");
        }

        _mapper.Map(sessaoDto, sessaoBanco);

        _context.SaveChanges();
        _context.Entry(sessaoBanco).Reference(s => s.Campanha).Load();
        var sessaoAtualizadaDto = _mapper.Map<SessaoResponseDto>(sessaoBanco);
        return Ok(sessaoAtualizadaDto);
    }

    [HttpDelete]
    public ActionResult DeleteSessao(int id)
    {
        var sessao = _context.Sessoes.FirstOrDefault(s => s.SessaoId == id);
        if (sessao == null) return NotFound("Sessão não encontrada");

        sessao.Ativo = false;

        _context.SaveChanges();

        return Ok("Sessão deletada com sucesso");

    }

}
