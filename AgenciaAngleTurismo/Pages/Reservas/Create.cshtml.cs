using AgenciaAngleTurismo.Services;
using AgenciaTurismo.Data;
using AgenciaTurismo.Models;
using AgenciaTurismo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AgenciaTurismo.Pages.Reservas
{
    public class CreateModel : PageModel
    {
        private readonly AgenciaTurismoDbContext _context;
        private readonly DiscountService _discountService;
        private readonly ReservationService _reservationService;

        public CreateModel(AgenciaTurismoDbContext context,
                          DiscountService discountService,
                          ReservationService reservationService)
        {
            _context = context;
            _discountService = discountService;
            _reservationService = reservationService;
        }

        [BindProperty]
        public Reserva Reserva { get; set; }

        public SelectList Clientes { get; set; }
        public SelectList Pacotes { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Clientes = new SelectList(await _context.Clientes.ToListAsync(), "Id", "Nome");
            Pacotes = new SelectList(await _context.PacotesTuristicos
                .Include(p => p.Destino)
                .ToListAsync(), "Id", "TituloCompleto");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadSelectListsAsync();
                return Page();
            }

            var cliente = await _context.Clientes.FindAsync(Reserva.ClienteId);
            var pacote = await _context.PacotesTuristicos.FindAsync(Reserva.PacoteTuristicoId);

            if (cliente == null || pacote == null)
            {
                ModelState.AddModelError("", "Cliente ou Pacote não encontrado");
                await LoadSelectListsAsync();
                return Page();
            }

            // Aplicar desconto
            CalculateDelegate discountDelegate = DiscountService.ApplySeniorDiscount;
            decimal precoComDesconto = discountDelegate(cliente.DataNascimento, pacote.Preco);

            // Calcular valor total
            Reserva.ValorTotal = _reservationService.CalculateTotal(
                (viajantes, preco) => viajantes * preco,
                Reserva.QuantidadeViajantes,
                precoComDesconto
            );

            Reserva.DataReserva = DateTime.Now;

            _context.Reservas.Add(Reserva);
            await _context.SaveChangesAsync();

            _reservationService.ProcessReserva(Reserva);

            return RedirectToPage("./Index");
        }

        private async Task LoadSelectListsAsync()
        {
            Clientes = new SelectList(await _context.Clientes.ToListAsync(), "Id", "Nome");
            Pacotes = new SelectList(await _context.PacotesTuristicos
                .Include(p => p.Destino)
                .ToListAsync(), "Id", "TituloCompleto");
        }
    }
}