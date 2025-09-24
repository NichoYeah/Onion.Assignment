using Microsoft.EntityFrameworkCore;
using Onion.Assignment.Services.Greetings.Data.Models;

namespace Onion.Assignment.Services.Greetings.Data.DataSources;

public class GreetingDbContext : DbContext
{
    public GreetingDbContext(DbContextOptions<GreetingDbContext> options) : base(options)
    {
    }

    public DbSet<GreetingDataModel> Greetings { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<GreetingDataModel>(entity =>
        {
            entity.ToTable("Greetings");
            
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Id)
                .ValueGeneratedNever(); // We'll generate GUIDs in the domain
            
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
            
            entity.Property(e => e.Message)
                .IsRequired()
                .HasMaxLength(200);
            
            entity.Property(e => e.CreatedAt)
                .IsRequired();

            entity.HasIndex(e => e.Name)
                .HasDatabaseName("IX_Greetings_Name");
                
            entity.HasIndex(e => e.CreatedAt)
                .HasDatabaseName("IX_Greetings_CreatedAt");
        });
    }
}