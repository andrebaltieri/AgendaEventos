using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agenda.Models
{
    [Table(nameof(User))]
    public class User : Entity
    {
        [Required(ErrorMessage = "Esse campo é obrigatorio")]
        [EmailAddress(ErrorMessage = "Informe um endereço de e-mail válido.")]
        [MaxLength(254, ErrorMessage = "Esse campo deve conter no máximo 254 caracteres.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Esse campo é obrigatorio")]
        [MaxLength(100, ErrorMessage = "Esse campo deve conter entre 3 e 100 caracteres.")]
        [MinLength(3, ErrorMessage = "Esse campo deve conter entre 3 e 100 caracteres.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Esse campo é obrigatorio")]
        [MaxLength(20, ErrorMessage = "Esse campo deve conter entre 3 e 20 caracteres.")]
        [MinLength(3, ErrorMessage = "Esse campo deve conter entre 3 e 20 caracteres.")]
        public string Password { get; set; }

        [MinLength(1, ErrorMessage = "Informe um perfil para o usuário")]
        public virtual ICollection<Role> Roles { get; set; }
    }
}