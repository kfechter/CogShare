using CogShare.Domain.Entities;
using CogShare.Domain.Interfaces;

namespace CogShare.EFCore.Repositories
{
    public class DocumentationRepository : GenericRepository<Documentation>, IDocumentationRepository
    {
        public DocumentationRepository(CogShareContext context)
            : base(context)
        {
        }
    }
}
