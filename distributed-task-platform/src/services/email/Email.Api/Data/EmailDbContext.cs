using Email.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Email.Api.Data
{
    public class EmailDbContext:DbContext
    {
        public EmailDbContext(DbContextOptions<EmailDbContext> options) : base(options)
        {
        }
        public DbSet<Emails> Emails => base.Set<Emails>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("public");
        }
    }
}
