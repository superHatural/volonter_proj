using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VolunteerProg.Domain.Aggregates.PetManagement.Entities;
using VolunteerProg.Domain.Aggregates.PetManagement.ValueObjects;
using VolunteerProg.Domain.Shared;
using VolunteerProg.Domain.Shared.Ids;

namespace VolunteerProg.Infrastructure.Configurations;

public class PetConfiguration : IEntityTypeConfiguration<Pet>
{
    public void Configure(EntityTypeBuilder<Pet> builder)
    {
        builder.ToTable("pets");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id)
            .HasConversion(
                id => id.Value,
                value => PetId.Create(value));
        builder.ComplexProperty(p => p.Name, nb =>
        {
            nb.Property(p => p.Value)
                .IsRequired()
                .HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH)
                .HasColumnName("name");
        });

        builder.ComplexProperty(p => p.SpeciesDetails, sdb =>
        {
            sdb.Property(sib => sib.SpeciesId)
                .HasConversion(
                    id => id.Value,
                    value => SpeciesId.Create(value))
                .HasColumnName("species_id");

            sdb.Property(bib => bib.BreedId)
                .HasColumnName("breed_id");
        });

        builder.ComplexProperty(p => p.Description, db =>
        {
            db.Property(d => d.Value)
                .IsRequired()
                .HasMaxLength(Constants.MAX_LARGE_TEXT_LENGTH)
                .HasColumnName("description");
        });

        builder.ComplexProperty(v => v.PhoneNumber, pnb =>
        {
            pnb.Property(f => f.PhoneNumber)
                .IsRequired()
                .HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH)
                .HasColumnName("phone_number");
        });

        builder.ComplexProperty(p => p.Color, cb =>
        {
            cb.Property(c => c.Value)
                .IsRequired()
                .HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH)
                .HasColumnName("color");
        });

        builder.ComplexProperty(p => p.HealthInfo, hb =>
        {
            hb.Property(c => c.Value)
                .IsRequired()
                .HasMaxLength(Constants.MAX_LARGE_TEXT_LENGTH)
                .HasColumnName("health_info");
        });

        builder.Property(p => p.Weight)
            .HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH);

        builder.Property(p => p.Height)
            .HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH);
        builder.Property(p => p.IsCastrated);

        builder.Property(p => p.IsVaccinated);

        builder.ComplexProperty(p => p.BirthDate, bb =>
        {
            bb.Property(d => d.DateTime)
                .IsRequired()
                .HasColumnName("birth_date");
        });
        builder.ComplexProperty(p => p.DateOfCreate, dcb =>
        {
            dcb.Property(d => d.DateTime)
                .IsRequired()
                .HasColumnName("date_of_create");
        });

        builder.Property(p => p.Status)
            .HasConversion(
                status => status.ToString(),
                value => (PetStatus)Enum.Parse(typeof(PetStatus), value))
            .HasColumnName("status");

        builder.ComplexProperty(v => v.Address, ab =>
        {
            ab.Property(a => a.City)
                .IsRequired()
                .HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH)
                .HasColumnName("city");

            ab.Property(a => a.Country)
                .IsRequired()
                .HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH)
                .HasColumnName("country");

            ab.Property(a => a.PostalCode)
                .IsRequired()
                .HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH)
                .HasColumnName("postal_code");

            ab.Property(a => a.Street)
                .IsRequired()
                .HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH)
                .HasColumnName("street");
        });
        builder.OwnsOne(p => p.PetPhotoDetails, pb =>
        {
            pb.ToJson("pet_photo_details");
            pb.OwnsMany(d => d.PetPhotos, ppb =>
            {
                ppb.Property(pp => pp.Path)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH);

                ppb.Property(pp => pp.IsMainImage)
                    .IsRequired();
            });
        });

        builder.OwnsOne(p => p.RequisiteDetails, pb =>
        {
            pb.ToJson("requisite_details");
            pb.OwnsMany(d => d.Requisites, rb =>
            {
                rb.Property(r => r.Description)
                    .HasMaxLength(Constants.MAX_LARGE_TEXT_LENGTH);

                rb.Property(r => r.Title)
                    .HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH);
            });
        });
        builder.Property<bool>("_deleted")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("is_deleted");
    }
}