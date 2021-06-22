using System.Collections.Generic;
using Agenda.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgendaEventos.Models
{
    [Table(nameof(User))]
    public class User : Entity
    {
        [Required(ErrorMessage = "Esse campo e obrigatorio")]
        [MaxLength(100, ErrorMessage = "Esse campo deve conter entre 3 e 100 caracteres.")]
        [MinLength(3, ErrorMessage = "Esse campo deve conter entre 3 e 100 caracteres.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Esse campo e obrigatorio")]
        [MaxLength(60, ErrorMessage = "Esse campo deve conter entre 3 e 60 caracteres.")]
        [MinLength(3, ErrorMessage = "Esse campo deve conter entre 3 e 60 caracteres.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Esse campo e obrigatorio")]
        [MaxLength(20, ErrorMessage = "Esse campo deve conter entre 3 e 20 caracteres.")]
        [MinLength(3, ErrorMessage = "Esse campo deve conter entre 3 e 20 caracteres.")]
        public string Password { get; set; }
        
        [NotMapped]
        public int RoleId { get; set; }
        public IList<Role> Roles { get; set; }
    }
}