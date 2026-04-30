using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Wyvern.Application.DTOs.Usuario;
using Wyvern.Domain.Entities;
using Wyvern.Infrastructure.Repositories;

namespace Wyvern.Api.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;

        public UsuarioController(IUnitOfWork uof, IMapper mapper)
        {
            _uof = uof;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<UsuarioResponseDto>> GetUsers()
        {
            var users = _uof.UsuarioRepository.GetUsuarios();
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
            var user = _uof.UsuarioRepository.GetUsuario(id);
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
            _uof.UsuarioRepository.CreateUsuario(usuario);
            var usuarioCriadoDto = _mapper.Map<UsuarioResponseDto>(usuario);
            return new CreatedAtRouteResult(nameof(GetUserById),new {id = usuario.UsuarioId},usuarioCriadoDto);
        }

        [HttpPut("{id:int}")]
        public ActionResult UpdateUser(int id, UsuarioUpdateDto usuarioDto)
        {
            var usuarioBanco = _uof.UsuarioRepository.GetUsuario(id);
            if (usuarioBanco == null)
            {
                return NotFound("Usuário não encontrado");
            }
            _mapper.Map(usuarioDto, usuarioBanco);
            _uof.UsuarioRepository.UpdateUsuario(usuarioBanco);
            return Ok(_mapper.Map<UsuarioResponseDto>(usuarioBanco));
        }

        [HttpDelete("{id:int}")]
        public ActionResult DeleteUser(int id)
        {
            var user = _uof.UsuarioRepository.DeleteUsuario(id);
            if (user == null)
            {
                return NotFound("Usuário não encontrado");
            }
            return Ok(_mapper.Map<UsuarioResponseDto>(user));
        }
    }
}
