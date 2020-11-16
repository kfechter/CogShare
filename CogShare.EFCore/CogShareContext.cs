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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Request>()
                .Property(s => s.Created)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<Item>()
                .Property(s => s.Created)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }

        public DbSet<Item> Items { get; set; }

        public DbSet<Request> Requests { get; set; }
    }
}
