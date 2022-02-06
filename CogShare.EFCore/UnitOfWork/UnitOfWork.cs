using CogShare.EFCore.Repositories;
using CogShare.Domain.Interfaces;

namespace CogShare.EFCore.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CogShareContext _context;

        public UnitOfWork(CogShareContext context)
        {
            _context = context;
            Docs = new DocumentationRepository(_context);
            ExternalProjects = new ExternalProjectRepository(_context);
            Hardware = new HardwareRepository(_context);
            PersonalProjects = new PersonalProjectRepository(_context);
            SoftwareLibraries = new SoftwareLibraryRepository(_context);
            Software = new SoftwareRepository(_context);
            CogShareUsers = new CogShareUserRepository(_context);
        }

        public ICogShareUserRepository CogShareUsers { get; private set; }

        public IDocumentationRepository Docs { get; private set; }

        public IExternalProjectRepository ExternalProjects { get; private set; }

        public IHardwareRepository Hardware { get; private set; }

        public IPersonalProjectRepository PersonalProjects { get; private set; }

        public ISoftwareLibraryRepository SoftwareLibraries { get; private set; }

        public ISoftwareRepository Software { get; private set; }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
