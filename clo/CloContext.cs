using Microsoft.EntityFrameworkCore;

namespace clo;

public partial class CloContext : DbContext
{
    public CloContext()
    {
    }

    public CloContext(DbContextOptions<CloContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Models.Employee> Employees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Models.Employee>(entity =>
        {
            entity.HasKey(e => e.Name).HasName("PRIMARY");

            entity.ToTable("Employee");

            entity.Property(e => e.Name).HasMaxLength(20);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Joined).HasColumnType("datetime(3)");
            entity.Property(e => e.Tel).HasMaxLength(20);
        });
    }
}