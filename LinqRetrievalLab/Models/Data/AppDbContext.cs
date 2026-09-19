using Microsoft.EntityFrameworkCore;
using LinqRetrievalLab.Models.Domain;

namespace LinqRetrievalLab.Data
{
    public class LinqRetrievalLabContext : DbContext
    {
        public LinqRetrievalLabContext(DbContextOptions<LinqRetrievalLabContext> options)
            : base(options) { }

        public DbSet<Category> Category { get; set; } = default!;
        public DbSet<Product> Product { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);
        }

    }
}