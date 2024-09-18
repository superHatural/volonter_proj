using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VolunteerProg.Domain.PetManagement.AggregateRoot;
using VolunteerProg.Domain.PetManagement.ValueObjects.Ids;
using VolunteerProg.Domain.Shared;

namespace VolunteerProg.Infrastructure.Configurations;

public class SpeciesConfiguration : IEntityTypeConfiguration<Species>
{
    public void Configure(EntityTypeBuilder<Species> builder)
    {
        builder.ToTable("species");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id)
            .HasConversion(
                id => id.Value,
                value => SpeciesId.Create(value));
        builder.ComplexProperty(s => s.Title, tb =>
        {
            tb.Property(vb => vb.Value)
                .IsRequired()
                .HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH)
                .HasColumnName("title");
        });
        builder.HasMany(s => s.Breeds)
            .WithOne()
            .HasForeignKey("species_id")
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(s => s.Breeds).AutoInclude();
    }
}