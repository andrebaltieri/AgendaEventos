using System.Threading.Tasks;
using Agenda.Data;
using Agenda.Models;
using AgendaEventos.Repositories.Interface;
using AgendaEventos.Services.Interfaces;

namespace Agenda.Services
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;
        private readonly IUserRepository _userRepository;

        public UserService(DataContext context, IUserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }

        public async Task<bool> CheckIfEmailAlreadyRegistered(User user) => await _userRepository.EmailExist(user);
    }
}