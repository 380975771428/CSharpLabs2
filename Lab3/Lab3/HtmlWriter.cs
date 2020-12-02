using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Lab3
{
    public class HtmlWriter
    {
        public static async Task ShowPage(HttpContext httpContext, string pageName, string[] columns, List<string[]> rows)
        {
            await httpContext.Response.WriteAsync(File.ReadAllText(@".\wwwroot\templates\header.html"));

            string tableData =
                $"<h1>{pageName}</h1>" +
                "<table class='table table-striped'>" +
                "<tr>";

            foreach (var column in columns)
            {
                tableData += $"<th>{column}</th>";
            }

            await httpContext.Response.WriteAsync(tableData + "</tr>");

            foreach (var row in rows)
            {
                string htmlRow = "<tr>";

                foreach (var cell in row)
                {
                    htmlRow += $"<td>{cell}</td>";
                }

                await httpContext.Response.WriteAsync(htmlRow + "</tr>");
            }

            await httpContext.Response.WriteAsync("</table>");
            await httpContext.Response.WriteAsync(File.ReadAllText(@".\wwwroot\templates\footer.html"));
        }



        public static async Task WriteClientsPage(HttpContext httpContext)
        {
            string pageName = "Список клієнтів";
            List<string[]> clientsList = new List<string[]>();
            string[] columns = new[] { "Прізвище", "Ім'я", "По батькові", "Адреса", "Номер телефону", "Номер паспорта" };

            using (var context = new ApplicationContext())
            {
                foreach (var el in context.Clients.ToList())
                {
                    string[] client = new string[] { el.Surname, el.Name, el.MiddleName, el.Address, el.PhoneNumber, el.Passport };

                    clientsList.Add(client);
                }
            }

            await ShowPage(httpContext, pageName, columns, clientsList);
        }


        public static async Task WriteProductsPage(HttpContext httpContext)
        {
            string pageName = "Список товарів";
            List<string[]> productsList = new List<string[]>();
            string[] columns = new[] { "Назва", "Опис", "Ціна" };

            using (var context = new ApplicationContext())
            {
                foreach (var el in context.Products.ToList())
                {
                    string[] product = new string[] { el.Name, el.Description, el.Price.ToString() };

                    productsList.Add(product);
                }
            }

            await ShowPage(httpContext, pageName, columns, productsList);
        }


        public static async Task WriteMainPage(HttpContext httpContext)
        {
            string pageName = "Головна сторінка";
            List<string[]> unionList = new List<string[]>();
            string[] columns = new[] { "Прізвище", "Ім'я", "Товари" };

            using (var context = new ApplicationContext())
            {
                var query = context.Rentals
                    .Join(context.Clients, rentals => rentals.ClientId, clients => clients.Id, (o, c) => new { o.ProductId, ClientId = c.Id, c.Name, c.Surname })
                    .Join(context.Products, c => c.ProductId, o => o.Id, (c, o) => new { Product = o.Name, c.ClientId, c.Name, c.Surname }).ToList()
                    .GroupBy(t => new { t.Name, t.Surname, t.ClientId })
                    .Where(g => g.Count() >= 2);

                foreach (var el in query)
                {
                    string[] products = el.Select(q => q.Product).ToArray();
                    string[] union = new string[] { el.Key.Surname, el.Key.Name, string.Join(", ", products) };

                    unionList.Add(union);
                }

                await ShowPage(httpContext, pageName, columns, unionList);
            }
        }


    }
}
