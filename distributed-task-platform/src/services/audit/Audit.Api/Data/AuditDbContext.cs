using Audit.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Audit.Api.Data
{
    public class AuditDbContext:DbContext
    {
        public AuditDbContext(DbContextOptions<AuditDbContext> options) : base(options)
        {
        }
        public DbSet<TaskOperates> Operates => base.Set<TaskOperates>();
        public DbSet<TaskOperates_Dtl> Operates_Dtl => base.Set<TaskOperates_Dtl>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("public");
        }
    }
}
