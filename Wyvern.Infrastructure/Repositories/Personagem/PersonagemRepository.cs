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

        public IEnumerable<PersonagemEntity> GetPersonagens()
        {
            return _context.Personagens
                .AsNoTracking()
                .Include(p => p.Atributo)
                .Include(p => p.PersonagemPlayer)
                .Include(p => p.PersonagemCombate)
                .Where(p => p.Ativo)
                .ToList();
        }

        public PersonagemEntity? GetPersonagem(int id)
        {
            return _context.Personagens
                .Include(p => p.Atributo)
                .Include(p => p.PersonagemPlayer)
                .Include(p => p.PersonagemCombate)
                .FirstOrDefault(p => p.PersonagemId == id && p.Ativo);
        }

        public PersonagemEntity CreatePersonagem(PersonagemEntity personagem)
        {
            if (personagem is null)
                throw new ArgumentNullException(nameof(personagem));

            _context.Personagens.Add(personagem);

            return personagem;
        }

        public PersonagemEntity UpdatePersonagem(PersonagemEntity personagem)
        {
            if (personagem is null)
                throw new ArgumentNullException(nameof(personagem));

            _context.Entry(personagem).State = EntityState.Modified;

            return personagem;
        }

        public PersonagemEntity DeletePersonagem(int id)
        {
            var personagem = _context.Personagens.Find(id);

            if (personagem is null)
                throw new ArgumentNullException(nameof(personagem));

            personagem.Ativo = false;

            return personagem;
        }
    }
}
