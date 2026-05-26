using HelpDeskManager.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HelpDeskManager.DAL.Data.Configurations;

public class SupportRequestConfiguration : IEntityTypeConfiguration<SupportRequest>
{
    public void Configure(EntityTypeBuilder<SupportRequest> builder)
    {
        builder.ToTable("SupportRequests");

        builder.HasKey(sr => sr.Id);

        builder.Property(sr => sr.Type)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(sr => sr.Subject)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(sr => sr.Description)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(sr => sr.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(30);


        builder.HasOne(sr => sr.Customer)
            .WithMany(c => c.SupportRequests)
            .HasForeignKey(sr => sr.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(sr => sr.Comments)
            .WithOne(c => c.SupportRequest) 
            .HasForeignKey(c => c.SupportRequestId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(sr => sr.History)
            .WithOne(h => h.SupportRequest)
            .HasForeignKey(h => h.SupportRequestId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(c => !c.IsDeleted);
    }
}
