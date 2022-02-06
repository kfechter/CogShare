using CogShare.Domain.Entities;
using CogShare.Domain.Interfaces;

namespace CogShare.EFCore.Repositories
{
    public class SoftwareLibraryRepository : GenericRepository<SoftwareLibrary>, ISoftwareLibraryRepository
    {
        public SoftwareLibraryRepository(CogShareContext context)
            : base(context)
        {

        }
    }
}
