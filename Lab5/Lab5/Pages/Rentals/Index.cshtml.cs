using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Lab5;
using Lab5.Models;

namespace Lab5.Pages.Rentals
{
    public class IndexModel : PageModel
    {
        private readonly Lab5.ApplicationContext _context;

        public IndexModel(Lab5.ApplicationContext context)
        {
            _context = context;
        }

        public IList<Rental> Rental { get;set; }

        public async Task OnGetAsync()
        {
            Rental = await _context.Rentals
                .Include(r => r.Client)
                .Include(r => r.Product).ToListAsync();
        }
    }
}
