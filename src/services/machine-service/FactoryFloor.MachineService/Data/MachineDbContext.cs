using FactoryFloor.MachineService.Entities;
using Microsoft.EntityFrameworkCore;

namespace FactoryFloor.MachineService.Data
{
    public class MachineDbContext : DbContext
    {
        public MachineDbContext(DbContextOptions<MachineDbContext> options)
            : base(options)
        {

        }

        public DbSet<Machine> Machines { get; set; } = null!;
        public DbSet<MaintenanceLog> MaintenanceLogs { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Machine>(entity =>
            {
                entity.HasKey(m => m.Id);
                entity.HasIndex(m => new { m.TenantId, m.SerialNumber }).IsUnique();
                entity.Property(m => m.Name).IsRequired().HasMaxLength(200);
                entity.Property(m => m.SerialNumber).IsRequired().HasMaxLength(100);
                entity.Property(m => m.Model).HasMaxLength(100);
                entity.Property(m => m.Manufacturer).HasMaxLength(100);
                entity.Property(m => m.Location).HasMaxLength(200);
                entity.Property(m => m.Status).HasConversion<string>();
                entity.HasMany(m => m.MaintenanceLogs)
                    .WithOne(ml => ml.Machine)
                    .HasForeignKey(ml => ml.MachineId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<MaintenanceLog>(entity =>
            {
                entity.HasKey(ml => ml.Id);
                entity.Property(ml => ml.Description).IsRequired().HasMaxLength(1000);
                entity.Property(ml => ml.Type).HasConversion<string>();
                entity.Property(ml => ml.PerformedBy).HasMaxLength(100);
                entity.HasOne(ml => ml.Machine)
                      .WithMany(m => m.MaintenanceLogs)
                      .HasForeignKey(ml => ml.MachineId);
            });
        }
    }
}
