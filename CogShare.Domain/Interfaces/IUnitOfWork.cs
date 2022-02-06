using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CogShare.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IDocumentationRepository Docs { get; }

        IExternalProjectRepository ExternalProjects { get; }

        IHardwareRepository Hardware { get; }

        IPersonalProjectRepository PersonalProjects { get; }

        ISoftwareLibraryRepository SoftwareLibraries { get; }

        ISoftwareRepository Software { get; }

        ICogShareUserRepository CogShareUsers { get; }

        int Complete();
    }
}
