using CogShare.Domain.Entities;
using CogShare.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CogShare.EFCore.Repositories
{
    public class ApplicationUserRepository : GenericRepository<ApplicationUser>, IApplicationUserRepository
    {
        public ApplicationUserRepository(CogShareContext context)
            : base(context)
        {

        }
    }
}
