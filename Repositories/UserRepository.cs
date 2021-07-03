using System.Linq;
using System.Threading.Tasks;
using Agenda.Data;
using Agenda.Models;
using AgendaEventos.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Agenda.Repositories
{
    public class UserRepository : IUserRepository
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

        public async Task<bool> EmailExist(User user)
        {
            string _user = await _context.Users.Where(x => x.Email.Equals(user.Email)).Select(s => s.Email).FirstOrDefaultAsync();
            if (string.IsNullOrEmpty(_user))
                return false;

            return true;
        }
    }
}