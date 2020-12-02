using Lab3.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Lab3.Models
{
    class Client : IReadableFromString, IModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Surname { get; set; }
        public string Name { get; set; }
        public string MiddleName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Passport { get; set; }

        [InverseProperty("Client")]
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
