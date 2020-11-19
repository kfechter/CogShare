using CogShare.Domain.Entities;
using CogShare.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CogShare.EFCore.Repositories
{
    public class FriendshipRepository : GenericRepository<Friendship>, IFriendshipRepository
    {
        public FriendshipRepository(CogShareContext context)
            : base(context)
        {

        }
    }
}
