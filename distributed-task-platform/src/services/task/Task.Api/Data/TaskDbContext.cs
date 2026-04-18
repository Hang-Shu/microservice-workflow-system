using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using Task.Api.Entities;

namespace Task.Api.Data
{
    public class TaskDbContext: DbContext
    {
        public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options)
        {
        }

        public DbSet<TaskItems> Tasks => base.Set<TaskItems>();
        public DbSet<Projects> Projects => base.Set<Projects>();
        public DbSet<Users> Users => base.Set<Users>();
        public DbSet<TaskOperates> TaskOperates => base.Set<TaskOperates>();
        public DbSet<TaskOperates_Dtl> TaskOperates_Dtl => base.Set<TaskOperates_Dtl>();
        public DbSet<TaskComment> TaskComment => base.Set<TaskComment>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("public");

            //Let Table "TaskItems"-"TaskNumber" set automatically
            modelBuilder.Entity<TaskItems>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.TaskNumber)
                    .UseIdentityAlwaysColumn();

                entity.HasIndex(x => x.TaskNumber)
                    .IsUnique();
            });

            //Let Table "Users"-"UserNumber" set automatically
            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.UserNumber)
                    .UseIdentityAlwaysColumn();

                entity.HasIndex(x => x.UserNumber)
                    .IsUnique();
            });
        }
    }
}
