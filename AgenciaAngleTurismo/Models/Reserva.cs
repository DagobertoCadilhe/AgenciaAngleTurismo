using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgenciaTurismo.Models
{
    public class Reserva
    {
        public int Id { get; set; }

        [Display(Name = "Cliente")]
        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }

        [Display(Name = "Pacote Turístico")]
        public int PacoteTuristicoId { get; set; }
        public PacoteTuristico? PacoteTuristico { get; set; }

        [Required(ErrorMessage = "A quantidade de viajantes é obrigatória")]
        [Range(1, 100, ErrorMessage = "Quantidade deve ser entre 1 e 100")]
        [Display(Name = "Quantidade de Viajantes")]
        public int QuantidadeViajantes { get; set; }

        [Display(Name = "Valor Total")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorTotal { get; set; }

        [Display(Name = "Data da Reserva")]
        [DataType(DataType.Date)]
        public DateTime DataReserva { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}