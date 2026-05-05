using Microsoft.EntityFrameworkCore;
using Wyvern.Infrastructure.Data;
using PersonagemEntity = Wyvern.Domain.Entities.Personagem;

namespace Wyvern.Infrastructure.Repositories.Personagem
{
    public class PersonagemRepository : IPersonagemRepository
    {
        private readonly WyvernDbContext _context;

        public PersonagemRepository(WyvernDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PersonagemEntity>> GetPersonagensAsync()
        {
            return await _context.Personagens
                .AsNoTracking()
                .Include(p => p.Atributo)
                .Include(p => p.PersonagemPlayer)
                .Include(p => p.PersonagemCombate)
                .Where(p => p.Ativo)
                .ToListAsync();
        }

        public async Task<PersonagemEntity?> GetPersonagemAsync(int id)
        {
            return await _context.Personagens
                .Include(p => p.Atributo)
                .Include(p => p.PersonagemPlayer)
                .Include(p => p.PersonagemCombate)
                .FirstOrDefaultAsync(p => p.PersonagemId == id && p.Ativo);
        }

        public async Task<PersonagemEntity> CreatePersonagemAsync(PersonagemEntity personagem)
        {
            if (personagem is null)
                throw new ArgumentNullException(nameof(personagem));

            _context.Personagens.Add(personagem);
            await _context.SaveChangesAsync();

            return personagem;
        }

        public async Task<PersonagemEntity> UpdatePersonagemAsync(PersonagemEntity personagem)
        {
            if (personagem is null)
                throw new ArgumentNullException(nameof(personagem));

            _context.Entry(personagem).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return personagem;
        }

        public async Task<PersonagemEntity> DeletePersonagemAsync(int id)
        {
            var personagem = await _context.Personagens.FindAsync(id);

            if (personagem is null)
                throw new ArgumentNullException(nameof(personagem));

            personagem.Ativo = false;
            await _context.SaveChangesAsync();

            return personagem;
        }
    }
}
