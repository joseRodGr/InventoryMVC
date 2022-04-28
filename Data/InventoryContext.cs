using InventoryMVC.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryMVC.Data
{
    public class InventoryContext : IdentityDbContext<AppUser>
    {
        public InventoryContext(DbContextOptions<InventoryContext> options): base(options)
        {

        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<ProductSupplier> ProductSuppliers { get; set; }
        public DbSet<InventoryMovement> InventoryMovements { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Product>().Property(p => p.Price).HasColumnType("decimal(18,4)");

            builder.Entity<Product>()
                .HasOne(x => x.Category)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ProductSupplier>()
                .HasKey(x => new { x.ProductId, x.SupplierId });

            //foreach(var foreignKey in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            //{
            //    foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            //}


        }
    }
}
