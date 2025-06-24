using System.ComponentModel.DataAnnotations;

namespace AgenciaTurismo.Models
{
    public class Destino
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do destino é obrigatório")]
        [MinLength(5, ErrorMessage = "O nome deve ter no mínimo 5 caracteres")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O país é obrigatório")]
        [MinLength(5, ErrorMessage = "O país deve ter no mínimo 5 caracteres")]
        public string Pais { get; set; }

        public ICollection<PacoteTuristico>? PacotesTuristicos { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}