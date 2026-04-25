using Microsoft.EntityFrameworkCore;
using Query.Api.Entities;

namespace Query.Api.Data
{
    public class QueryDbContext : DbContext
    {
        public QueryDbContext(DbContextOptions<QueryDbContext> options) : base(options)
        {
        }
        public DbSet<TaskCommentRead> TaskCommentRead => base.Set<TaskCommentRead>();
        public DbSet<TaskEmailSummaryRead> TaskEmailSummaryRead => base.Set<TaskEmailSummaryRead>();
        public DbSet<TaskModifyLineRead> TaskModifyLineRead => base.Set<TaskModifyLineRead>();
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskEmailSummaryRead>().HasIndex(x => x.IdempotentId).IsUnique();
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("public");
        }
    }
}
