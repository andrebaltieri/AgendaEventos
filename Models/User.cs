using System.Collections.Generic;
using Agenda.Models;

namespace AgendaEventos.Models
{
    public class User : Entity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public IList<Role> Roles { get; set; }
    }
}