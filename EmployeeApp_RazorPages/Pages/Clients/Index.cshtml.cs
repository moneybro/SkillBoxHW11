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
    public class IndexModel : PageModel
    {
        private readonly ClassLibrary.Contexts.SqlDbContext _context;

        public IndexModel(ClassLibrary.Contexts.SqlDbContext context)
        {
            _context = context;
        }

        public IList<Client> Clients { get;set; } = default!;

        //public async Task OnGetAsync()
        //{
        //    if (_context.Clients != null)
        //    {
        //        Clients = await _context.Clients.Where(c => c.Status == "active").ToListAsync();
        //    }
        //}

        public async Task<IActionResult> OnGetAsync(string status)
        {
            if (_context.Clients != null)
            {
                if (status == "deleted")
                {
                    Clients = await _context.Clients.Where(c => c.Status == "deleted").ToListAsync();
                    return Page();
                }
                if (status == "all")
                {
                    Clients = await _context.Clients.ToListAsync();
                    return Page();
                }
                if (status == "" || status == null)
                {
                    Clients = await _context.Clients.Where(c => c.Status == "active").ToListAsync();
                    return Page();
                }
                Clients = await _context.Clients.Where(c => c.Status == "active").ToListAsync();
                return Page();
            }
            else
            {
                return NotFound();
            }
        }

        //public async Task<IActionResult> OnGetAsync(long? id)
        //{
        //    if (id == null || _context.Clients == null)
        //    {
        //        return NotFound();
        //    }

        //    var client = await _context.Clients.FirstOrDefaultAsync(m => m.ID == id);
        //    if (client == null)
        //    {
        //        return NotFound();
        //    }
        //    Client = client;
        //    return Page();
        //}
    }
}
