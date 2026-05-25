using HelpDeskManager.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HelpDeskManager.DAL.Data.Configurations;

public class RequestHistoryConfiguration : IEntityTypeConfiguration<RequestHistory>
{
    public void Configure(EntityTypeBuilder<RequestHistory> builder)
    {
        builder.ToTable("RequestHistories");

        builder.HasKey(rh => rh.Id);

        builder.Property(rh => rh.PreviousStatus)
            .HasConversion<string>()
            .HasMaxLength(30);

        builder.Property(rh => rh.NewStatus)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(30);

        builder.Property(rh => rh.ChangeNotes)
            .HasMaxLength(500);

        builder.Property(rh => rh.ModifiedByUserId)
            .IsRequired()
            .HasMaxLength(450);

        builder.HasOne(rh => rh.SupportRequest)
            .WithMany(sr => sr.History)
            .HasForeignKey(rh => rh.SupportRequestId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(rh => !rh.SupportRequest.IsDeleted);
    }
}
