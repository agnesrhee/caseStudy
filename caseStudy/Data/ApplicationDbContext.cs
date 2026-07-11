using caseStudy.Models;
using Microsoft.EntityFrameworkCore;

namespace caseStudy.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Sale> Sales => Set<Sale>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sale>(entity =>
            {
                entity.Property(sale => sale.Username)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(sale => sale.ProductName)
                    .HasMaxLength(200)
                    .IsRequired();

                entity.Property(sale => sale.ProductSku)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(sale => sale.ProductCategory)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(sale => sale.Price)
                    .HasColumnType("decimal(18,2)");

                entity.Property(sale => sale.Quantity)
                    .HasColumnType("decimal(18,2)");
            });
        }
    }
}
