using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Lab4.Models
{
    public class Product
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Display(Name = "Назва")]
        public string Name { get; set; }
        [Display(Name = "Опис")]
        public string Description { get; set; }
        [Display(Name = "Ціна")]
        public int Price { get; set; }
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
