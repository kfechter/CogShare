using CogShare.Domain.Entities;
using CogShare.Domain.Interfaces;

namespace CogShare.EFCore.Repositories
{
    public class SoftwareRepository : GenericRepository<Software>, ISoftwareRepository
    {
        public SoftwareRepository(CogShareContext context)
            : base(context)
        {

        }
    }
}
