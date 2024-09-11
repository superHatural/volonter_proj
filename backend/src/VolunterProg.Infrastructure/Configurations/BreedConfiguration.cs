using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VolunterProg.Domain.Shared;
using VolunterProg.Domain.Voluunters;

namespace VolunterProg.Infrastructure.Configurations;

public class BreedConfiguration: IEntityTypeConfiguration<Breed>
{
    public void Configure(EntityTypeBuilder<Breed> builder)
    {
        builder.ToTable("breeds");
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id)
            .HasConversion(
                id => id.Value,
                value => BreedId.Create(value));
        builder.ComplexProperty(b => b.Title, tb =>
        {
            tb.Property(vb => vb.Value)
                .IsRequired()
                .HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH)
                .HasColumnName("title");
        });
        
    }
}