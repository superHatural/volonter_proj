﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using VolunteerProg.Domain.Aggregates.PetManagement.AggregateRoot;
using VolunteerProg.Domain.Aggregates.SpeciesManagement.AggregateRoot;

namespace VolunteerProg.Infrastructure;

public class ApplicationDbContext(IConfiguration configuration) : DbContext
{
    public DbSet<Volunteer> Voluunters => Set<Volunteer>();
    public DbSet<Species> Species => Set<Species>();
    const string DATABASE = "Database";

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(configuration.GetConnectionString(DATABASE));
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => { builder.AddConsole(); });
}