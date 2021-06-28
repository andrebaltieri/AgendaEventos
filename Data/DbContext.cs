using Agenda.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Agenda.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<User> Users { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries().Where(e => e.State == EntityState.Modified))
            {
                if (entry.Metadata.GetProperties().Any(p => p.Name == nameof(Entity.LastUpdatedDate)))
                {
                    entry.Property(nameof(Entity.LastUpdatedDate)).CurrentValue = DateTime.UtcNow;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.LogTo(Console.WriteLine);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<User>()
                .HasMany(u => u.Roles)
                .WithMany(r => r.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserRoles",
                    ur => ur
                    .HasOne<Role>()
                    .WithMany()
                    .HasForeignKey("RoleId")
                    .HasConstraintName("FK_UserRoles_Role_RoleId")
                    .OnDelete(DeleteBehavior.Cascade),
                    ur => ur
                    .HasOne<User>()
                    .WithMany()
                    .HasForeignKey("UserId")
                    .HasConstraintName("FK_UserRoles_User_UserId")
                    .OnDelete(DeleteBehavior.ClientCascade));
        }
    }
}