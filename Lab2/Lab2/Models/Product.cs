using Lab2.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Lab2.Models
{
    class Product : IModel, IReadableFromString
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }

        [InverseProperty("Product")]
        public virtual ICollection<Rental> Rentals { get; set; }

        public void ReadFromStringArray(string[] values)
        {
            Name = values[1];
            Description = values[2];
            Price = int.Parse(values[3]);
        }

        public override string ToString()
        {
            return string.Join(" \\ ", new object[] { Id, Name, Description, Price });
        }
    }
}
