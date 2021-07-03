using System.Threading.Tasks;
using Agenda.Models;

namespace AgendaEventos.Services.Interfaces
{
    public interface IUserService
    {
         Task<bool> CheckIfEmailAlreadyRegistered(User user);
    }
}