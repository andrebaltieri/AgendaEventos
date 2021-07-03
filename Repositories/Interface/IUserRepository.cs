using System.Threading.Tasks;
using Agenda.Models;

namespace AgendaEventos.Repositories.Interface
{
    public interface IUserRepository
    {
        Task<User> Authenticate(string email, string password);
        Task<bool> EmailExist(User user);
    }
}