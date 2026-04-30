using PericiaEntity = Wyvern.Domain.Entities.Pericia;

namespace Wyvern.Infrastructure.Repositories.Pericia
{
    public interface IPericiaRepository
    {
        IEnumerable<PericiaEntity> GetPericias();
        PericiaEntity? GetPericia(int id);
        PericiaEntity CreatePericia(PericiaEntity pericia);
        PericiaEntity UpdatePericia(PericiaEntity pericia);
        PericiaEntity DeletePericia(int id);
    }
}
