using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Lab4.Models
{
    public class Client
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Display(Name = "Прізвище")]
        public string Surname { get; set; }
        [Display(Name = "Ім'я")]
        public string Name { get; set; }
        [Display(Name = "По батькові")]
        public string MiddleName { get; set; }
        [Display(Name = "Адреса")]
        public string Address { get; set; }
        [Display(Name = "Номер телефону")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Номер паспорту")]
        public string Passport { get; set; }


        public string FullName => $"{Name} {MiddleName} {Surname}";

        public virtual ICollection<Rental> Rentals { get; set; }

        public void ReadFromStringArray(string[] values)
        {
            Surname = values[1];
            Name = values[2];
            MiddleName = values[3];
            Address = values[4];
            PhoneNumber = values[5];
            Passport = values[6];
        }

        public override string ToString()
        {
            return string.Join(" \\ ", new object[] { Id, Name, Surname, MiddleName, Address, PhoneNumber, Passport });
        }

    }
}
