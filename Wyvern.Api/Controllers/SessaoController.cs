using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Wyvern.Application.DTOs.Sessao;
using Wyvern.Domain.Entities;
using Wyvern.Infrastructure.Repositories;

namespace Wyvern.Api.Controllers;


[ApiController]
[Route("[Controller]")]
public class SessaoController : ControllerBase
{
    private readonly IUnitOfWork _uof;
    private readonly IMapper _mapper;

    public SessaoController(IUnitOfWork uof, IMapper mapper)
    {
        _uof = uof;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SessaoResponseDto>>> GetSessao()
    {
        var sessions = await _uof.SessaoRepository.GetSessoesAsync();
        if (!sessions.Any())
        {
            return NotFound("Nenhuma sessão encontrada");
        }
        var sessionsDto = _mapper.Map<List<SessaoResponseDto>>(sessions);
        return Ok(sessionsDto);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<SessaoResponseDto>> GetSessaoById(int id)
    {
        var session = await _uof.SessaoRepository.GetSessaoAsync(id);
        if (session == null)
        {
            return NotFound("Sessao não encontrado");
        }
        var sessionDto = _mapper.Map<SessaoResponseDto>(session);
        return Ok(sessionDto);
    }
    [HttpPost]
    public async Task<ActionResult<SessaoResponseDto>> CreateSessao(CreateSessaoDto sessaoDto)
    {
        if (sessaoDto == null) return BadRequest("Dados inválidos");

        var sessao = _mapper.Map<Sessao>(sessaoDto);
        await _uof.SessaoRepository.CreateSessaoAsync(sessao);

        var sessaoCompleta = await _uof.SessaoRepository.GetSessaoAsync(sessao.SessaoId);
        if (sessaoCompleta == null)
        {
            return CreatedAtAction(nameof(GetSessaoById), new { id = sessao.SessaoId }, null);
        }
        var sessaoCriadaDto = _mapper.Map<SessaoResponseDto>(sessaoCompleta);
        return CreatedAtAction(nameof(GetSessaoById), new { id = sessao.SessaoId }, sessaoCriadaDto);
    }
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateSessao(int id, UpdateSessaoDto sessaoDto)
    {
        var sessaoBanco = await _uof.SessaoRepository.GetSessaoAsync(id);
        if (sessaoBanco == null) return NotFound("Sessão não encontrada.");
        if (sessaoBanco.CampanhaId != sessaoDto.CampanhaId)
        {
            var campanha = await _uof.CampanhaRepository.GetCampanhaAsync(sessaoDto.CampanhaId);
            if (campanha == null) return BadRequest("A nova campanha informada não existe");
        }

        _mapper.Map(sessaoDto, sessaoBanco);

        await _uof.SessaoRepository.UpdateSessaoAsync(sessaoBanco);
        var sessaoAtualizadaDto = _mapper.Map<SessaoResponseDto>(sessaoBanco);
        return Ok(sessaoAtualizadaDto);
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteSessao(int id)
    {
        var sessao = await _uof.SessaoRepository.DeleteSessaoAsync(id);
        if (sessao == null) return NotFound("Sessão não encontrada");
        return Ok("Sessão deletada com sucesso");

    }

}
