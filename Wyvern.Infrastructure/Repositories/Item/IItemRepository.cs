using System;
using System.Collections.Generic;
using System.Text;
using ItemEntity = Wyvern.Domain.Entities.Item;


namespace Wyvern.Infrastructure.Repositories.Item
{
    internal interface IItemRepository
    {
        IEnumerable<ItemEntity> GetItens();
        ItemEntity? GetItem(int id);
        ItemEntity CreateItem(ItemEntity item);
        ItemEntity UpdateItem(ItemEntity item);
        ItemEntity DeleteItem(int id);
    }
}
