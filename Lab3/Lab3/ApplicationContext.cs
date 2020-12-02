using Lab3.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lab3
{
    class ApplicationContext : DbContext
    {
        private const string Host = "localhost";
        private const string Db = "rental_product_v2";
        private const string User = "root";
        private const string Password = "";

        public DbSet<Client> Clients { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Rental> Rentals { get; set; }

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql($"Database={Db};Datasource={Host};User={User};Password={Password}");
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
