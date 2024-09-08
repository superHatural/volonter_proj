using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using VolunterProg.Domain.Shared;
using VolunterProg.Domain.Voluunters;

namespace VolunterProg.Infrastructure.Configurations;

public class VoluunterConfiguration: IEntityTypeConfiguration<Voluunter>
{
    public void Configure(EntityTypeBuilder<Voluunter> builder)
    {
        builder.ToTable("voluunters");
        builder.HasKey(v => v.Id);
        builder.Property(v => v.Id)
            .HasConversion(
                id => id.Value,
                value => VoluunterId.Create(value));
        builder.ComplexProperty(v => v.FullName, fnb =>
        {
            fnb.Property(fb => fb.FirstName)
                .IsRequired()
                .HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH)
                .HasColumnName("first_name");
            fnb.Property(lb => lb.LastName)
                .IsRequired()
                .HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH)
                .HasColumnName("last_name");
        });
        builder.ComplexProperty(v => v.Email, eb =>
        {
            eb.Property(f => f.EmailAddress)
                .IsRequired()
                .HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH)
                .HasColumnName("email");
        });
        builder.ComplexProperty(v => v.PhoneNumber, pnb =>
        {
            pnb.Property(f => f.PhoneNumber)
                .IsRequired()
                .HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH)
                .HasColumnName("phone_number");
        });

        builder.ComplexProperty(v => v.Description, db =>
        {
            db.Property(d => d.Value)
                .IsRequired()
                .HasMaxLength(Constants.MAX_LARGE_TEXT_LENGTH)
                .HasColumnName("description");
        });
        
        builder.Property(v => v.Experience)
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