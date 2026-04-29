using UsuarioEntity = Wyvern.Domain.Entities.Usuario;

namespace Wyvern.Infrastructure.Repositories.Usuario
{
    internal interface IUsuarioRepository
    {
        IEnumerable<UsuarioEntity> GetUsuarios();
        UsuarioEntity? GetUsuario(int id);
        UsuarioEntity CreateUsuario(UsuarioEntity usuario);
        UsuarioEntity UpdateUsuario(UsuarioEntity usuario);
        UsuarioEntity DeleteUsuario(int id);
    }
}
