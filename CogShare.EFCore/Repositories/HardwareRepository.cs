using CogShare.Domain.Entities;
using CogShare.Domain.Interfaces;

namespace CogShare.EFCore.Repositories
{
    public class HardwareRepository : GenericRepository<Hardware>, IHardwareRepository
    {
        public HardwareRepository(CogShareContext context)
            : base(context)
        {

        }
    }
}
