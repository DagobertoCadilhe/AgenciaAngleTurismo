using AgenciaTurismo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgenciaTurismo.Pages.Reservas
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly AgenciaTurismo.Data.AgenciaTurismoDbContext _context;

        public EditModel(AgenciaTurismo.Data.AgenciaTurismoDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Reserva Reserva { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reservas.FirstOrDefaultAsync(m => m.Id == id);
            if (reserva == null)
            {
                return NotFound();
            }
            Reserva = reserva;

            // Carrega os dropdowns com os clientes e pacotes disponíveis
            ViewData["ClienteId"] = new SelectList(_context.Clientes.Where(c => !c.IsDeleted), "Id", "Nome");
            ViewData["PacoteTuristicoId"] = new SelectList(_context.PacotesTuristicos.Where(p => !p.IsDeleted), "Id", "Titulo");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Valida o estado do modelo antes de tentar salvar
            if (!ModelState.IsValid)
            {
                // Se o modelo for inválido, recarrega os dropdowns e retorna para a página
                ViewData["ClienteId"] = new SelectList(_context.Clientes.Where(c => !c.IsDeleted), "Id", "Nome");
                ViewData["PacoteTuristicoId"] = new SelectList(_context.PacotesTuristicos.Where(p => !p.IsDeleted), "Id", "Titulo");
                return Page();
            }

            // Anexa a entidade e define seu estado como modificado
            _context.Attach(Reserva).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservaExists(Reserva.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ReservaExists(int id)
        {
            return _context.Reservas.Any(e => e.Id == id);
        }
    }
}