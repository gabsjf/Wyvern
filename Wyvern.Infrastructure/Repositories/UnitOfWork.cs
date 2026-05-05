using System;
using System.Collections.Generic;
using System.Text;
using Wyvern.Infrastructure.Data;
using Wyvern.Infrastructure.Repositories.Campanha;
using Wyvern.Infrastructure.Repositories.Item;
using Wyvern.Infrastructure.Repositories.Magia;
using Wyvern.Infrastructure.Repositories.Pericia;
using Wyvern.Infrastructure.Repositories.Personagem;
using Wyvern.Infrastructure.Repositories.Sessao;
using Wyvern.Infrastructure.Repositories.Usuario;


namespace Wyvern.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {

        private ICampanhaRepository _campanhaRepo;
        private IItemRepository _itemRepo;

        private IMagiaRepository _magiaRepo;

        private IPericiaRepository _periciaRepo;

        private IPersonagemRepository _personagemRepo;

        private ISessaoRepository _sessaoRepo;

        private IUsuarioRepository _usuarioRepo;
        public WyvernDbContext _context;

        public UnitOfWork (WyvernDbContext context)
        {
            _context = context;
        }

        public ICampanhaRepository CampanhaRepository
        {
            get
            {
                return _campanhaRepo = _campanhaRepo ?? new CampanhaRepository(_context);
            }
        }
        public IItemRepository ItemRepository
        {
            get
            {
                return _itemRepo = _itemRepo ?? new ItemRepository(_context);
            }
        }
        public IMagiaRepository MagiaRepository
        {
            get
            {
                return _magiaRepo = _magiaRepo ?? new MagiaRepository(_context);
            }
        }
        public IPericiaRepository PericiaRepository
        {
            get
            {
                return _periciaRepo = _periciaRepo ?? new PericiaRepository(_context);
            }
        }
        public IPersonagemRepository PersonagemRepository
        {
            get
            {
                return _personagemRepo = _personagemRepo ?? new PersonagemRepository(_context);
            }
        }
        public ISessaoRepository SessaoRepository
        {
            get
            {
                return _sessaoRepo = _sessaoRepo ?? new SessaoRepository(_context);
            }
        }
        public IUsuarioRepository UsuarioRepository
        {
            get
            {
                return _usuarioRepo = _usuarioRepo ?? new UsuarioRepository(_context);
            }
        }

        public void Commit()
        {
            _context.SaveChanges();
        }
        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
