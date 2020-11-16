using CogShare.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CogShare.EFCore
{
    public class CogShareContext : IdentityDbContext<ApplicationUser>
    {
        public CogShareContext(DbContextOptions<CogShareContext> options)
        : base(options)
        {
        }

        public DbSet<Item> Items { get; set; }

        public DbSet<Request> Requests { get; set; }
    }
}
