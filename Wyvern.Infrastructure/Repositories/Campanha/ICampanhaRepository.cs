using System;
using System.Collections.Generic;
using System.Text;
using CampanhaEntity = Wyvern.Domain.Entities.Campanha;

namespace Wyvern.Infrastructure.Repositories.Campanha
{
   public interface ICampanhaRepository
    {
        IEnumerable<CampanhaEntity> GetCampanhas();
        CampanhaEntity? GetCampanha(int id);
        CampanhaEntity CreateCampanha(CampanhaEntity campanha);
        CampanhaEntity UpdateCampanha(CampanhaEntity campanha);
        CampanhaEntity DeleteCampanha(int id);
    }
}
