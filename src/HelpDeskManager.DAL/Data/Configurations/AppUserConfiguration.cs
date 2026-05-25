using HelpDeskManager.Core.Entities;
using HelpDeskManager.DAL.Data.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HelpDeskManager.DAL.Data.Configurations;

internal class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.ToTable("AppUsers");

        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.DocumentType)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(u => u.DocumentNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(u => u.DocumentNumber).IsUnique();

        builder.Property(u => u.CreatedAt).IsRequired();

        builder.HasQueryFilter(c => !c.IsDeleted);
    }
}
