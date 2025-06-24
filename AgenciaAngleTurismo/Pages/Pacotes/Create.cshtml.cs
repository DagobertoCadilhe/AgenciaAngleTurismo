using AgenciaTurismo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgenciaTurismo.Pages_Pacotes
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly AgenciaTurismo.Data.AgenciaTurismoDbContext _context;

        public CreateModel(AgenciaTurismo.Data.AgenciaTurismoDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["DestinoId"] = new SelectList(_context.Destinos, "Id", "Nome");
            return Page();
        }

        [BindProperty]
        public PacoteTuristico PacoteTuristico { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.PacotesTuristicos.Add(PacoteTuristico);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
