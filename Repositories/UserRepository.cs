using System.Threading.Tasks;
using Agenda.Data;
using Agenda.Models;
using Microsoft.EntityFrameworkCore;

namespace AgendaEventos.Repositories
{
    public class UserRepository
    {   
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<User> Authenticate(string email, string password)
        {
            return await _context
                    .Users
                    .Include(x => x.Roles)
                    .SingleOrDefaultAsync(x => x.Email.Equals(email) && x.Password.Equals(password));            
        }
    }
}