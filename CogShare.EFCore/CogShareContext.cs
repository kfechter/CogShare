using CogShare.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CogShare.EFCore
{
    public class CogShareContext : IdentityDbContext<CogShareUser>
    {
        public CogShareContext(DbContextOptions<CogShareContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public virtual DbSet<CogShareUser>? CogShareUser { get; set; }

        public DbSet<Documentation>? Docs { get; set; }

        public DbSet<ExternalProject>? ExternalProjects { get; set; }

        public DbSet<Hardware>? Hardware { get; set; }

        public DbSet<PersonalProject>? PersonalProjects { get; set; }

        public DbSet<Software>? Software { get; set; }

        public DbSet<SoftwareLibrary>? SoftwareLibraries { get; set; }

    }
}
