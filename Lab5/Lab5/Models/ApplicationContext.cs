using Lab5.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lab5
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Rental> Rentals { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rental>()
                .HasOne(rental => rental.Client)
                .WithMany(r => r.Rentals)
                .HasForeignKey(rental => rental.ClientId);

            modelBuilder.Entity<Rental>()
                .HasOne(rental => rental.Product)
                .WithMany(r => r.Rentals)
                .HasForeignKey(rental => rental.ProductId);
        }
    }
}
