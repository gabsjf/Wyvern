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

        public async Task<IEnumerable<CampanhaEntity>> GetCampanhasAsync()
        {
            return await _context.Campanhas
                .Include(c => c.Mestre)
                .Include(c => c.Sessoes)
                .Where(c => c.Ativo)
                .ToListAsync();
        }

        public async Task<CampanhaEntity?> GetCampanhaAsync(int id)
        {
            return await _context.Campanhas
                .Include(c => c.Mestre)
                .Include(c => c.Sessoes)
                .FirstOrDefaultAsync(c => c.CampanhaId == id && c.Ativo);
        }

        public async Task<CampanhaEntity> CreateCampanhaAsync(CampanhaEntity campanha)
        {
            if (campanha is null)
                throw new ArgumentNullException(nameof(campanha));

            _context.Campanhas.Add(campanha);
            await _context.SaveChangesAsync();

            return campanha;
        }

        public async Task<CampanhaEntity> UpdateCampanhaAsync(CampanhaEntity campanha)
        {
            if (campanha is null)
                throw new ArgumentNullException(nameof(campanha));

            _context.Entry(campanha).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return campanha;
        }

        public async Task<CampanhaEntity> DeleteCampanhaAsync(int id)
        {
            var campanha = await _context.Campanhas.FindAsync(id);

            if (campanha is null)
                throw new ArgumentNullException(nameof(campanha));

            campanha.Ativo = false;
            await _context.SaveChangesAsync();

            return campanha;
        }
    }
}