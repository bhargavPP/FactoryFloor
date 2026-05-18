using FactoryFloor.IdentityService.Entities;
using Microsoft.EntityFrameworkCore;
namespace FactoryFloor.IdentityService.Data;

public class IdentityDbContext : DbContext
{
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options):base(options)
    {
        
    }
    public DbSet<User> Users { get; set; }
    public DbSet<Tenant> Tenants { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Tenant>(e =>
        {
            e.HasKey(t => t.Id);
            e.HasIndex(t => t.Slug).IsUnique();
            e.Property(t => t.Name).HasMaxLength(100).IsRequired();
            e.Property(t => t.Slug).HasMaxLength(100).IsRequired();
        });

        builder.Entity<User>(e =>
        {
            e.HasKey(t => t.Id);
            e.HasIndex(u => new { u.TenantId, u.Email }).IsUnique();
            e.Property(u => u.Email).HasMaxLength(200).IsRequired();
            e.Property(u => u.Role).HasMaxLength(50).IsRequired();
            e.HasOne(u => u.Tenant)
             .WithMany(t => t.Users)
             .HasForeignKey(u => u.TenantId);

        });
        base.OnModelCreating(builder);
    }
}