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

        public DbSet<TaskItem> Tasks => Set<TaskItem>();
    }
}
