using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Wyvern.Domain.Entities;
using Wyvern.Infrastructure.Data;
using MagiaEntity = Wyvern.Domain.Entities.Magia;


namespace Wyvern.Infrastructure.Repositories.Magia
{
    public class MagiaRepository : IMagiaRepository
    {
        private readonly WyvernDbContext _context;
        public MagiaRepository(WyvernDbContext context)
        {
            _context = context;
        }

        public MagiaEntity CreateMagia(MagiaEntity magia)
        {
            if (magia is null)
                throw new ArgumentNullException(nameof(magia));

            _context.Magias.Add(magia);
            return magia;
        }

        public MagiaEntity DeleteMagia(int id)
        {
            var magia = _context.Magias.Find(id);
            if (magia is null)
                throw new ArgumentNullException(nameof(magia));
            magia.Ativo = false;
            return magia;
        }

        public MagiaEntity? GetMagiaById(int id)
        {
            return _context.Magias.FirstOrDefault(m => m.MagiaId == id);
        }

        public IEnumerable<MagiaEntity> GetMagias()
        {
            return _context.Magias
                    .Where(m => m.Ativo)
                    .ToList();
        }

        public MagiaEntity UpdateMagia(MagiaEntity magia)
        {
            if (magia is null)
                throw new ArgumentNullException(nameof(magia));

            _context.Entry(magia).State = EntityState.Modified;

            return magia;
        }
    }
}
