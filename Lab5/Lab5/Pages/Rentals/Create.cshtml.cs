﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Lab5;
using Lab5.Models;

namespace Lab5.Pages.Rentals
{
    public class CreateModel : PageModel
    {
        private readonly Lab5.ApplicationContext _context;

        public CreateModel(Lab5.ApplicationContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "FullName");
        ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public Rental Rental { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Rentals.Add(Rental);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
