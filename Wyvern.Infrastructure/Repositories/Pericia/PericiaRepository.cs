using Microsoft.EntityFrameworkCore;
using Wyvern.Infrastructure.Data;
using PericiaEntity = Wyvern.Domain.Entities.Pericia;

namespace Wyvern.Infrastructure.Repositories.Pericia
{
    public class PericiaRepository : IPericiaRepository
    {
        private readonly WyvernDbContext _context;

        public PericiaRepository(WyvernDbContext context)
        {
            _context = context;
        }

        public IEnumerable<PericiaEntity> GetPericias()
        {
            return _context.Pericias
                .Where(p => p.Ativo)
                .ToList();
        }

        public PericiaEntity? GetPericia(int id)
        {
            return _context.Pericias.FirstOrDefault(p => p.PericiaId == id && p.Ativo);
        }

        public PericiaEntity CreatePericia(PericiaEntity pericia)
        {
            if (pericia is null)
                throw new ArgumentNullException(nameof(pericia));

            _context.Pericias.Add(pericia);

            return pericia;
        }

        public PericiaEntity UpdatePericia(PericiaEntity pericia)
        {
            if (pericia is null)
                throw new ArgumentNullException(nameof(pericia));

            _context.Entry(pericia).State = EntityState.Modified;

            return pericia;
        }

        public PericiaEntity DeletePericia(int id)
        {
            var pericia = _context.Pericias.Find(id);

            if (pericia is null)
                throw new ArgumentNullException(nameof(pericia));

            pericia.Ativo = false;

            return pericia;
        }
    }
}
