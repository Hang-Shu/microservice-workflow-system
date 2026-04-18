using Microsoft.EntityFrameworkCore;
using Notification.Api.Entities;

namespace Notification.Api.Data
{
    public class NotificationDbContext : DbContext
    {
        public NotificationDbContext(DbContextOptions<NotificationDbContext> options) : base(options)
        {
        }

        public DbSet<Notifications> Notifications => base.Set<Notifications>();
        public DbSet<Notifications_Dtl> Notifications_Dtl => base.Set<Notifications_Dtl>();
        public DbSet<NotificationInbox> NotificationInbox => base.Set<NotificationInbox>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("public");
        }

    }
}
