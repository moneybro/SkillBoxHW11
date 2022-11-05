using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ClassLibrary.Classes;
using ClassLibrary.Contexts;

namespace EmployeeApp_RazorPages.Pages.Clients
{
    public class DetailsModel : PageModel
    {
        private readonly ClassLibrary.Contexts.SqlDbContext _context;

        public DetailsModel(ClassLibrary.Contexts.SqlDbContext context)
        {
            _context = context;
        }

      public Client Client { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null || _context.Clients == null)
            {
                return NotFound();
            }

            var client = await _context.Clients.FirstOrDefaultAsync(m => m.ID == id);
            if (client == null)
            {
                return NotFound();
            }
            else 
            {
                Client = client;
            }
            return Page();
        }
    }
}
