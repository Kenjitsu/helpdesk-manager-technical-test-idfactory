using HelpDeskManager.Core.Entities;
using HelpDeskManager.Core.Interfaces.Entities;
using HelpDeskManager.DAL.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace HelpDeskManager.DAL.Data;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<SupportRequest> SupportRequests { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<RequestHistory> RequestHistories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<IdentityRole>()
            .HasData(
                new IdentityRole { Id = "8403bc3d-89b6-4190-b3a6-d41d57e7b03f", Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "45e21001-451d-46e6-a2a0-aeb61de41b65", Name = "Agent", NormalizedName = "AGENT" },
                new IdentityRole { Id = "664d3e82-7b78-4b56-a7ff-f2ee0fb41f64", Name = "Reader", NormalizedName = "READER" }
            );

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Manejar el update de las propiedades de auditoría y soft delete.

        var auditableEntries = ChangeTracker.Entries<IAuditableEntity>();
        foreach (var entry in auditableEntries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
        }

        var softDeletableEntries = ChangeTracker.Entries<ISoftDeletable>();
        foreach (var entry in softDeletableEntries)
        {
            if (entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Modified;

                entry.Entity.IsDeleted = true;
                entry.Entity.DeletedAt = DateTime.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
