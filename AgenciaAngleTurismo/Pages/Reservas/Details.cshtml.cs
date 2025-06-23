using AgenciaTurismo.Data;
using AgenciaTurismo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AgenciaTurismo.Pages.Reservas
{
    public class DetailsModel : PageModel
    {
        private readonly AgenciaTurismoDbContext _context;

        public DetailsModel(AgenciaTurismoDbContext context)
        {
            _context = context;
        }

        public Reserva Reserva { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Reserva = await _context.Reservas
                .Include(r => r.Cliente)
                .Include(r => r.PacoteTuristico)
                .ThenInclude(p => p.Destino)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Reserva == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}