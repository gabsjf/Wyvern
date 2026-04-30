using PersonagemEntity = Wyvern.Domain.Entities.Personagem;

namespace Wyvern.Infrastructure.Repositories.Personagem
{
    public interface IPersonagemRepository
    {
        IEnumerable<PersonagemEntity> GetPersonagens();
        PersonagemEntity? GetPersonagem(int id);
        PersonagemEntity CreatePersonagem(PersonagemEntity personagem);
        PersonagemEntity UpdatePersonagem(PersonagemEntity personagem);
        PersonagemEntity DeletePersonagem(int id);
    }
}
