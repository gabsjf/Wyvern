using Microsoft.EntityFrameworkCore;
using Wyvern.Infrastructure.Data;
using UsuarioEntity = Wyvern.Domain.Entities.Usuario;

namespace Wyvern.Infrastructure.Repositories.Usuario
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly WyvernDbContext _context;

        public UsuarioRepository(WyvernDbContext context)
        {
            _context = context;
        }

        public IEnumerable<UsuarioEntity> GetUsuarios()
        {
            return _context.Usuarios
                .Include(u => u.Campanhas)
                .Where(u => u.Ativo)
                .ToList();
        }

        public UsuarioEntity? GetUsuario(int id)
        {
            return _context.Usuarios
                .Include(u => u.Campanhas)
                .FirstOrDefault(u => u.UsuarioId == id && u.Ativo);
        }

        public UsuarioEntity CreateUsuario(UsuarioEntity usuario)
        {
            if (usuario is null)
                throw new ArgumentNullException(nameof(usuario));

            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            return usuario;
        }

        public UsuarioEntity UpdateUsuario(UsuarioEntity usuario)
        {
            if (usuario is null)
                throw new ArgumentNullException(nameof(usuario));

            _context.Entry(usuario).State = EntityState.Modified;
            _context.SaveChanges();

            return usuario;
        }

        public UsuarioEntity DeleteUsuario(int id)
        {
            var usuario = _context.Usuarios.Find(id);

            if (usuario is null)
                throw new ArgumentNullException(nameof(usuario));

            usuario.Ativo = false;
            _context.SaveChanges();

            return usuario;
        }
    }
}
