using Lab2.Interfaces;
using Lab2.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lab2
{
    static class Program
    {
        private static IEnumerable<Client> _newClients = new List<Client>();
        private static IEnumerable<Product> _newProducts = new List<Product>();
        private static List<Rental> _newRentals = new List<Rental>();

        private static List<IReadableFromString> ReadClientsAndProductsFromFile(Type type, string fileName)
        {
            List<IReadableFromString> readables = new List<IReadableFromString>();

            using (var context = new ApplicationContext())
            {
                string[] rows = File.ReadAllLines(fileName);

                foreach (var row in rows)
                {
                    var obj = Activator.CreateInstance(type) as IReadableFromString;

                    obj.ReadFromStringArray(row.Split(';'));
                    readables.Add(obj);
                }

                context.AddRange(readables);
                context.SaveChanges();
            }

            return readables;
        }


        private static void ReadRentalsFromFile(string fileName)
        {
            using (var context = new ApplicationContext())
            {
                string[] rows = File.ReadAllLines(fileName);

                foreach (var row in rows)
                {
                    string[] words = row.Split(';');

                    Rental rental = new Rental();
                    rental.ClientId = _newClients.ElementAt(int.Parse(words[1]) - 1).Id;
                    rental.ProductId = _newProducts.ElementAt(int.Parse(words[2]) - 1).Id;
                    rental.RentalPrice = int.Parse(words[3]);
                    rental.DateIssue = DateTime.Parse(words[4]);
                    rental.DateReturn = DateTime.Parse(words[5]);

                    _newRentals.Add(rental);
                }

                context.AddRange(_newRentals);
                context.SaveChanges();
            }
        }


        private static void DeleteData()
        {
            using (var context = new ApplicationContext())
            {
                context.Rentals.RemoveRange(context.Rentals);
                context.Clients.RemoveRange(context.Clients);
                context.Products.RemoveRange(context.Products);
                context.SaveChanges();
            }
        }


        public static void PrintUnionTable()
        {
            Console.WriteLine("\n\n===== Union Tables =====");
            using (var context = new ApplicationContext())
            {
                var data = context.Rentals
                     .Join(context.Clients, rentals => rentals.ClientId, clients => clients.Id, (o, c) => new { o.ProductId, ClientId = c.Id, c.Name, c.Surname })
                     .Join(context.Products, c => c.ProductId, o => o.Id, (c, o) => new { Product = o.Name, c.ClientId, c.Name, c.Surname })
                     .GroupBy(t => new { t.Name, t.Surname, t.ClientId })
                     .Where(g => g.Count() >= 2)
                     .Select(e => new { e.Key, Count = e.Count(), Cities = e.Select(e => e.Product).ToList() })
                     .ToDictionary(e => e.Key, e => e.Cities);


                /*var data = context.Rentals
                    .Join(context.Clients, rentals => rentals.ClientId, clients => clients.Id, (o, c) => new { o.ProductId, ClientId = c.Id, c.Name, c.Surname })
                    .Join(context.Products, c => c.ProductId, o => o.Id, (c, o) => new { Product = o.Name, c.ClientId, c.Name, c.Surname }).ToList()
                    .GroupBy(t => new { t.Name, t.Surname, t.ClientId })
                    .Where(g => g.Count() >= 2);*/



                foreach (var el in data)
                {
                    Console.WriteLine($"'{el.Key.Name} {el.Key.Surname}' " +
                        $"Product: [{string.Join(", ", el.Value)}], " +
                        $"Products Count: {el.Value}");

                    /*string[] products = el.Select(q => q.Product).ToArray();

                    Console.WriteLine($"{el.Key.Name}, {el.Key.Surname}, Product: {String.Join(',', products)} " +
                        $"Products Count: {el.Count()}");*/
                }
            }
        }

        public static void InsertDataFromFileToDB()
        {
            _newClients = Enumerable.Cast<Client>(ReadClientsAndProductsFromFile(typeof(Client), "clients.txt"));
            _newProducts = Enumerable.Cast<Product>(ReadClientsAndProductsFromFile(typeof(Product), "products.txt"));
            ReadRentalsFromFile("rentals.txt");
        }

        private static void ShowData()
        {
            using (var context = new ApplicationContext())
            {
                PrintTable("Clients", context.Clients.ToList());
                PrintTable("Product", context.Products.ToList());
                PrintTable("Rentals", context.Rentals.ToList());
            }
        }

        private static void PrintTable(string table, IEnumerable<IModel> query)
        {
            Console.WriteLine($"\n\n===== { table } =====");
            using (var context = new ApplicationContext())
            {
                foreach (var element in query)
                {
                    Console.WriteLine(element);
                }
            }
        }



        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            DeleteData();
            InsertDataFromFileToDB();
            ShowData();
            PrintUnionTable();
        }
    }
}
