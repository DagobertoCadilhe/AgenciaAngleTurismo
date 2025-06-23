using AgenciaTurismo.Models;

namespace AgenciaAngleTurismo.Services
{
    public class ReservationService
    {
        public event Action<string> CapacityReached;

        private readonly Action<Reserva> _logReservaActions;

        public ReservationService()
        {
            _logReservaActions = LogReservaBasica;
            _logReservaActions += LogReservaValor;
            _logReservaActions += LogReservaCliente;
        }

        private void LogReservaBasica(Reserva reserva)
        {
            Console.WriteLine($"Nova reserva criada: ID {reserva.Id}");
        }

        private void LogReservaValor(Reserva reserva)
        {
            Console.WriteLine($"Valor total da reserva: {reserva.ValorTotal:C}");
        }

        private void LogReservaCliente(Reserva reserva)
        {
            Console.WriteLine($"Cliente da reserva: {reserva.Cliente.Nome}");
        }

        public void ProcessReserva(Reserva reserva)
        {
            // Verificar limite de viajantes
            if (reserva.QuantidadeViajantes > 10)
            {
                CapacityReached?.Invoke("Oferecer ao cliente um serviço de transfer.");
            }

            // Logs multicast
            _logReservaActions(reserva);
        }

        public decimal CalculateTotal(Func<int, decimal, decimal> calculator,
                                    int viajantes, decimal preco)
        {
            return calculator(viajantes, preco);
        }
    }
}
