using Microsoft.EntityFrameworkCore;
using CampanhaEntity = Wyvern.Domain.Entities.Campanha;
using Wyvern.Infrastructure.Data;

namespace Wyvern.Infrastructure.Repositories.Campanha
{
    public class CampanhaRepository : ICampanhaRepository
    {
        private readonly WyvernDbContext _context;

        public CampanhaRepository(WyvernDbContext context)
        {
            _context = context;
        }

        public IEnumerable<CampanhaEntity> GetCampanhas()
        {
            return _context.Campanhas
                .Include(c => c.Mestre)
                .Include(c => c.Sessoes)
                .Where(c => c.Ativo)
                .ToList();
        }

        public CampanhaEntity? GetCampanha(int id)
        {
            return _context.Campanhas
                .Include(c => c.Mestre)
                .Include(c => c.Sessoes)
                .FirstOrDefault(c => c.CampanhaId == id && c.Ativo);
        }

        public CampanhaEntity CreateCampanha(CampanhaEntity campanha)
        {
            if (campanha is null)
                throw new ArgumentNullException(nameof(campanha));

            _context.Campanhas.Add(campanha);
            _context.SaveChanges();

            return campanha;
        }

        public CampanhaEntity UpdateCampanha(CampanhaEntity campanha)
        {
            if (campanha is null)
                throw new ArgumentNullException(nameof(campanha));

            _context.Entry(campanha).State = EntityState.Modified;
            _context.SaveChanges();

            return campanha;
        }

        public CampanhaEntity DeleteCampanha(int id)
        {
            var campanha = _context.Campanhas.Find(id);

            if (campanha is null)
                throw new ArgumentNullException(nameof(campanha));

            campanha.Ativo = false;
            _context.SaveChanges();

            return campanha;
        }
    }
}