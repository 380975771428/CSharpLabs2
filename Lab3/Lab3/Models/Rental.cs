using Lab3.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Lab3.Models
{
    class Rental : IModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid ClientId { get; set; }
        [ForeignKey("ClientId")]
        public Client Client { get; set; }

        public Guid ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        public int RentalPrice { get; set; }
        public DateTime DateIssue { get; set; }
        public DateTime DateReturn { get; set; }

        public override string ToString()
        {
            return string.Join(" \\ ", new object[] { Id, ClientId, ProductId, RentalPrice, DateIssue, DateReturn });
        }
    }
}
