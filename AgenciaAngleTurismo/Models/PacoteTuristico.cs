using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgenciaTurismo.Models
{
    public class PacoteTuristico
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O título é obrigatório")]
        [MinLength(5, ErrorMessage = "O título deve ter no mínimo 5 caracteres")]
        public string Titulo { get; set; }

        public string TituloCompleto => $"{Titulo} - {Destino?.Nome} ({DataInicio:dd/MM/yyyy})";

        [Required(ErrorMessage = "A data de início é obrigatória")]
        [Display(Name = "Data de Início")]
        [DataType(DataType.Date)]
        [FutureDate(ErrorMessage = "A data de início deve ser futura")]
        public DateTime DataInicio { get; set; }

        [Required(ErrorMessage = "O preço é obrigatório")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser maior que zero")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Preco { get; set; }

        [Display(Name = "Destino")]
        public int DestinoId { get; set; }
        public Destino? Destino { get; set; }

        public ICollection<Reserva>? Reservas { get; set; }

        public bool IsDeleted { get; set; } = false;
    }

    public class FutureDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return value is DateTime date && date > DateTime.Now;
        }
    }
}