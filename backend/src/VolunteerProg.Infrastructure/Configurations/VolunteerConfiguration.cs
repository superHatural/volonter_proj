using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VolunteerProg.Domain.Shared;
using VolunteerProg.Domain.Volunteers;

namespace VolunteerProg.Infrastructure.Configurations;

public class VolunteerConfiguration: IEntityTypeConfiguration<Volunteer>
{
    public void Configure(EntityTypeBuilder<Volunteer> builder)
    {
        builder.ToTable("volunteers");
        builder.HasKey(v => v.Id);
        builder.Property(v => v.Id)
            .HasConversion(
                id => id.Value,
                value => VolunteerId.Create(value));
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
            .HasForeignKey("volunteer_id");

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