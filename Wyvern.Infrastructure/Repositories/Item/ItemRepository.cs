using Microsoft.EntityFrameworkCore;
using Wyvern.Infrastructure.Data;
using ItemEntity = Wyvern.Domain.Entities.Item;

namespace Wyvern.Infrastructure.Repositories.Item
{
    public class ItemRepository : IItemRepository
    {
        private readonly WyvernDbContext _context;

        public ItemRepository(WyvernDbContext context)
        {
            _context = context;
        }


        public ItemEntity DeleteItem(int id)
        {
            var item = _context.Itens.Find(id);

            if (item is null)
                throw new ArgumentNullException(nameof(item));

            item.Ativo = false;
           

            return item;
        }

        public ItemEntity? GetItem(int id)
        {
            return _context.Itens.FirstOrDefault(i => i.ItemId == id && i.Ativo);
        }

        public IEnumerable<ItemEntity> GetItens()
        {
            return _context.Itens
                .Where(i => i.Ativo)
                .ToList();
        }

        public ItemEntity UpdateItem(ItemEntity item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            _context.Entry(item).State = EntityState.Modified;
           

            return item;
        }

        public ItemEntity CreateItem(ItemEntity item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            _context.Itens.Add(item);

            return item;
        }

    }
}
