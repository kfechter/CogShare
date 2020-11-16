using CogShare.Domain.Entities;
using CogShare.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CogShare.EFCore.Repositories
{
    public class RequestRepository : GenericRepository<Request>, IRequestRepository
    {
        public RequestRepository(CogShareContext context)
            : base(context)
        {

        }
    }
}
