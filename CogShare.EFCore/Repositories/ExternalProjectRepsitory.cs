using CogShare.Domain.Entities;
using CogShare.Domain.Interfaces;

namespace CogShare.EFCore.Repositories
{
    public class ExternalProjectRepository : GenericRepository<ExternalProject>, IExternalProjectRepository
    {
        public ExternalProjectRepository(CogShareContext context)
            : base(context)
        {

        }
    }
}
