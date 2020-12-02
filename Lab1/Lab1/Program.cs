using MySql.Data.MySqlClient;
using System;
using System.IO;
using System.Linq;

namespace Lab1
{
    static class Program
    {
        private const string Host = "localhost";
        private const string Database = "rental_product";
        private const string User = "root";
        private const string Password = "";


        private readonly static string[] Tables = { "clients", "products", "rentals" };
        private static MySqlConnection _connection;

        private static void MakeConnection()
        {
            _connection = new MySqlConnection($"Database={Database};Datasource={Host};User={User};Password={Password}");
            _connection.Open();
        }

        private static void InsertData(string table)
        {
            string[] rows = File.ReadAllLines($"{table}.txt");
            foreach (var row in rows)
            {
                var command = _connection.CreateCommand();
                command.CommandText = $"INSERT INTO {table} VALUES({row.Replace(';', ',')})";
                command.ExecuteNonQuery();
            }

        }

        private static void DeleteData()
        {
            foreach (var table in Tables.Reverse())
            {
                var command = _connection.CreateCommand();
                command.CommandText = $"DELETE FROM {table}";
                command.ExecuteNonQuery();
            }
        }


        private static void PrintTable(string table)
        {
            var command = _connection.CreateCommand();
            command.CommandText = $"SELECT * FROM {table}";
            var reader = command.ExecuteReader();

            Console.WriteLine($"---------- Table {table} ----------");
            while (reader.Read())
            {
                object[] values = new object[reader.FieldCount];
                reader.GetValues(values);

                string str = values.Aggregate((v1, v2) => $"{v1} | {v2}").ToString();
                Console.WriteLine(str);
            }
            Console.WriteLine("\n\n");
            reader.Close();
        }


        private static void PrintDetails()
        {
            var command = _connection.CreateCommand();
            command.CommandText = $"SELECT * FROM rentals re JOIN clients cl ON re.client_id = cl.id";
            var reader = command.ExecuteReader();

            Console.WriteLine($"---------- Details ----------");
            while (reader.Read())
            {
                object[] values = new object[reader.FieldCount];
                reader.GetValues(values);

                string str = values.Aggregate((v1, v2) => $"{v1} | {v2}").ToString();
                Console.WriteLine(str);
            }
            Console.WriteLine("\n\n");
            reader.Close();
        }

        private static void PrintCount()
        {
            var command = _connection.CreateCommand();
            command.CommandText = $"SELECT * FROM rentals re JOIN clients cl ON re.client_id = cl.id GROUP BY re.client_id HAVING COUNT(*) >= 2";
            var reader = command.ExecuteReader();

            int rowCount = 0;
            while (reader.Read())
            {
                ++rowCount;
            }

            Console.WriteLine($"\n\nCount of clients that have two rentals: {rowCount}\n\n");
            Console.WriteLine("\n\n");
            reader.Close();
        }


        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            try
            {
                MakeConnection();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


            DeleteData();
            InsertData("clients");
            PrintTable("clients");
            InsertData("products");
            PrintTable("products");
            InsertData("rentals");
            PrintTable("rentals");
            PrintDetails();
            PrintCount();
        }
    }
}
