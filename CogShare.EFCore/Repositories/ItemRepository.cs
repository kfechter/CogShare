using CogShare.Domain.Entities;
using CogShare.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CogShare.EFCore.Repositories
{
    public class ItemRepository : GenericRepository<Item>, IItemRepository
    {
        public ItemRepository(CogShareContext context)
            : base(context)
        {

        }
    }
}
