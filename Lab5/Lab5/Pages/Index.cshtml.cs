using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab5.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Lab5.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger, ApplicationContext context)
        {
            _context = context;
            _logger = logger;
        }

        public IEnumerable<ClientAndProducts> ClientAndProducts { get; set; }

        public void OnGet()
        {
            ClientAndProducts = _context.Rentals
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
        }
    }
}
