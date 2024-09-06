using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using VolunterProg.Domain.Shared;
using VolunterProg.Domain.Voluunters;

namespace VolunterProg.Infrastructure.Configurations;

public class ValuunterConfiguration: IEntityTypeConfiguration<Voluunter>
{
    public void Configure(EntityTypeBuilder<Voluunter> builder)
    {
        builder.ToTable("voluunters");
        builder.HasKey(v => v.Id);
        builder.Property(v => v.Id)
            .HasConversion(
                id => id.Value,
                value => VoluunterId.Create(value));
        builder.Property(v => v.FullName)
            .IsRequired()
            .HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH);
        builder.Property(v => v.Email)
            .IsRequired()
            .HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH);
        builder.Property(v => v.Description)
            .HasMaxLength(Constants.MAX_LARGE_TEXT_LENGTH);
        builder.Property(v => v.Experience)
            .HasMaxLength(Constants.MAX_LARGE_TEXT_LENGTH);
        builder.Property(v => v.PhoneNumber)
            .HasMaxLength(Constants.MAX_LARGE_TEXT_LENGTH);
        builder.HasMany(v => v.Pets)
            .WithOne()
            .HasForeignKey("volunter_id");

        builder.OwnsOne(v => v.Details, vb =>
        {
            vb.ToJson();
            vb.OwnsMany(d => d.SocialMedias, ppb =>
            {
                ppb.Property(pp => pp.Title)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH);
                ppb.Property(pp => pp.Url)
                    .IsRequired();
            });
            vb.OwnsMany(d => d.Requisites, rb =>
            {
                rb.Property(r => r.Description)
                    .HasMaxLength(Constants.MAX_LARGE_TEXT_LENGTH);
                rb.Property(r => r.Title)
                    .HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH);
            });

        });
    }
}