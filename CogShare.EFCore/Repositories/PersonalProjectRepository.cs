using CogShare.Domain.Entities;
using CogShare.Domain.Interfaces;

namespace CogShare.EFCore.Repositories
{
    public class PersonalProjectRepository : GenericRepository<PersonalProject>, IPersonalProjectRepository
    {
        public PersonalProjectRepository(CogShareContext context)
            : base(context)
        {

        }
    }
}
