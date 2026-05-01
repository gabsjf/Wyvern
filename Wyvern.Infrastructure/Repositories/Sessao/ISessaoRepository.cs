using SessaoEntity = Wyvern.Domain.Entities.Sessao;

namespace Wyvern.Infrastructure.Repositories.Sessao
{
    public interface ISessaoRepository
    {
        IEnumerable<SessaoEntity> GetSessoes();
        SessaoEntity? GetSessao(int id);
        SessaoEntity CreateSessao(SessaoEntity sessao);
        SessaoEntity UpdateSessao(SessaoEntity sessao);
        SessaoEntity DeleteSessao(int id);
    }
}
