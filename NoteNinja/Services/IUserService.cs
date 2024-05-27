using System.Threading.Tasks;
using NoteNinja.Models;

namespace NoteNinja.Services
{
    public interface IUserService
    {
        Task<User> ValidateUserAsync(string email, string password);
        Task CreateAsync(User user);
    }
}
