using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Lab4.Models;

namespace Lab4.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var data = _context.Rentals
                .Join(_context.Clients, rentals => rentals.ClientId, clients => clients.Id, (o, c) => new { o.ProductId, ClientId = c.Id, c.Name, c.Surname })
                .Join(_context.Products, c => c.ProductId, o => o.Id, (c, o) => new { Product = o.Name, c.ClientId, c.Name, c.Surname }).ToList()
                .GroupBy(t => new { t.Name, t.Surname, t.ClientId })
                .Where(g => g.Count() >= 2)
                .Select(r => new ClientAndProducts
                {
                    FirstName = r.Key.Name,
                    LastName = r.Key.Surname,
                    Products = string.Join(", ", r.Select(q => q.Product))
                });

            return View(data);
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
