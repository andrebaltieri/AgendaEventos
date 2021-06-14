using AgendaEventos.Models;
using System.Collections.Generic;

namespace Agenda.Models
{
    public class Role : Entity
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public IList<User> Users { get; set; }
    }
}