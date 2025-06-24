using AgenciaTurismo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgenciaTurismo.Pages_Pacotes
{
    public class IndexModel : PageModel
    {
        private readonly AgenciaTurismo.Data.AgenciaTurismoDbContext _context;

        public IndexModel(AgenciaTurismo.Data.AgenciaTurismoDbContext context)
        {
            _context = context;
        }

        public IList<PacoteTuristico> PacoteTuristico { get;set; } = default!;

        public async Task OnGetAsync()
        {
            PacoteTuristico = await _context.PacotesTuristicos
                .Include(p => p.Destino).ToListAsync();
        }
    }
}
