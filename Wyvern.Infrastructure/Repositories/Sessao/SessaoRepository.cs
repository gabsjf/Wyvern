using Microsoft.EntityFrameworkCore;
using Wyvern.Infrastructure.Data;
using SessaoEntity = Wyvern.Domain.Entities.Sessao;

namespace Wyvern.Infrastructure.Repositories.Sessao
{
    public class SessaoRepository : ISessaoRepository
    {
        private readonly WyvernDbContext _context;

        public SessaoRepository(WyvernDbContext context)
        {
            _context = context;
        }

        public IEnumerable<SessaoEntity> GetSessoes()
        {
            return _context.Sessoes
                .Include(s => s.Campanha)
                .Where(s => s.Ativo)
                .ToList();
        }

        public SessaoEntity? GetSessao(int id)
        {
            return _context.Sessoes
                .Include(s => s.Campanha)
                .FirstOrDefault(s => s.SessaoId == id && s.Ativo);
        }

        public SessaoEntity CreateSessao(SessaoEntity sessao)
        {
            if (sessao is null)
                throw new ArgumentNullException(nameof(sessao));

            _context.Sessoes.Add(sessao);
            _context.SaveChanges();

            return sessao;
        }

        public SessaoEntity UpdateSessao(SessaoEntity sessao)
        {
            if (sessao is null)
                throw new ArgumentNullException(nameof(sessao));

            _context.Entry(sessao).State = EntityState.Modified;
            _context.SaveChanges();

            return sessao;
        }

        public SessaoEntity DeleteSessao(int id)
        {
            var sessao = _context.Sessoes.Find(id);

            if (sessao is null)
                throw new ArgumentNullException(nameof(sessao));

            sessao.Ativo = false;
            _context.SaveChanges();

            return sessao;
        }
    }
}
