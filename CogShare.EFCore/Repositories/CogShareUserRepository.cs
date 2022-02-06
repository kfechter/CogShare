
using CogShare.Domain.Entities;
using CogShare.Domain.Interfaces;

namespace CogShare.EFCore.Repositories
{
    public class CogShareUserRepository : GenericRepository<CogShareUser>, ICogShareUserRepository
    {
        public CogShareUserRepository(CogShareContext context)
            : base(context)
        {

        }
    }
}
