using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Lab5.Models
{
    public class Rental
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid ClientId { get; set; }
        [Display(Name = "Клієнт")]
        public Client Client { get; set; }

        public Guid ProductId { get; set; }
        [Display(Name = "Продукт")]
        public Product Product { get; set; }

        [Display(Name = "Вартісь прокату")]
        public int RentalPrice { get; set; }

        [Display(Name = "Дата видачі")]
        public DateTime DateIssue { get; set; }

        [Display(Name = "Дата повернення")]
        public DateTime DateReturn { get; set; }

        public override string ToString()
        {
            return string.Join(" \\ ", new object[] { Id, ClientId, ProductId, RentalPrice, DateIssue, DateReturn });
        }
    }
}
