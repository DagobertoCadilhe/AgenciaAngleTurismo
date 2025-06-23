using AgenciaTurismo.Data;
using AgenciaTurismo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AgenciaTurismo.Pages.Reservas
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly AgenciaTurismoDbContext _context;

        public IndexModel(AgenciaTurismoDbContext context)
        {
            _context = context;
        }

        public IList<Reserva> Reservas { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Reservas = await _context.Reservas
                .Include(r => r.Cliente)
                .Include(r => r.PacoteTuristico)
                .ThenInclude(p => p.Destino)
                .ToListAsync();
        }
    }
}