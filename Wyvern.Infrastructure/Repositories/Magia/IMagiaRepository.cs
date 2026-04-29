using System;
using System.Collections.Generic;
using System.Text;
using MagiaEntity = Wyvern.Domain.Entities.Magia;


namespace Wyvern.Infrastructure.Repositories.Magia
{
    internal interface IMagiaRepository
    {
        IEnumerable<MagiaEntity> GetMagias();
        MagiaEntity? GetMagiaById(int id);
        MagiaEntity CreateMagia(MagiaEntity magia);
        MagiaEntity UpdateMagia(MagiaEntity magia);
        MagiaEntity DeleteMagia(int id);
    }
}
