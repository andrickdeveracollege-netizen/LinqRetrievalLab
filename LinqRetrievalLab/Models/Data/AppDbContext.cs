using Microsoft.EntityFrameworkCore;
using LinqRetrievalLab.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace LinqRetrievalLab.Models.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Category> Category { get; set; } = default!;
        public DbSet<Product> Product { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Product)
                .HasForeignKey(p => p.CategoryId);
        }
    }
}