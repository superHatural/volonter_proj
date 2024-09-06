using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VolunterProg.Domain.Shared;
using VolunterProg.Domain.Voluunters;

namespace VolunterProg.Infrastructure.Configurations;

public class PetConfiguration: IEntityTypeConfiguration<Pet>
{
    public void Configure(EntityTypeBuilder<Pet> builder)
    {
        builder.ToTable("pets");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id)
            .HasConversion(
                id => id.Value,
                value => PetId.Create(value));
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH);
        
        builder.Property(p => p.Breed)
            .IsRequired()
            .HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH);
        
        builder.Property(p => p.Description)
            .HasMaxLength(Constants.MAX_LARGE_TEXT_LENGTH);
        
        builder.Property(p => p.Species)
            .HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH);
        
        builder.Property(p => p.PhoneNumber)
            .HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH);
        
        builder.Property(p => p.Color)
            .HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH);
        
        builder.Property(p => p.HealthInfo)
            .HasMaxLength(Constants.MAX_LARGE_TEXT_LENGTH);
        
        builder.Property(p => p.Weight)
            .HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH);
        
        builder.Property(p => p.Height)
            .HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH);
        builder.Property(p => p.IsCastrated);
        
        builder.Property(p => p.IsVaccinated);
        
        builder.Property(p => p.BirthDate)
            .HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH);
        builder.Property(p => p.DateOfCreate)
            .HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH);
        
        builder.Property(p => p.Status)
            .HasConversion(
                status => status.ToString(),
                value => (PetStatus)Enum.Parse(typeof(PetStatus), value));
        builder.OwnsOne(v => v.Address, ab =>
        {
            ab.ToJson();
            ab.Property(a => a.City)
                .IsRequired();

            ab.Property(a => a.Country)
                .IsRequired();
        });
        builder.OwnsOne(p => p.Details, pb =>
        {
            pb.ToJson();
            pb.OwnsMany(d => d.PetPhotos, ppb =>
            {
                ppb.Property(pp => pp.Path)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH);
                ppb.Property(pp => pp.IsMainImage)
                    .IsRequired();
            });
            pb.OwnsMany(d => d.Requisites, rb =>
            {
                rb.Property(r => r.Description)
                    .HasMaxLength(Constants.MAX_LARGE_TEXT_LENGTH);
                rb.Property(r => r.Title)
                    .HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH);
            });

        });

            
            
        
    }
} 