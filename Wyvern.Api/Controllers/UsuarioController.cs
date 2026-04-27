using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wyvern.Application.DTOs.Usuario;
using Wyvern.Domain.Entities;
using Wyvern.Infrastructure.Data;

namespace Wyvern.Api.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly WyvernDbContext _context;
        private readonly IMapper _mapper;

        public UsuarioController(WyvernDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<UsuarioResponseDto>> GetUsers()
        {
            var users = _context.Usuarios
                .Include(u => u.Campanhas)
                .Where(u => u.Ativo)
                .ToList();
            if (!users.Any())
            {
                return NotFound("Nenhum usuário encontrado no banco.");
            }

            var usersDto = _mapper.Map<List<UsuarioResponseDto>>(users);
            return Ok(usersDto);
        }
        [HttpGet("{id:int}")]
        public ActionResult<UsuarioResponseDto> GetUserById(int id)
        {
            var user = _context.Usuarios
                .Include(u => u.Campanhas)
                .FirstOrDefault(u => u.UsuarioId == id && u.Ativo);
            if ( user == null)
            {
                return NotFound("Usuário não encontrado");
            }
            var userDto = _mapper.Map<UsuarioResponseDto>(user);
            return Ok(userDto);
        }

        [HttpPost]
        public ActionResult<UsuarioResponseDto> CreateUser (CreateUsuarioDto usuarioDto)
        {
            if (usuarioDto == null)
            {
                return BadRequest("Usuário inválido.");
            }
            var usuario = _mapper.Map<Usuario>(usuarioDto);
            usuario.CriadoEm = DateTime.Now;
            usuario.Ativo = true;
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();
            var usuarioCriadoDto = _mapper.Map<UsuarioResponseDto>(usuario);
            return new CreatedAtRouteResult(nameof(GetUserById),new {id = usuario.UsuarioId},usuarioCriadoDto);
        }

        [HttpPut("{id:int}")]
        public ActionResult UpdateUser(int id, UsuarioUpdateDto usuarioDto)
        {
            var usuarioBanco = _context.Usuarios.FirstOrDefault(u => u.UsuarioId == id && u.Ativo);
            if (usuarioBanco == null)
            {
                return NotFound("Usuário não encontrado");
            }
            _mapper.Map(usuarioDto, usuarioBanco);
            _context.SaveChanges();
            return Ok(_mapper.Map<UsuarioResponseDto>(usuarioBanco));
        }

        [HttpDelete("{id:int}")]
        public ActionResult DeleteUser(int id)
        {
            var user = _context.Usuarios.FirstOrDefault(u => u.UsuarioId == id);
            if (user == null)
            {
                return NotFound("Usuário não encontrado");
            }
            user.Ativo = false;
            _context.SaveChanges();
            return Ok(_mapper.Map<UsuarioResponseDto>(user));
        }
    }
}
