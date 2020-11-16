using CogShare.Domain.Interfaces;
using CogShare.EFCore.Repositories;

namespace CogShare.EFCore.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CogShareContext _context;

        public UnitOfWork(CogShareContext context)
        {
            _context = context;
            Items = new ItemRepository(_context);
            Requests = new RequestRepository(_context);
            Users = new ApplicationUserRepository(_context);
        }

        public IItemRepository Items { get; private set; }

        public IRequestRepository Requests { get; private set; }

        public IApplicationUserRepository Users { get; private set; }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
