﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CogShare.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IApplicationUserRepository Users { get; }

        IItemRepository Items { get; }

        IRequestRepository Requests { get; }

        int Complete();
    }
}
