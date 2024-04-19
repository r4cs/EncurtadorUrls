using cp2.Entities;
using cp2.Services;
using cp2.Entities;
using cp2.Services;
using Microsoft.EntityFrameworkCore;

namespace cp2;

public class EncurtadorDbContext : DbContext
{
    public EncurtadorDbContext(DbContextOptions<EncurtadorDbContext> options) : base(options)
    {
    }
    
    public DbSet<EncurtadorUrl> EncurtadorUrls { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<EncurtadorUrl>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.UrlLonga)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(e => e.UrlCurta)
                .IsRequired();

            entity.Property(e => e.Codigo)
                .IsRequired()
                .HasMaxLength(EncurtadorUrlService.QtdeCharsEncurtadorNoLink);

            entity.Property(e => e.CriadoEm)
                .IsRequired();

            entity.HasIndex(e => e.Codigo).IsUnique();

            entity.ToTable("EncurtadorUrls");
        });
    }
}