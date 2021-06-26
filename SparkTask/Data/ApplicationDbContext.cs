using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SparkTask.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SparkTask.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Product> Products{ get; set; }
        public DbSet<Category> Categories{ get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Product>(entity =>
            {
                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_product_Category")
                    .OnDelete(DeleteBehavior.ClientCascade);
            });

            builder.Entity<Category>()
    .HasMany<Product>(g => g.Products)
    .WithOne(s => s.Category)
    .HasForeignKey(s => s.CategoryId)
    .OnDelete(DeleteBehavior.Cascade);

        }

    }
}
