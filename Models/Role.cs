using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agenda.Models
{
    [Table(nameof(Role))]
    public class Role : Entity
    {
        [Required(ErrorMessage = "Campo obrigatório.")]
        [MinLength(3, ErrorMessage = "Este campo deve conter entre 3 e 500 caracteres.")]
        [MaxLength(500, ErrorMessage = "Este campo deve conter entre 3 e 500 caracteres.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Campo obrigatório.")]
        [MinLength(3, ErrorMessage = "Este campo deve conter entre 3 e 50 caracteres.")]
        [MaxLength(50, ErrorMessage = "Este campo deve conter entre 3 e 50 caracteres.")]
        public string Title { get; set; }

        public ICollection<User> Users { get; set; }
    }
}