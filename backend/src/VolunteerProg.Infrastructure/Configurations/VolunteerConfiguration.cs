using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VolunteerProg.Domain.Aggregates.PetManagement.AggregateRoot;
using VolunteerProg.Domain.Shared;
using VolunteerProg.Domain.Shared.Ids;

namespace VolunteerProg.Infrastructure.Configurations;

public class VolunteerConfiguration : IEntityTypeConfiguration<Volunteer>
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

        builder.Property(v => v.Experience);

        builder.HasMany(v => v.Pets)
            .WithOne()
            .HasForeignKey("volunteer_id")
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(v => v.Pets).AutoInclude();

        builder.OwnsOne(v => v.SocMedDetails, vb =>
        {
            vb.ToJson("social_media_details");
            vb.OwnsMany(d => d.Values, ppb =>
            {
                ppb.Property(pp => pp.Title)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH);

                ppb.Property(pp => pp.Url)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH);
            });
        });

        builder.OwnsOne(v => v.ReqDetails, rb =>
        {
            rb.ToJson("requisite_details");
            rb.OwnsMany(d => d.Values, rqb =>
            {
                rqb.Property(r => r.Description)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_LARGE_TEXT_LENGTH);

                rqb.Property(r => r.Title)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH);
            });
        });
        builder.Property<bool>("_deleted")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("is_deleted");
    }
}