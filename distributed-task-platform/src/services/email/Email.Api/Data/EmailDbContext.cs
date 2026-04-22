using Email.Api.Dtos;
using Email.Api.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Email.Api.Data
{
    public class EmailDbContext:DbContext
    {
        public EmailDbContext(DbContextOptions<EmailDbContext> options) : base(options)
        {
        }
        public DbSet<Emails> Emails => base.Set<Emails>();
        public DbSet<TasksEmailsPending> TasksEmailsPending => base.Set<TasksEmailsPending>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("public");
        }
    }
}
